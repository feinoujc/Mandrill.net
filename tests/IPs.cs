using System.Linq;
using System.Threading.Tasks;
using Mandrill;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "ips")]
    [Collection("ips")]
    public class IPs(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual Task DisposeAsync() => Task.CompletedTask;

        [Trait("Category", "ips/list.json")]
        public class List : IPs
        {
            public List(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_ips()
            {
                var results = await Api.Ips.ListAsync();
                Assert.NotNull(results);
                var found = results.FirstOrDefault();
                if (found != null)
                {
                    Assert.False(string.IsNullOrEmpty(found.Ip));
                }
                else
                {
                    Output.WriteLine("no dedicated IPs provisioned.");
                }
            }
        }

        [Trait("Category", "ips/list-pools.json")]
        public class ListPools : IPs
        {
            public ListPools(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_pools()
            {
                var results = await Api.Ips.ListPoolsAsync();
                Assert.NotNull(results);
                var found = results.FirstOrDefault();
                if (found != null)
                {
                    Assert.False(string.IsNullOrEmpty(found.Name));
                }
                else
                {
                    Output.WriteLine("no IP pools found.");
                }
            }
        }

        [Trait("Category", "ips/provision.json")]
        public class Provision : IPs
        {
            public Provision(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact(Skip = "Provisions a real dedicated IP which has a cost")]
            public async Task Can_provision_ip()
            {
                var result = await Api.Ips.ProvisionAsync(warmup: true);
                Assert.NotNull(result);
            }
        }

        [Trait("Category", "ips/create-pool.json")]
        public class CreatePool : IPs
        {
            public CreatePool(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact(Skip = "Creates a pool that requires manual cleanup")]
            public async Task Can_create_pool()
            {
                var result = await Api.Ips.CreatePoolAsync("test-pool");
                Assert.NotNull(result);
                Assert.Equal("test-pool", result.Name);
            }
        }
    }
}
