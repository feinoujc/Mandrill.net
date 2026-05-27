using System;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Mandrill.Model;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "users")]
    public class Users(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual Task DisposeAsync() => Task.CompletedTask;

        [Trait("Category", "users/info.json")]
        public class Info(MandrillFixture fixture, ITestOutputHelper output) : Users(fixture, output)
        {
            [Fact]
            public async Task Can_get_info()
            {
                var result = await Api.Users.InfoAsync();

                result.CreatedAt.Should().BeBefore(DateTime.UtcNow);
                result.Username.Should().NotBeNullOrEmpty();

                result.Stats.Should().NotBeNull();
                result.Stats.Today.Should().NotBeNull();
                result.Stats.Last7Days.Should().NotBeNull();
                result.Stats.Last30Days.Should().NotBeNull();
                result.Stats.Last60Days.Should().NotBeNull();
                result.Stats.Last90Days.Should().NotBeNull();
                result.Stats.AllTime.Should().NotBeNull();
            }
        }

        [Trait("Category", "users/ping2.json")]
        public class Ping(MandrillFixture fixture, ITestOutputHelper output) : Users(fixture, output)
        {
            [Fact]
            public async Task Can_ping()
            {
                var ping = await Api.Users.PingAsync();
                ping.Should().Be("PONG!");
            }

            [Fact]
            public async Task Throws_when_invalid_key()
            {
                var badApi = new MandrillApi(Guid.NewGuid().ToString("N"));
                var mandrillExpection = await Assert.ThrowsAsync<MandrillException>(() => badApi.Users.PingAsync());
                mandrillExpection.Name.Should().Be("Invalid_Key");
            }
        }

        [Trait("Category", "users/senders.json")]
        public class Senders(MandrillFixture fixture, ITestOutputHelper output) : Users(fixture, output)
        {
            [Fact]
            public async Task Can_list_senders()
            {
                var results = await Api.Users.SendersAsync();

                if (results.Count == 0)
                    Output.WriteLine("No senders returned");

                foreach (var sender in results)
                {
                    sender.Address.Should().NotBeNullOrEmpty();
                    sender.CreatedAt.Should().BeBefore(DateTime.UtcNow);
                }
            }
        }
    }
}
