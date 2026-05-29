using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "mctemplates")]
    [Collection("mctemplates")]
    public class McTemplates(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual Task DisposeAsync() => Task.CompletedTask;

        [Trait("Category", "mctemplates/list.json")]
        public class List : McTemplates
        {
            public List(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_templates()
            {
                var results = await Api.McTemplates.ListAsync();
                results.Should().NotBeNull();
                var found = results.FirstOrDefault();
                if (found != null)
                {
                    found.McTemplateName.Should().NotBeNullOrEmpty();
                }
                else
                {
                    Output.WriteLine("no MC templates found.");
                }
            }

            [Fact]
            public async Task Can_list_with_search_query()
            {
                var results = await Api.McTemplates.ListAsync(searchQuery: "nonexistent-template-xyz");
                results.Should().NotBeNull();
                results.Count.Should().Be(0);
            }
        }

        [Trait("Category", "mctemplates/info.json")]
        public class Info : McTemplates
        {
            public Info(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_get_info_for_existing_template()
            {
                var templates = await Api.McTemplates.ListAsync();
                var first = templates.FirstOrDefault();
                if (first != null)
                {
                    var result = await Api.McTemplates.InfoAsync(first.McTemplateId);
                    result.Should().NotBeNull();
                    result.McTemplateId.Should().Be(first.McTemplateId);
                    result.McTemplateName.Should().Be(first.McTemplateName);
                }
                else
                {
                    Output.WriteLine("no MC templates found, skipping info test.");
                }
            }
        }

        [Trait("Category", "mctemplates/time-series.json")]
        public class TimeSeries : McTemplates
        {
            public TimeSeries(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_get_time_series()
            {
                var templates = await Api.McTemplates.ListAsync();
                var first = templates.FirstOrDefault();
                if (first != null)
                {
                    var results = await Api.McTemplates.TimeSeriesAsync(first.McTemplateId);
                    results.Should().NotBeNull();
                }
                else
                {
                    Output.WriteLine("no MC templates found, skipping time series test.");
                }
            }
        }
    }
}
