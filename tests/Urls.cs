using System.Linq;
using System.Threading.Tasks;
using Mandrill;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "urls")]
    [Collection("urls")]
    public class Urls(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual Task DisposeAsync() => Task.CompletedTask;

        [Trait("Category", "urls/tracking-domains.json")]
        public class TrackingDomains : Urls
        {
            public TrackingDomains(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_tracking_domains()
            {
                var results = await Api.Urls.TrackingDomainsAsync();
                Assert.NotNull(results);
                var found = results.FirstOrDefault();
                if (found != null)
                {
                    Assert.False(string.IsNullOrEmpty(found.Domain));
                }
                else
                {
                    Output.WriteLine("no tracking domains found.");
                }
            }
        }

        [Trait("Category", "urls/add-tracking-domain.json")]
        public class AddTrackingDomain : Urls
        {
            public AddTrackingDomain(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact(Skip = "Adds a tracking domain that requires manual cleanup")]
            public async Task Can_add_tracking_domain()
            {
                var result = await Api.Urls.AddTrackingDomainAsync("tracking.mandrilldotnet.org");
                Assert.NotNull(result);
                Assert.Equal("tracking.mandrilldotnet.org", result.Domain);
            }
        }

        [Trait("Category", "urls/check-tracking-domain.json")]
        public class CheckTrackingDomain : Urls
        {
            public CheckTrackingDomain(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_check_existing_tracking_domain()
            {
                var domains = await Api.Urls.TrackingDomainsAsync();
                var first = domains.FirstOrDefault();
                if (first != null)
                {
                    var result = await Api.Urls.CheckTrackingDomainAsync(first.Domain);
                    Assert.NotNull(result);
                    Assert.Equal(first.Domain, result.Domain);
                }
                else
                {
                    Output.WriteLine("no tracking domains found, skipping check test.");
                }
            }
        }
    }
}
