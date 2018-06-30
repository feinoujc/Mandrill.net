using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Tests
{
    [Trait("Category", "senders")]
    [Collection("senders")]
    public class Senders : IntegrationTest
    {
        [Trait("Category", "senders/list.json")]
        public class List : Senders
        {
            [Fact]
            public async Task Can_list_all()
            {
                var results = await Api.Senders.ListAsync();

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.Address).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Console.Error.WriteLine("no senders found.");
                }
            }
        }

        [Trait("Category", "senders/domains.json")]
        public class Domains : Senders
        {
            [Fact]
            public async Task Can_list_sender_domains()
            {
                var results = await Api.Senders.DomainsAsync();

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.CreatedAt).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Console.Error.WriteLine("no sender domains found.");
                }
            }
        }

        [Trait("Category", "senders/add_domain.json")]
        public class Add : Senders
        {
            [Fact]
            public async Task Can_add_domain()
            {
                var domain = Guid.NewGuid().ToString("N") + "example.com";
                var result = await Api.Senders.AddDomainAsync(domain);
                result.Domain.Should().Contain(domain);
            }
        }

        [Trait("Category", "senders/check_domain.json")]
        public class Check : Senders
        {
            [Fact]
            public async Task Can_check_domain()
            {
                var domain = Guid.NewGuid().ToString("N") + "example.com";
                await Api.Senders.AddDomainAsync(domain);
                var result = await Api.Senders.CheckDomainAsync(domain);
                result.Domain.Should().Contain(domain);
            }
        }

        [Trait("Category", "senders/verify_domain.json")]
        public class Verify : Senders
        {
            [Fact]
            public async Task Can_verify_domain()
            {
                var domain = Guid.NewGuid().ToString("N") + "example.com";
                // Not sure the best way to stub a mailbox here. This call
                // sends a verification email to `mailbox`. Tested with a
                // valid mailbox to ensure it works.
                var mailbox = "testmailbox";
                await Api.Senders.AddDomainAsync(domain);
                var result = await Api.Senders.VerifyDomainAsync(domain, mailbox);
                result.Domain.Should().Contain(domain);
            }
        }

        [Trait("Category", "senders/info.json")]
        public class Info : Senders
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
                    Console.Error.WriteLine("no address found");
                }
            }
        }

        [Trait("Category", "senders/time_series.json")]
        public class TimeSeries : Senders
        {
            [Fact]
            public async Task Can_get_sender_time_series()
            {
                var address = (await Api.Senders.ListAsync()).LastOrDefault();
                if (address != null)
                {
                    var results = await Api.Senders.TimeSeriesAsync(address.Address);

                    //the api doesn't return results immediately, it may return no results
                    var found = results.OrderBy(x => x.Time).FirstOrDefault();
                    if (found != null)
                    {
                        results.Count.Should().BeGreaterOrEqualTo(1);
                    }
                    else
                    {
                        Console.Error.WriteLine("no time series found.");
                    }
                }
                else
                {
                    Console.Error.WriteLine("no address found");
                }
            }
        }
    }
}
