using System;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Model;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "users")]
    [Collection("users")]
    public class Users(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual Task DisposeAsync() => Task.CompletedTask;

        [Trait("Category", "users/info.json")]
        public class Info : Users
        {
            public Info(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_get_info()
            {
                var result = await Api.Users.InfoAsync();
                Assert.True(result.CreatedAt < DateTime.UtcNow);
                Assert.False(string.IsNullOrEmpty(result.Username));
                Assert.NotNull(result.Stats);
                Assert.NotNull(result.Stats.Today);
                Assert.NotNull(result.Stats.Last7Days);
                Assert.NotNull(result.Stats.Last30Days);
                Assert.NotNull(result.Stats.Last60Days);
                Assert.NotNull(result.Stats.Last90Days);
                Assert.NotNull(result.Stats.AllTime);
            }
        }

        [Trait("Category", "users/ping2.json")]
        public class Ping : Users
        {
            public Ping(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_ping()
            {
                var ping = await Api.Users.PingAsync();
                Assert.Equal("PONG!", ping);
            }

            [Fact]
            public async Task Can_ping2()
            {
                var result = await Api.Users.Ping2Async();
                Assert.NotNull(result);
                Assert.Equal("PONG!", result.Ping);
            }

            [Fact]
            public async Task Throws_when_invalid_key()
            {
                var badApi = new MandrillApi(Guid.NewGuid().ToString("N"));
                var mandrillExpection = await Assert.ThrowsAsync<MandrillException>(() => badApi.Users.PingAsync());
                Assert.Equal("Invalid_Key", mandrillExpection.Name);
            }
        }

        [Trait("Category", "users/senders.json")]
        public class Senders : Users
        {
            public Senders(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_senders()
            {
                var results = await Api.Users.SendersAsync();
                if (results.Count == 0)
                {
                    Output.WriteLine("No senders returned");
                }
                foreach (var sender in results)
                {
                    Assert.False(string.IsNullOrEmpty(sender.Address));
                    Assert.True(sender.CreatedAt < DateTime.UtcNow);
                }
            }
        }
    }
}
