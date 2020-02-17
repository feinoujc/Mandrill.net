using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "inbound")]
    [Collection("inbound")]
    public class Inbound : IntegrationTest
    {
        private HashSet<string> _added = new HashSet<string>();

        public Inbound(ITestOutputHelper output) : base(output)
        {
        }

        public override void Dispose()
        {
            foreach (var id in _added)
            {
                Api.Inbound.DeleteDomainAsync(id).GetAwaiter().GetResult();
                Output.WriteLine("inbound domain deleted: " + id);
            }
            base.Dispose();
        }

        [Trait("Category", "inbound/domains")]
        public class Domains : Inbound
        {
            public Domains(ITestOutputHelper output) : base(output)
            {
            }

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
            protected Uri WebhookUri { get; set; }

            public Routes(ITestOutputHelper output) : base(output)
            {
                var configuredWebHook = Environment.GetEnvironmentVariable("MANDRILL_INBOUND_WEBHOOK") ?? "https://reqres.in/api/mandrill-webhook-test";

                WebhookUri = new UriBuilder(configuredWebHook) { Query = "id=" + Guid.NewGuid().ToString("N") }.Uri;

                //configure webhook api at http://requestb.in
            }
            public override void Dispose()
            {
                var webhook = Api.WebHooks.ListAsync().GetAwaiter().GetResult().Single(x => x.Url == WebhookUri).Id;
                Api.WebHooks.DeleteAsync(webhook).GetAwaiter().GetResult();
                base.Dispose();
            }

            public string SendingDomain { get; set; }

            [Fact]
            public async Task Can_get_routes()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var route = await Api.Inbound.AddRouteAsync(domain, domain, WebhookUri);

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

                var result = await Api.Inbound.AddRouteAsync(domain, "*", WebhookUri);

                result.Id.Should().NotBeNull();
                result.Url.Should().Be(WebhookUri);
            }

            [Fact]
            public async Task Can_update_route()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.AddRouteAsync(domain, "*", WebhookUri);
                result.Id.Should().NotBeNull();
                result.Url.Should().Be(WebhookUri);
                var id = result.Id;

                var newpattern = string.Format("{0:N}-*", Guid.NewGuid());

                result = await Api.Inbound.UpdateRouteAsync(id, newpattern, WebhookUri);
                result.Id.Should().Be(id);
                result.Pattern.Should().Be(newpattern);
            }


            [Fact]
            public async Task Can_delete_route()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.AddRouteAsync(domain, "*", WebhookUri);
                result.Id.Should().NotBeNull();
                result.Url.Should().Be(WebhookUri);
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
                var result = await Api.Inbound.AddRouteAsync(domain, pattern, WebhookUri);
                result.Id.Should().NotBeNull();
                result.Url.Should().Be(WebhookUri);


                var email = string.Format(id + "-@" + domain);
                var raw = string.Format(@"From: sender@mandrilldotnet.org\nTo: {0}\nSubject: Some Subject\n\nSome content.", email);

                var responses = await Api.Inbound.SendRawAsync(raw, new[] { email });

                responses.Should().NotBeEmpty();
                responses[0].Email.Should().EndWith(domain);
                responses[0].Url.Should().Be(WebhookUri);
                responses[0].Pattern.Should().Be(pattern);
            }
        }
    }
}
