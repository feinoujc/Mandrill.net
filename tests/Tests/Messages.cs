using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Mandrill.Model;
using NUnit.Framework;

namespace Tests
{
    [Category("messages")]
    internal class Messages : IntegrationTest
    {
        [Category("messages/cancel_scheduled.json")]
        internal class CancelScheduled : Messages
        {
            [Test]
            public async void Can_cancel_scheduled()
            {
                var list = await Api.Messages.ListScheduledAsync();

                var schedule = list.LastOrDefault();
                if (schedule != null)
                {
                    var result = await Api.Messages.CancelScheduledAsync(schedule.Id);
                    result.Id.Should().Be(schedule.Id);
                }
                else
                {
                    Assert.Inconclusive("no scheduled results");
                }
            }
        }

        [Category("messages/content.json")]
        internal class Content : Messages
        {
            [Test]
            public async void Can_retrieve_content()
            {
                var results = await Api.Messages.SearchAsync(null, DateTime.Today.AddDays(-1));

                //the api doesn't return results immediately, it may return no results. 
                //Also, the content may not be around > 24 hrs
                var found = results.Where(x => x.Ts > DateTime.UtcNow.AddHours(-24)).OrderBy(x => x.Ts).FirstOrDefault();
                if (found != null)
                {
                    var result = await Api.Messages.ContentAsync(found.Id);

                    result.Should().NotBeNull();
                    var content = result.Html ?? result.Text;
                    content.Should().NotBeNullOrWhiteSpace();
                }
                else
                {
                    Assert.Inconclusive("no results were found yet, try again in a few minutes");
                }
            }

            [Test]
            public void Throws_when_not_found()
            {
                var mandrillException = Assert.Throws<MandrillException>(async () => await Api.Messages.ContentAsync(Guid.NewGuid().ToString("N")));
                mandrillException.Name.Should().Be("Unknown_Message");
            }
        }

        [Category("messages/info.json")]
        internal class Info : Messages
        {
            [Test]
            public async void Can_retrieve_info()
            {
                var results = await Api.Messages.SearchAsync("email:example.com");

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.Ts).FirstOrDefault();
                if (found != null)
                {
                    var result = await Api.Messages.InfoAync(found.Id);

                    result.Should().NotBeNull();
                    result.Id.Should().Be(found.Id);
                }
                else
                {
                    Assert.Inconclusive("no results were found yet, try again in a few minutes");
                }
            }

            [Test]
            public void Throws_when_not_found()
            {
                var mandrillException = Assert.Throws<MandrillException>(async () => await Api.Messages.InfoAync(Guid.NewGuid().ToString("N")));
                mandrillException.Name.Should().Be("Unknown_Message");
            }
        }

        [Category("messages/list_scheduled.json")]
        internal class ListScheduled : Messages
        {
            [Test]
            public async void Can_list_scheduled()
            {
                var result = await Api.Messages.ListScheduledAsync();
                result.Should().NotBeNull();
                result.Count.Should().BeGreaterOrEqualTo(0);
            }
        }

        [Category("messages/parse.json")]
        internal class Parse : Messages
        {
            [Test]
            public async void Can_parse_raw_message()
            {
                var rawMessage = "From: sender@example.com\nTo: recipient.email@example.com\nSubject: Some Subject\n\nSome content.";
                var result = await Api.Messages.ParseAsync(rawMessage);
                result.Should().NotBeNull();
                result.FromEmail.Should().Be("sender@example.com");
                result.To[0].Email.Should().Be("recipient.email@example.com");
                result.Subject.Should().Be("Some Subject");
                result.Text.Should().Be("Some content.");
            }
        }

