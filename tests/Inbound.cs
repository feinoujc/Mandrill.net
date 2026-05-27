using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "inbound")]
    [Collection("inbound")]
    public class Inbound(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        private readonly HashSet<string> _added = [];

        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual async Task DisposeAsync()
        {
            foreach (var id in _added)
            {
                await Api.Inbound.DeleteDomainAsync(id);
                Output.WriteLine("inbound domain deleted: " + id);
            }
        }

        [Trait("Category", "inbound/domains")]
        public class Domains(MandrillFixture fixture, ITestOutputHelper output) : Inbound(fixture, output)
        {
            [Fact]
            public async Task Can_get_domains()
            {
                var results = await Api.Inbound.DomainsAsync();
                results.Count.Should().BeGreaterOrEqualTo(0);
            }

            [Fact]
            public async Task Can_add_domain()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                var results = await Api.Inbound.AddDomainAsync(domain);
                _added.Add(results.Domain);
                results.Domain.Should().Be(domain);
            }

            [Fact]
            public async Task Can_check_domain()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.CheckDomainAsync(domain);
                result.ValidMx.Should().Be(false);
            }

            [Fact]
            public async Task Can_delete_domain()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);

                var results = await Api.Inbound.DeleteDomainAsync(domain);
                results.Domain.Should().Be(domain);
            }
        }

        [Trait("Category", "inbound/routes")]
        public class Routes : Inbound
        {
            protected Uri WebhookUrl { get; }

            public Routes(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output)
            {
                var configured = Environment.GetEnvironmentVariable("MANDRILL_INBOUND_WEBHOOK") ?? "https://httpbin.org/status/200";
                WebhookUrl = new UriBuilder(configured) { Query = "id=" + Guid.NewGuid().ToString("N") }.Uri;
            }

            public string SendingDomain { get; set; }

            public override async Task DisposeAsync()
            {
                var webhooks = await Api.WebHooks.ListAsync();
                var webhook = webhooks.Single(x => x.Url == WebhookUrl);
                await Api.WebHooks.DeleteAsync(webhook.Id);
                await base.DisposeAsync();
            }

            [Fact]
            public async Task Can_get_routes()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var route = await Api.Inbound.AddRouteAsync(domain, domain, WebhookUrl.ToString());

                var results = await Api.Inbound.RoutesAsync(domain);
                results.Should().NotBeEmpty();
                results[0].Id.Should().Be(route.Id);
            }

            [Fact]
            public async Task Can_add_route()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.AddRouteAsync(domain, "*", WebhookUrl.ToString());
                result.Id.Should().NotBeNull();
                result.Url.Should().Be(WebhookUrl);
            }

            [Fact]
            public async Task Can_update_route()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.AddRouteAsync(domain, "*", WebhookUrl.ToString());
                result.Id.Should().NotBeNull();
                result.Url.Should().Be(WebhookUrl);
                var id = result.Id;

                var newpattern = string.Format("{0:N}-*", Guid.NewGuid());
                result = await Api.Inbound.UpdateRouteAsync(id, newpattern, WebhookUrl.ToString());
                result.Id.Should().Be(id);
                result.Pattern.Should().Be(newpattern);
            }

            [Fact]
            public async Task Can_delete_route()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.AddRouteAsync(domain, "*", WebhookUrl.ToString());
                result.Id.Should().NotBeNull();
                result.Url.Should().Be(WebhookUrl.ToString());
                var id = result.Id;

                result = await Api.Inbound.DeleteRouteAsync(id);
                result.Id.Should().Be(id);
            }

            [Fact]
            public async Task Can_send_raw()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var id = Guid.NewGuid().ToString("N");
                var pattern = string.Format("{0}-*", id);
                var result = await Api.Inbound.AddRouteAsync(domain, pattern, WebhookUrl.ToString());
                result.Id.Should().NotBeNull();
                result.Url.Should().Be(WebhookUrl.ToString());

                var email = string.Format(id + "-@" + domain);
                var raw = string.Format(@"From: sender@mandrilldotnet.org\nTo: {0}\nSubject: Some Subject\n\nSome content.", email);

                var response = await Api.Inbound.SendRawAsync(raw, new List<string> { email });

                response.Should().NotBeNullOrEmpty();
                var first = response!.First();
                first.Email.Should().EndWith(domain);
                first.Url.Should().Be(WebhookUrl);
                first.Pattern.Should().Be(pattern);
            }
        }
    }
}
