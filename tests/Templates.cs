using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
        protected readonly HashSet<string> TemplatesToCleanup = [];

        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        protected string AddToBeDeleted(string templateName)
        {
            TemplatesToCleanup.Add(templateName);
            return templateName;
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual async Task DisposeAsync()
        {
            foreach (var templateName in TemplatesToCleanup)
                await Api.Templates.DeleteAsync(templateName);
        }

        [Trait("Category", "templates/add.json")]
        public class Add(MandrillFixture fixture, ITestOutputHelper output) : Templates(fixture, output)
        {
            [Fact]
            public async Task Can_add_template()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var result = await Api.Templates.AddAsync(name, code: TemplateContent.Code, text: TemplateContent.Text, publish: false);

                result.Name.Should().Be(name);
                result.Code.Should().Be(TemplateContent.Code);
                result.Slug.Should().Be(name);
                result.Text.Should().Be(TemplateContent.Text);
            }
        }

        [Trait("Category", "templates/add.json")]
        public class Info(MandrillFixture fixture, ITestOutputHelper output) : Templates(fixture, output)
        {
            [Fact]
            public async Task Can_get_template_info()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var added = await Api.Templates.AddAsync(name, code: TemplateContent.Code, text: TemplateContent.Text, publish: false);
                var result = await Api.Templates.InfoAsync(added.Name);

                result.Name.Should().Be(name);
                result.Code.Should().Be(TemplateContent.Code);
                result.Slug.Should().Be(name);
                result.Text.Should().Be(TemplateContent.Text);
            }
        }

        [Trait("Category", "templates/list.json")]
        public class List(MandrillFixture fixture, ITestOutputHelper output) : Templates(fixture, output)
        {
            [Fact]
            public async Task Can_list_all_templates()
            {
                var testLabel = Guid.NewGuid().ToString("N");
                var templates = Enumerable.Range(1, 10).Select(i => AddToBeDeleted(Guid.NewGuid().ToString())).ToArray();
                foreach (var template in templates)
                    await Api.Templates.AddAsync(template, code: TemplateContent.Code, text: TemplateContent.Text, publish: false, labels: new List<string> { testLabel });

                var results = await Api.Templates.ListAsync(testLabel);

                results.Count.Should().BeGreaterOrEqualTo(10);
                results.Where(info => templates.Contains(info.Name)).Should().HaveCount(10);
            }

            [Fact]
            public async Task Can_list_templates()
            {
                var testLabel = Guid.NewGuid().ToString("N");
                var templates = Enumerable.Range(1, 10).Select(i => AddToBeDeleted(Guid.NewGuid().ToString())).ToArray();
                foreach (var template in templates)
                    await Api.Templates.AddAsync(template, code: TemplateContent.Code, text: TemplateContent.Text, publish: false, labels: new List<string> { testLabel });

                var results = await Api.Templates.ListAsync(testLabel);

                results.Should().HaveCount(10);
                results.All(x =>
                {
                    x.Labels.Should().NotBeNullOrEmpty();
                    x.Labels[0].Should().Be(testLabel);
                    return true;
                }).Should().BeTrue();
            }
        }

        [Trait("Category", "templates/add.json")]
        public class Publish(MandrillFixture fixture, ITestOutputHelper output) : Templates(fixture, output)
        {
            [Fact]
            public async Task Can_publish_template()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var now = DateTime.UtcNow;
                var skew = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc).AddSeconds(-60);

                var added = await Api.Templates.AddAsync(name, code: TemplateContent.Code, text: TemplateContent.Text, publish: false);
                var result = await Api.Templates.PublishAsync(added.Name);

                result.PublishedAt.Should().BeOnOrAfter(skew);
            }
        }

        [Trait("Category", "templates/render.json")]
        public class Render(MandrillFixture fixture, ITestOutputHelper output) : Templates(fixture, output)
        {
            [Fact]
            public async Task Can_render()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                await Api.Templates.AddAsync(name, code: TemplateContent.Code, text: TemplateContent.Text, publish: false);

                var templateContent = new List<MandrillTemplateContent>
                {
                    new MandrillTemplateContent { Name = "footer", Content = "this is my footer" }
                };
                var mergeVars = new List<MandrillMergeVar>
                {
                    new MandrillMergeVar { Name = "fname", Content = "Joe" },
                    new MandrillMergeVar { Name = "ORDERDATE", Content = "11/28/2014" },
                    new MandrillMergeVar { Name = "INVOICEDETAILS", Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod" }
                };
                var result = await Api.Templates.RenderAsync(name, templateContent, mergeVars);

                result.Html.Should().NotBeNullOrWhiteSpace();
                result.Html.Should().Contain("Joe");
                result.Html.Should().Contain("11/28/2014");
                result.Html.Should().Contain("Lorem ipsum dolor sit amet");
            }
        }

        [Trait("Category", "templates/time_series.json")]
        public class TimeSeries(MandrillFixture fixture, ITestOutputHelper output) : Templates(fixture, output)
        {
            [Fact]
            public async Task Can_get_time_series()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var added = await Api.Templates.AddAsync(name, code: TemplateContent.Code, text: TemplateContent.Text, publish: false);
                var result = await Api.Templates.TimeSeriesAsync(added.Name);

                if (result.Count == 0)
                    Output.WriteLine("time-series couldn't run for a new template");
            }
        }

        [Trait("Category", "templates/update.json")]
        public class Update(MandrillFixture fixture, ITestOutputHelper output) : Templates(fixture, output)
        {
            [Fact]
            public async Task Can_update()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                await Api.Templates.AddAsync(name, code: TemplateContent.Code, text: TemplateContent.Text, publish: false);
                var result = await Api.Templates.UpdateAsync(name, code: TemplateContent.Code.Replace("footer", "booger"), text: null, publish: false,
                    fromEmail: "update@mandrilldotnet.org");
                result.Name.Should().Be(name);
                result.Code.Should().Contain("booger");
                result.Slug.Should().Be(name);
                result.FromEmail.Should().Be("update@mandrilldotnet.org");
            }
        }
    }
}