        [Category("messages/reschedule.json")]
        internal class Reschedule : Messages
        {
            [Test]
            public async void Can_reschedule()
            {
                var list = await Api.Messages.ListScheduledAsync();

                var schedule = list.LastOrDefault();
                if (schedule != null)
                {
                    var sendAtUtc = DateTime.UtcNow.AddHours(1);
                    sendAtUtc = new DateTime(sendAtUtc.Year, sendAtUtc.Month, sendAtUtc.Day, sendAtUtc.Hour, sendAtUtc.Minute, sendAtUtc.Second, 0, DateTimeKind.Utc);
                    var result = await Api.Messages.RescheduleAsync(schedule.Id, sendAtUtc);
                    result.SendAt.Should().Be(sendAtUtc);
                }
                else
                {
                    Assert.Inconclusive("no scheduled messages found.");
                }
            }
        }

        [Category("messages/search.json")]
        internal class Search : Messages
        {
            [Test]
            public async void Can_search_all_params()
            {
                var results = await Api.Messages.SearchAsync("email:example.com",
                    DateTime.Today.AddDays(-1),
                    DateTime.Today.AddDays(1),
                    new[] {"mandrill-net"},
                    new[] {"mandrill.net@example.com"},
                    new[] {ApiKey},
                    10);

                //the api doesn't return results immediately, it may return no results
                results.Count.Should().BeLessOrEqualTo(10);

                foreach (var result in results)
                {
                    result.Id.Should().NotBeEmpty();
                }

                if (results.Count == 0)
                {
                    Assert.Inconclusive("no results were found yet, try again in a few minutes");
                }
            }

            [Test]
            public async void Can_search_query()
            {
                var results = await Api.Messages.SearchAsync("email:example.com", limit: 1);

                //the api doesn't return results immediately, it may return no results
                results.Count.Should().BeLessOrEqualTo(1);

                foreach (var result in results)
                {
                    result.Id.Should().NotBeEmpty();
                }
                if (results.Count == 0)
                {
                    Assert.Inconclusive("no results were found yet, try again in a few minutes");
                }
            }
        }

        [Category("messages/search_time_series.json")]
        internal class SearchTimeSeries : Messages
        {
            [Test]
            public async void Can_search_all_params()
            {
                var results = await Api.Messages.SearchTimeSeriesAsync("email:example.com",
                    DateTime.Today.AddDays(-1),
                    DateTime.Today.AddDays(1),
                    new[] {"mandrill-net"},
                    new[] {"mandrill.net@example.com"});

                foreach (var result in results)
                {
                    result.Clicks.Should().BeGreaterOrEqualTo(0);
                }

                if (results.Count == 0)
                {
                    Assert.Inconclusive("no results were found yet, try again in a few minutes");
                }
            }

            [Test]
            public async void Can_search_open_query()
            {
                var results = await Api.Messages.SearchTimeSeriesAsync(null);

                foreach (var result in results)
                {
                    result.Clicks.Should().BeGreaterOrEqualTo(0);
                }

                if (results.Count == 0)
                {
                    Assert.Inconclusive("no results were found yet, try again in a few minutes");
                }
            }
        }

        [Category("messages/send.json")]
        internal class Send : Messages
        {
            [Test]
            public async void Can_send_message()
            {
                var message = new MandrillMessage
                {
                    FromEmail = "mandrill.net@example.com",
                    Subject = "test",
                    Tags = new List<string>() {"test-send", "mandrill-net"},
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com"),
                        new MandrillMailAddress("test2@example.com", "A test")
                    },
                    Text = "This is a test",
                    Html = @"<html>
<head>
	<title>a test</title>
</head>
<body>
<p>this is a test</p>
</body>
</html>"
                };
                var result = await Api.Messages.SendAsync(message);

                result.Should().HaveCount(2);
                result[0].Status.Should().Be(MandrillSendMessageResponseStatus.Sent);
                result[0].Id.Should().NotBeEmpty();
                result[1].Id.Should().NotBeEmpty();
                result[1].Status.Should().Be(MandrillSendMessageResponseStatus.Sent);
            }

            [Test]
            public void Can_throw_errors_when_error_response()
            {
                var invalidSubaccount = Guid.NewGuid().ToString("N");
                var message = new MandrillMessage
                {
                    FromEmail = "mandrill.net@example.com",
                    Subject = "test",
                    Tags = new List<string>() {"test-send-invalid"},
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com")
                    },
                    Text = "This is a test",
                    Subaccount = invalidSubaccount
                };

