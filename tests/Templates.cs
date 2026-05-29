using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Model;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "templates")]
    [Collection("templates")]
    public class Templates(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;
        protected HashSet<string> TemplatesToCleanup = new();

        protected string AddToBeDeleted(string templateName)
        {
            TemplatesToCleanup.Add(templateName);
            return templateName;
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual async Task DisposeAsync()
        {
            if (TemplatesToCleanup != null)
            {
                foreach (var templateName in TemplatesToCleanup)
                {
                    await Api.Templates.DeleteAsync(templateName);
                }
                TemplatesToCleanup = null;
            }
        }

        [Trait("Category", "templates/add.json")]
        public class Add : Templates
        {
            public Add(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_add_template()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var result = await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);

                Assert.Equal(name, result.Name);
                Assert.Equal(TemplateContent.Code, result.Code);
                Assert.Equal(name, result.Slug);
                Assert.Equal(TemplateContent.Text, result.Text);
            }
        }

        [Trait("Category", "templates/add.json")]
        public class Info : Templates
        {
            public Info(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_get_template_info()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var added = await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);
                var result = await Api.Templates.InfoAsync(added.Name);

                Assert.Equal(name, result.Name);
                Assert.Equal(TemplateContent.Code, result.Code);
                Assert.Equal(name, result.Slug);
                Assert.Equal(TemplateContent.Text, result.Text);
            }
        }

        [Trait("Category", "templates/list.json")]
        public class List : Templates
        {
            public List(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_all_templates()
            {
                var testLabel = Guid.NewGuid().ToString("N");
                var templates = Enumerable.Range(1, 10).Select(i => AddToBeDeleted(Guid.NewGuid().ToString())).ToArray();
                foreach (var template in templates)
                {
                    await Api.Templates.AddAsync(template, TemplateContent.Code, TemplateContent.Text, false, labels: new[] { testLabel });
                }

                var results = await Api.Templates.ListAsync(testLabel);

                Assert.True(results.Count >= 10);
                Assert.Equal(10, results.Where(info => templates.Contains(info.Name)).Count());
            }

            [Fact]
            public async Task Can_list_templates()
            {
                var testLabel = Guid.NewGuid().ToString("N");
                var templates = Enumerable.Range(1, 10).Select(i => AddToBeDeleted(Guid.NewGuid().ToString())).ToArray();
                foreach (var template in templates)
                {
                    await Api.Templates.AddAsync(template, TemplateContent.Code, TemplateContent.Text, false, labels: new[] { testLabel });
                }

                var results = await Api.Templates.ListAsync(testLabel);

                Assert.Equal(10, results.Count);
                foreach (var x in results)
                {
                    Assert.NotNull(x.Labels);
                    Assert.NotEmpty(x.Labels);
                    Assert.Equal(testLabel, x.Labels[0]);
                }
            }
        }

        [Trait("Category", "templates/add.json")]
        public class Publish : Templates
        {
            public Publish(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_publish_template()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var now = DateTime.UtcNow;
                var skew = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc).AddSeconds(-60);

                var added = await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);
                var result = await Api.Templates.PublishAsync(added.Name);

                Assert.True(result.PublishedAt >= skew);
            }
        }

        [Trait("Category", "templates/render.json")]
        public class Render : Templates
        {
            public Render(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_render()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);

                var templateContent = new List<MandrillTemplateContent>
                {
                    new MandrillTemplateContent {Name = "footer", Content = "this is my footer"}
                };
                var mergeVars = new List<MandrillMergeVar>
                {
                    new MandrillMergeVar {Name = "fname", Content = "Joe"},
                    new MandrillMergeVar {Name = "ORDERDATE", Content = "11/28/2014"},
                    new MandrillMergeVar {Name = "INVOICEDETAILS", Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod"}
                };
                var result = await Api.Templates.RenderAsync(name, templateContent, mergeVars);

                Assert.False(string.IsNullOrWhiteSpace(result.Html));
                Assert.Contains("Joe", result.Html);
                Assert.Contains("11/28/2014", result.Html);
                Assert.Contains("Lorem ipsum dolor sit amet", result.Html);
            }
        }

        [Trait("Category", "templates/time_series.json")]
        public class TimeSeries : Templates
        {
            public TimeSeries(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_get_time_series()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var added = await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);
                var result = await Api.Templates.TimeSeriesAsync(added.Name);

                if (result.Count == 0)
                {
                    Output.WriteLine("time-series couldn't run for a new template");
                }
            }
        }

        [Trait("Category", "templates/update.json")]
        public class Update : Templates
        {
            public Update(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_update()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);
                var result = await Api.Templates.UpdateAsync(name, TemplateContent.Code.Replace("footer", "booger"), null, false,
                    "update@mandrilldotnet.org");
                Assert.Equal(name, result.Name);
                Assert.Contains("booger", result.Code);
                Assert.Equal(name, result.Slug);
                Assert.Equal("update@mandrilldotnet.org", result.FromEmail);
            }
        }
    }
}
