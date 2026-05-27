using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "senders")]
    [Collection("senders")]
    public class Senders(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual Task DisposeAsync() => Task.CompletedTask;

        [Trait("Category", "senders/list.json")]
        public class List(MandrillFixture fixture, ITestOutputHelper output) : Senders(fixture, output)
        {
            [Fact]
            public async Task Can_list_all()
            {
                var results = await Api.Senders.ListAsync();

                var found = results.OrderBy(x => x.Address).FirstOrDefault();
                if (found != null)
                    results.Count.Should().BeGreaterOrEqualTo(1);
                else
                    Output.WriteLine("no senders found.");
            }
        }

        [Trait("Category", "senders/domains.json")]
        public class Domains(MandrillFixture fixture, ITestOutputHelper output) : Senders(fixture, output)
        {
            [Fact]
            public async Task Can_list_sender_domains()
            {
                var results = await Api.Senders.DomainsAsync();

                var found = results.OrderBy(x => x.CreatedAt).FirstOrDefault();
                if (found != null)
                    results.Count.Should().BeGreaterOrEqualTo(1);
                else
                    Output.WriteLine("no sender domains found.");
            }
        }

        [Trait("Category", "senders/add_domain.json")]
        public class Add(MandrillFixture fixture, ITestOutputHelper output) : Senders(fixture, output)
        {
            [Fact(Skip = "no way to delete an added domain")]
            public async Task Can_add_domain()
            {
                var domain = Guid.NewGuid().ToString("N") + "mandrilldotnet.org";
                var result = await Api.Senders.AddDomainAsync(domain);
                result.Domain.Should().Contain(domain);
            }
        }

        [Trait("Category", "senders/check_domain.json")]
        public class Check(MandrillFixture fixture, ITestOutputHelper output) : Senders(fixture, output)
        {
            [Fact(Skip = "No way to delete an added domain")]
            public async Task Can_check_domain()
            {
                var domain = Guid.NewGuid().ToString("N") + "mandrilldotnet.org";
                await Api.Senders.AddDomainAsync(domain);
                var result = await Api.Senders.CheckDomainAsync(domain);
                result.Domain.Should().Contain(domain);
            }
        }

        [Trait("Category", "senders/verify_domain.json")]
        public class Verify(MandrillFixture fixture, ITestOutputHelper output) : Senders(fixture, output)
        {
            [Fact(Skip = "No way to delete an added domain")]
            public async Task Can_verify_domain()
            {
                var domain = Guid.NewGuid().ToString("N") + "mandrilldotnet.org";
                var mailbox = "testmailbox";
                await Api.Senders.AddDomainAsync(domain);
                var result = await Api.Senders.VerifyDomainAsync(domain, mailbox);
                result.Domain.Should().Contain(domain);
            }
        }

        [Trait("Category", "senders/info.json")]
        public class Info(MandrillFixture fixture, ITestOutputHelper output) : Senders(fixture, output)
        {
            [Fact]
            public async Task Can_retrieve_info()
            {
                var address = (await Api.Senders.ListAsync()).LastOrDefault();
                if (address != null)
                {
                    var result = await Api.Senders.InfoAsync(address.Address);
                    result.Should().NotBeNull();
                    result.Address.Should().Be(address.Address);
                }
                else
                {
                    Output.WriteLine("no address found");
                }
            }
        }

        [Trait("Category", "senders/time_series.json")]
        public class TimeSeries(MandrillFixture fixture, ITestOutputHelper output) : Senders(fixture, output)
        {
            [Fact]
            public async Task Can_get_sender_time_series()
            {
                var address = (await Api.Senders.ListAsync()).LastOrDefault();
                if (address != null)
                {
                    var results = await Api.Senders.TimeSeriesAsync(address.Address);

                    var found = results.OrderBy(x => x.Time).FirstOrDefault();
                    if (found != null)
                        results.Count.Should().BeGreaterOrEqualTo(1);
                    else
                        Output.WriteLine("no time series found.");
                }
                else
                {
                    Output.WriteLine("no address found");
                }
            }
        }
    }
}
