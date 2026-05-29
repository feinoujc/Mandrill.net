using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
                results.Should().NotBeNull();
                var found = results.FirstOrDefault();
                if (found != null)
                {
                    found.Domain.Should().NotBeNullOrEmpty();
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
                result.Should().NotBeNull();
                result.Domain.Should().Be("tracking.mandrilldotnet.org");
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
                    result.Should().NotBeNull();
                    result.Domain.Should().Be(first.Domain);
                }
                else
                {
                    Output.WriteLine("no tracking domains found, skipping check test.");
                }
            }
        }
    }
}
