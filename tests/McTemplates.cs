using System.Linq;
using System.Threading.Tasks;
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
                Assert.NotNull(results);
                var found = results.FirstOrDefault();
                if (found != null)
                {
                    Assert.False(string.IsNullOrEmpty(found.McTemplateName));
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
                Assert.NotNull(results);
                Assert.Empty(results);
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
                    Assert.NotNull(result);
                    Assert.Equal(first.McTemplateId, result.McTemplateId);
                    Assert.Equal(first.McTemplateName, result.McTemplateName);
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
                    Assert.NotNull(results);
                }
                else
                {
                    Output.WriteLine("no MC templates found, skipping time series test.");
                }
            }
        }
    }
}
