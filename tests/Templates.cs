using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill.Model;
using Xunit;

namespace Tests
{
    [Trait("Category", "templates")]
    [Collection("templates")]
    public class Templates : IntegrationTest
    {
        protected HashSet<string> TemplatesToCleanup;

        protected string AddToBeDeleted(string templateName)
        {
            TemplatesToCleanup.Add(templateName);
            return templateName;
        }

        public Templates()
        {
            TemplatesToCleanup = new HashSet<string>();
        }

        public override void Dispose()
        {
            foreach (var templateName in TemplatesToCleanup)
            {
                var result = Api.Templates.DeleteAsync(templateName).GetAwaiter().GetResult();
            }
            TemplatesToCleanup = null;
            base.Dispose();
        }

        [Trait("Category", "templates/add.json")]
        public class Add : Templates
        {
            [Fact]
            public async Task Can_add_template()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var result = await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);

                result.Name.Should().Be(name);
                result.Code.Should().Be(TemplateContent.Code);
                result.Slug.Should().Be(name);
                result.Text.Should().Be(TemplateContent.Text);
            }
        }

        [Trait("Category", "templates/add.json")]
        public class Info : Templates
        {
            [Fact]
            public async Task Can_get_template_info()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var added = await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);
                var result = await Api.Templates.InfoAsync(added.Name);

                result.Name.Should().Be(name);
                result.Code.Should().Be(TemplateContent.Code);
                result.Slug.Should().Be(name);
                result.Text.Should().Be(TemplateContent.Text);
            }
        }

        [Trait("Category", "templates/list.json")]
        public class List : Templates
        {
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

                results.Count.Should().BeGreaterOrEqualTo(10);
                results.Where(info => templates.Contains(info.Name)).Should().HaveCount(10);
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
        public class Publish : Templates
        {
            [Fact]
            public async Task Can_publish_template()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var now = DateTime.UtcNow;
                var skew = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc).AddSeconds(-60);

                var added = await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);
                var result = await Api.Templates.PublishAsync(added.Name);

                result.PublishedAt.Should().BeOnOrAfter(skew);
            }
        }

        [Trait("Category", "templates/render.json")]
        public class Render : Templates
        {
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

                result.Html.Should().NotBeNullOrWhiteSpace();
                result.Html.Should().Contain("Joe");
                result.Html.Should().Contain("11/28/2014");
                result.Html.Should().Contain("Lorem ipsum dolor sit amet");
            }
        }

        [Trait("Category", "templates/time_series.json")]
        public class TimeSeries : Templates
        {
            [Fact]
            public async Task Can_get_time_series()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                var added = await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);
                var result = await Api.Templates.TimeSeriesAsync(added.Name);

                if (result.Count == 0)
                {
                    Console.Error.WriteLine("time-series couldn't run for a new template");
                }
            }
        }

        [Trait("Category", "templates/update.json")]
        public class Update : Templates
        {
            [Fact]
            public async Task Can_update()
            {
                var name = AddToBeDeleted(Guid.NewGuid().ToString());
                await Api.Templates.AddAsync(name, TemplateContent.Code, TemplateContent.Text, false);
                var result = await Api.Templates.UpdateAsync(name, TemplateContent.Code.Replace("footer", "booger"), null, false,
                    "update@example.com");
                result.Name.Should().Be(name);
                result.Code.Should().Contain("booger");
                result.Slug.Should().Be(name);
                result.FromEmail.Should().Be("update@example.com");
            }
        }
    }
}
