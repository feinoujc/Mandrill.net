using System;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    class Exports : IntegrationTest
    {
        [Category("exports/list.json")]
        class List : Exports
        {
            [Test]
            public async Task Can_list_all()
            {
                var results = await Api.Exports.ListAsync();

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.Id).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Assert.Inconclusive("no exports found.");
                }
            }
        }

        [Category("exports/info.json")]
        class Info : Exports
        {
            [Test]
            public async Task Can_retrieve_info()
            {
                var export = (await Api.Exports.ListAsync()).LastOrDefault();
                if (export != null)
                {
                    var result = await Api.Exports.InfoAsync(export.Id);
                    result.Should().NotBeNull();
                    result.Id.Should().Be(export.Id);
                }
                else
                {
                    Assert.Inconclusive("no exports found");
                }
            }
        }

        [Category("exports/rejects.json")]
        class Rejects : Exports
        {
            [Test]
            public async Task Can_export_info()
            {
                // notifyEmail is an optional field that will 
                // be emailed when the export is done compiling. omitting for test purposes.
                string notifyEmail = string.Empty;
                var result = await Api.Exports.RejectsAsync(notifyEmail);
                result.Should().NotBeNull();
                result.Type.Should().Be("reject");
                result.State.Should().Be("waiting");
            }
        }

        [Category("exports/whitelist.json")]
        class Whitelist: Exports
        {
            [Test]
            public async Task Can_export_info()
            {
                // notifyEmail is an optional field that will 
                // be emailed when the export is done compiling. omitting for test purposes.
                string notifyEmail = string.Empty;
                var result = await Api.Exports.WhitelistAsync(notifyEmail);
                result.Should().NotBeNull();
                result.Type.Should().Be("whitelist");
                result.State.Should().Be("waiting");
            }
        }

        [Category("exports/activity.json")]
        class Activity : Exports
        {
            [Test]
            public async Task Can_export_activity()
            {
                // all of the activity parameters are optional. unsure of how much test
                // coverage you'd like here, so stubbed out the parameters to be filled
                // as desired.
                string notifyEmail = string.Empty;
                DateTime? dateFrom = null;
                DateTime? dateTo = null;
                IList<string> tags = null;
                IList<string> senders = null;
                IList<string> states = null;
                IList<string> apiKeys = null;

                var result = await Api.Exports.ActivityAsync(notifyEmail,
                    dateFrom,
                    dateTo,
                    tags,
                    senders,
                    states,
                    apiKeys);
                result.Should().NotBeNull();
                result.Type.Should().Be("activity");
                result.State.Should().Be("waiting");
            }
        }

    }
}
