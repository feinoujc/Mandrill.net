using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "tags")]
    [Collection("tags")]
    public class Tags : IntegrationTest
    {
        public Tags(ITestOutputHelper output) : base(output)
        {
        }

        [Trait("Category", "tags/list.json")]
        public class List : Tags
        {
            public List(ITestOutputHelper output) : base(output)
            {
            }

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
                    Output.WriteLine("no tags found.");
                }
            }
        }

        [Trait("Category", "tags/info.json")]
        public class Info : Tags
        {
            public Info(ITestOutputHelper output) : base(output)
            {
            }

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
                    Output.WriteLine("no tags found");
                }
            }
        }

        [Trait("Category", "tags/delete.json")]
        public class Delete : Tags
        {
            public Delete(ITestOutputHelper output) : base(output)
            {
            }

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
                    Output.WriteLine("no tags found");
                }
            }
        }

        [Trait("Category", "tags/time_series.json")]
        public class TimeSeries : Tags
        {
            public TimeSeries(ITestOutputHelper output) : base(output)
            {
            }

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
                        Output.WriteLine("no time series found.");
                    }
                }
                else
                {
                    Output.WriteLine("no tags found");
                }
            }
        }

        [Trait("Category", "tags/all_time_series.json")]
        public class AllTimeSeries : Tags
        {
            public AllTimeSeries(ITestOutputHelper output) : base(output)
            {
            }

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
                    Output.WriteLine("no time series found.");
                }
            }
        }
    }
}
