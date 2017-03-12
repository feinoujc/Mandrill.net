using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Tests
{
    [Trait("Category", "tags")]
    [Collection("tags")]
    public class Tags : IntegrationTest
    {

        [Trait("Category", "tags/list.json")]
        public class List : Tags
        {
            [Fact]
            public async Task Can_list_all()
            {
                var results = await Api.Tags.ListAsync();

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.Tag).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Console.Error.WriteLine("no tags found.");
                }
            }
        }

        [Trait("Category", "tags/info.json")]
        public class Info : Tags
        {
            [Fact]
            public async Task Can_retrieve_info()
            {
                var tag = (await Api.Tags.ListAsync()).LastOrDefault();
                if (tag != null)
                {
                    var result = await Api.Tags.InfoAsync(tag.Tag);
                    result.Should().NotBeNull();
                    result.Tag.Should().Be(tag.Tag);
                }
                else
                {
                    Console.Error.WriteLine("no tags found");
                }
            }
        }

        [Trait("Category", "tags/delete.json")]
        public class Delete : Tags
        {
            [Fact]
            public async Task Can_delete_tag()
            {
                var tag = (await Api.Tags.ListAsync()).LastOrDefault();
                if (tag != null)
                {
                    var result = await Api.Tags.DeleteAsync(tag.Tag);
                    result.Should().NotBeNull();
                    result.Tag.Should().Be(tag.Tag);
                }
                else
                {
                    Console.Error.WriteLine("no tags found");
                }
            }
        }

        [Trait("Category", "tags/time_series.json")]
        public class TimeSeries : Tags
        {
            [Fact]
            public async Task Can_get_tag_time_series()
            {
                var tag = (await Api.Tags.ListAsync()).LastOrDefault();
                if (tag != null)
                {
                    var results = await Api.Tags.TimeSeriesAsync(tag.Tag);

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
                    Console.Error.WriteLine("no tags found");
                }
            }
        }

        [Trait("Category", "tags/all_time_series.json")]
        public class AllTimeSeries : Tags
        {
            [Fact]
            public async Task Can_get_tag_all_time_series()
            {
                var results = await Api.Tags.AllTimeSeriesAsync();

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
        }
    }
}
