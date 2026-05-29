using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mandrill;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "inbound")]
    [Collection("inbound")]
    public class Inbound(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;
        private HashSet<string> _added = new();

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual async Task DisposeAsync()
        {
            foreach (var id in _added)
            {
                await Api.Inbound.DeleteDomainAsync(id);
                Output.WriteLine($"inbound domain deleted: {id}");
            }
        }

        [Trait("Category", "inbound/domains")]
        public class Domains : Inbound
        {
            public Domains(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_get_domains()
            {
                var results = await Api.Inbound.DomainsAsync();
                Assert.True(results.Count >= 0);
            }


            [Fact]
            public async Task Can_add_domain()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                var results = await Api.Inbound.AddDomainAsync(domain);
                _added.Add(results.Domain);

                Assert.Equal(domain, results.Domain);
            }

            [Fact]
            public async Task Can_check_domain()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.CheckDomainAsync(domain);

                Assert.False(result.ValidMx);

            }

            [Fact]
            public async Task Can_delete_domain()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);

                var results = await Api.Inbound.DeleteDomainAsync(domain);
                Assert.Equal(domain, results.Domain);
            }
        }

        [Trait("Category", "inbound/routes")]
        public class Routes : Inbound
        {
            protected Uri WebhookUri { get; set; }

            public Routes(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output)
            {
                var configuredWebHook = Environment.GetEnvironmentVariable("MANDRILL_INBOUND_WEBHOOK") ?? "https://httpbin.org/status/200";

                WebhookUri = new UriBuilder(configuredWebHook) { Query = "id=" + Guid.NewGuid().ToString("N") }.Uri;
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
                Assert.NotEmpty(results);
                Assert.Equal(route.Id, results[0].Id);
            }

            [Fact(Skip = "Webhook validation is flaky")]
            public async Task Can_add_route()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.AddRouteAsync(domain, "*", WebhookUri);

                Assert.NotNull(result.Id);
                Assert.Equal(WebhookUri, result.Url);
            }

            [Fact(Skip = "Webhook validation is flaky")]
            public async Task Can_update_route()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.AddRouteAsync(domain, "*", WebhookUri);
                Assert.NotNull(result.Id);
                Assert.Equal(WebhookUri, result.Url);
                var id = result.Id;

                var newpattern = string.Format("{0:N}-*", Guid.NewGuid());

                result = await Api.Inbound.UpdateRouteAsync(id, newpattern, WebhookUri);
                Assert.Equal(id, result.Id);
                Assert.Equal(newpattern, result.Pattern);
            }


            [Fact(Skip = "Webhook validation is flaky")]
            public async Task Can_delete_route()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var result = await Api.Inbound.AddRouteAsync(domain, "*", WebhookUri);
                Assert.NotNull(result.Id);
                Assert.Equal(WebhookUri, result.Url);
                var id = result.Id;


                result = await Api.Inbound.DeleteRouteAsync(id);
                Assert.Equal(id, result.Id);
            }

            [Fact(Skip = "Webhook validation is flaky")]
            public async Task Can_send_raw()
            {
                var domain = string.Format("{0:N}.mandrilldotnet.org", Guid.NewGuid());
                await Api.Inbound.AddDomainAsync(domain);
                _added.Add(domain);

                var id = Guid.NewGuid().ToString("N");
                var pattern = string.Format("{0}-*", id);
                var result = await Api.Inbound.AddRouteAsync(domain, pattern, WebhookUri);
                Assert.NotNull(result.Id);
                Assert.Equal(WebhookUri, result.Url);


                var email = string.Format(id + "-@" + domain);
                var raw = string.Format(@"From: sender@mandrilldotnet.org\nTo: {0}\nSubject: Some Subject\n\nSome content.", email);

                var responses = await Api.Inbound.SendRawAsync(raw, new[] { email });

                Assert.NotEmpty(responses);
                Assert.EndsWith(domain, responses[0].Email);
                Assert.Equal(WebhookUri, responses[0].Url);
                Assert.Equal(pattern, responses[0].Pattern);
            }
        }
    }
}