                var result = Assert.Throws<MandrillException>(async () => await Api.Messages.SendAsync(message));
                result.Should().NotBeNull();
                result.Name.Should().Be("Unknown_Subaccount");
                result.Message.Should().Contain(invalidSubaccount);
            }

            [Test]
            public async void Can_send_async()
            {
                var message = new MandrillMessage
                {
                    FromEmail = "mandrill.net@example.com",
                    Subject = "test",
                    Tags = new List<string> {"test-send", "mandrill-net"},
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com")
                    },
                    Text = "This is a test",
                };
                var result = await Api.Messages.SendAsync(message, true);

                result.Should().HaveCount(1);
                result[0].Email.Should().Be("test1@example.com");
                result[0].Status.Should().NotBe(MandrillSendMessageResponseStatus.Rejected);
                result[0].Status.Should().NotBe(MandrillSendMessageResponseStatus.Invalid);
            }

            [Test]
            [Ignore("Requires account with $")]
            public async void Can_send_scheduled()
            {
                var message = new MandrillMessage
                {
                    FromEmail = "mandrill.net@example.com",
                    Subject = "test",
                    Tags = new List<string>() {"test-send", "mandrill-net"},
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com")
                    },
                    Text = "This is a test",
                };

                var sendAtUtc = DateTime.UtcNow.AddHours(1);
                var result = await Api.Messages.SendAsync(message, sendAtUtc: sendAtUtc);

                result.Should().HaveCount(1);
                result[0].Email.Should().Be("test1@example.com");
                result[0].Status.Should().Be(MandrillSendMessageResponseStatus.Scheduled);
            }

            [Test]
            public void Throws_if_scheduled_is_not_utc()
            {
                var message = new MandrillMessage();

                var sendAtLocal = DateTime.SpecifyKind(DateTime.Now.AddHours(1), DateTimeKind.Local);
                var result = Assert.Throws<ArgumentException>(async () => await Api.Messages.SendAsync(message, sendAtUtc: sendAtLocal));

                result.ParamName.Should().Be("sendAtUtc");
            }
        }

        [Category("messages/send_raw.json")]
        internal class SendRaw : Messages
        {
            [Test]
            public async void Can_send_raw_message()
            {
                var rawMessage = "From: sender@example.com\nTo: recipient.email@example.com\nSubject: Some Subject\n\nSome content.";
                var fromEmail = "sender@example.com";
                var fromName = "From Name";
                var to = new[] {"recipient.email@example.com"};
                bool async = false;
                string ipPool = "Main Pool";
                DateTime? sendAt = null;
                string returnPathDomain = null;


                var result = await Api.Messages.SendRawAsync(rawMessage, fromEmail, fromName, to, async, ipPool, sendAt, returnPathDomain);

                result.Should().HaveCount(1);
                result[0].Status.Should().Be(MandrillSendMessageResponseStatus.Sent);
                result[0].Email.Should().Be("recipient.email@example.com");
                result[0].Id.Should().NotBeEmpty();
            }
        }

        [Category("messages/send_template.json")]
        internal class SendTemplate : Messages
        {
            protected string TestTemplateName;

            [TestFixtureSetUp]
            public override void SetUp()
            {
                base.SetUp();
                TestTemplateName = Guid.NewGuid().ToString();
                var result = Api.Templates.AddAsync(TestTemplateName, TemplateContent.Code, TemplateContent.Text, true).Result;
                result.Should().NotBeNull();
            }

            [TestFixtureTearDown]
            public override void TearDown()
            {
                var result = Api.Templates.DeleteAsync(TestTemplateName).Result;
                result.Should().NotBeNull();
                base.TearDown();
            }

            [Test]
            public async void Can_send_template()
            {
                var message = new MandrillMessage
                {
                    FromEmail = "mandrill.net@example.com",
                    Subject = "test",
                    Tags = new List<string>() {"test-send-template", "mandrill-net"},
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com", "Test1 User"),
                        new MandrillMailAddress("test2@example.com", "Test2 User")
                    },
                };

                message.AddGlobalMergeVars("ORDERDATE", DateTime.UtcNow.ToShortDateString());
                message.AddRcptMergeVars("test1@example.com", "INVOICEDETAILS", "invoice for test1@example.com");
                message.AddRcptMergeVars("test2@example.com", "INVOICEDETAILS", "invoice for test2@example.com");

                var result = await Api.Messages.SendTemplateAsync(message, TestTemplateName);

                result.Should().HaveCount(2);
                result[0].Status.Should().Be(MandrillSendMessageResponseStatus.Sent);
                result[0].Id.Should().NotBeEmpty();
                result[1].Id.Should().NotBeEmpty();
                result[1].Status.Should().Be(MandrillSendMessageResponseStatus.Sent);
            }
        }

        [Category("messages/send_template.json"), Category("handlebars")]
        internal class SendTemplate_Handlebars : Messages
        {
            protected string TestTemplateName;

            [TestFixtureSetUp]
            public override void SetUp()
            {
                base.SetUp();
                TestTemplateName = Guid.NewGuid().ToString();
                var result = Api.Templates.AddAsync(TestTemplateName, TemplateContent.HandleBarCode, null, true).Result;
                result.Should().NotBeNull();
            }

            [TestFixtureTearDown]
            public override void TearDown()
            {
                var result = Api.Templates.DeleteAsync(TestTemplateName).Result;
                result.Should().NotBeNull();
                base.TearDown();
            }

            [Test]
            public async void Can_send_template()
            {
                var message = new MandrillMessage
                {
                    FromEmail = "mandrill.net@example.com",
                    Subject = "test",
                    Tags = new List<string>() { "test-send-template", "mandrill-net" },
                    MergeLanguage = MandrillMessageMergeLanguage.Handlebars,
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com", "Test1 User"),
                        new MandrillMailAddress("test2@example.com", "Test2 User")
                    },
                };


                var data1 = new[]
                {
                    new Dictionary<string, string>
                    {
                        {"sku", "APL43"},
                        {"name", "apples"},
                        {"description", "Granny Smith Apples"},
                        {"price", "$0.20"},
                        {"qty", "8"},
                        {"ordPrice", "$1.60"},

                    },
                    new Dictionary<string, string>
                    {
                        {"sku", "ORA44"},
                        {"name", "Oranges"},
                        {"description", "Blood Oranges"},
                        {"price", "$0.30"},
                        {"qty", "3"},
                        {"ordPrice", "$0.93"},

                    }
                };

                var data2 = new[]
                {
                    new Dictionary<string, string>
                    {
                        {"sku", "APL54"},
                        {"name", "apples"},
                        {"description", "Red Delicious Apples"},
                        {"price", "$0.22"},
                        {"qty", "9"},
                        {"ordPrice", "$1.98"},

                    },
                    new Dictionary<string, string>
                    {
                        {"sku", "ORA53"},
                        {"name", "Oranges"},
                        {"description", "Sunkist Oranges"},
                        {"price", "$0.20"},
                        {"qty", "1"},
                        {"ordPrice", "$0.20"},

                    }
                };

                message.AddGlobalMergeVars("ORDERDATE", DateTime.UtcNow.ToShortDateString());
                message.AddRcptMergeVars("test1@example.com", "PRODUCTS", data1);
                message.AddRcptMergeVars("test2@example.com", "PRODUCTS",  data2);

                var result = await Api.Messages.SendTemplateAsync(message, TestTemplateName);

                result.Should().HaveCount(2);
                result[0].Status.Should().Be(MandrillSendMessageResponseStatus.Sent);
                result[0].Id.Should().NotBeEmpty();
                result[1].Id.Should().NotBeEmpty();
                result[1].Status.Should().Be(MandrillSendMessageResponseStatus.Sent);
            }
        }
    }
}