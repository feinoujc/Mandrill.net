using System;
using FluentAssertions;
using Mandrill;
using Mandrill.Model;
using NUnit.Framework;

namespace Tests
{
    [Category("users")]
    internal class Users : IntegrationTest
    {
        [Category("users/info.json")]
        internal class Info : Users
        {
            [Test]
            public async void Can_get_info()
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

        [Category("users/ping2.json")]
        internal class Ping : Users
        {
            [Test]
            public async void Can_ping()
            {
                var ping = await Api.Users.PingAsync();
                ping.Should().Be("PONG!");
            }

            [Test]
            public void Throws_when_invalid_key()
            {
                var badApi = new MandrillApi(Guid.NewGuid().ToString("N"));
                var mandrillExpection = Assert.Throws<MandrillException>(async () => await badApi.Users.PingAsync());
                mandrillExpection.Name.Should().Be("Invalid_Key");
            }
        }

        [Category("users/senders.json")]
        internal class Senders : Users
        {
            [Test]
            public async void Can_list_senders()
            {
                var results = await Api.Users.SendersAsync();

                if (results.Count == 0)
                {
                    Assert.Inconclusive("No senders returned");
                }

                foreach (var sender in results)
                {
                    sender.Address.Should().NotBeNullOrEmpty();
                    sender.CreatedAt.Should().BeBefore(DateTime.UtcNow);
                }
            }
        }
    }
}