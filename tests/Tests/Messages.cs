﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill.Model;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tests
{
    [Category("messages")]
    internal class Messages : IntegrationTest
    {
        [SetUp]
        public virtual void Setup()
        {
            FromEmail = "mandrill.net@" +
                        (Environment.GetEnvironmentVariable("MANDRILL_SENDING_DOMAIN") ?? "test.mandrillapp.com");
        }

        public string FromEmail { get; set; }

        static void AssertResults(IEnumerable<MandrillSendMessageResponse> result)
        {
            foreach (var response in result)
            {
                if (response.Status == MandrillSendMessageResponseStatus.Invalid)
                {
                    Assert.Fail("invalid email: " + response.RejectReason);
                }
                if (response.Status == MandrillSendMessageResponseStatus.Rejected &&
          response.RejectReason == "unsigned")
                {
                    Assert.Inconclusive("unsigned sending domain");
                }
                if (response.Status == MandrillSendMessageResponseStatus.Rejected)
                {
                    Assert.Fail("rejected email: " + response.RejectReason);

                }

                if (response.Status == MandrillSendMessageResponseStatus.Queued ||
                    response.Status == MandrillSendMessageResponseStatus.Sent)
                {
                    Assert.Pass();
                }

                Assert.Fail("Unexptected status:" + response.Status);
            }
        }

        [Category("messages/cancel_scheduled.json")]
        internal class CancelScheduled : Messages
        {
            [Test]
            public async Task Can_cancel_scheduled()
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
            public async Task Can_retrieve_content()
            {
                var results = await Api.Messages.SearchAsync(null, DateTime.Today.AddDays(-1));

                //the api doesn't return results immediately, it may return no results. 
                //Also, the content may not be around > 24 hrs
                var found = results.Where(x => x.Ts > DateTime.UtcNow.AddHours(-24))
                        .OrderBy(x => x.Ts)
                        .FirstOrDefault(x => x.State == MandrillMessageState.Sent);
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
            public async Task Throws_when_not_found()
            {
                var mandrillException = await ThrowsAsync<MandrillException>(() => Api.Messages.ContentAsync(Guid.NewGuid().ToString("N")));
                mandrillException.Name.Should().Be("Unknown_Message");
            }

            [Test]
            public async Task Throws_when_not_found_sync()
            {
                var mandrillException = await ThrowsAsync<MandrillException>(() => Api.Messages.ContentAsync(Guid.NewGuid().ToString("N")));
                mandrillException.Name.Should().Be("Unknown_Message");
                Debug.WriteLine(mandrillException);
            }
        }

        [Category("messages/info.json")]
        internal class Info : Messages
        {
            [Test]
            public async Task Can_retrieve_info()
            {
                var results = await Api.Messages.SearchAsync("email:example.com");

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.Ts).FirstOrDefault();
                if (found != null)
                {
                    var result = await Api.Messages.InfoAsync(found.Id);

                    result.Should().NotBeNull();
                    result.Id.Should().Be(found.Id);
                }
                else
                {
                    Assert.Inconclusive("no results were found yet, try again in a few minutes");
                }
            }

            [Test]
            public async Task Throws_when_not_found()
            {
                var mandrillException = await ThrowsAsync<MandrillException>(() => Api.Messages.InfoAsync(Guid.NewGuid().ToString("N")));
                mandrillException.Name.Should().Be("Unknown_Message");
            }
        }

        [Category("messages/list_scheduled.json")]
        internal class ListScheduled : Messages
        {
            [Test]
            public async Task Can_list_scheduled()
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
            public async Task Can_parse_raw_message()
            {
                var rawMessage = $"From: {FromEmail}\nTo: recipient.email@example.com\nSubject: Some Subject\n\nSome content.";
                var result = await Api.Messages.ParseAsync(rawMessage);
                result.Should().NotBeNull();
                result.FromEmail.Should().Be(FromEmail);
                result.To[0].Email.Should().Be("recipient.email@example.com");
                result.Subject.Should().Be("Some Subject");
                result.Text.Should().Be("Some content.");
            }


            [Test]
            public async Task Can_parse_full_raw_message_headers()
            {
                var rawMessage = @"Delivered-To: MrSmith@gmail.com
Received: by 10.36.81.3 with SMTP id e3cs239nzb; Tue, 29 Mar 2005 15:11:47 -0800 (PST)
Return-Path: 
Received: from mail.emailprovider.com (mail.emailprovider.com [111.111.11.111]) by mx.gmail.com with SMTP id h19si826631rnb.2005.03.29.15.11.46; Tue, 29 Mar 2005 15:11:47 -0800 (PST)
Message-ID: <20050329231145.62086.mail@mail.emailprovider.com>
Reply-To: MrsJohnson@gmail.com
Received: from [11.11.111.111] by mail.emailprovider.com via HTTP; Tue, 29 Mar 2005 15:11:45 PST
Date: Tue, 29 Mar 2005 15:11:45 -0800 (PST)
From: Mr Jones 
Subject: Hello
To: Mr Smith
";
                var result = await Api.Messages.ParseAsync(rawMessage);

                result.Should().NotBeNull();
                result.Headers["Received"].Should().BeOfType<JArray>();
                ((JArray) result.Headers["Received"]).Count.Should().Be(3);
                result.Headers["Delivered-To"].Should().BeOfType<string>();
                result.Headers["Delivered-To"].Should().Be("MrSmith@gmail.com");
                result.ReplyTo.Should().Be("MrsJohnson@gmail.com");
            }
        }

        [Category("messages/reschedule.json")]
        internal class Reschedule : Messages
        {
            [Test]
            public async Task Can_reschedule()
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
            public async Task Can_search_all_params()
            {
                var results = await Api.Messages.SearchAsync("email:example.com",
                    DateTime.Today.AddDays(-1),
                    DateTime.Today.AddDays(1),
                    new string[0],
                    new[] {FromEmail},
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
            public async Task Can_search_query()
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
            public async Task Can_search_all_params()
            {
                var results = await Api.Messages.SearchTimeSeriesAsync("email:example.com",
                    DateTime.Today.AddDays(-1),
                    DateTime.Today.AddDays(1),
                    new string[0],
                    new[] {FromEmail});

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
            public async Task Can_search_open_query()
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
            public async Task Can_send_message()
            {
                var message = new MandrillMessage
                {
                    FromEmail = FromEmail,
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
                AssertResults(result);
            }

            [Test]
            public async Task Can_throw_errors_when_error_response()
            {
                var invalidSubaccount = Guid.NewGuid().ToString("N");
                var message = new MandrillMessage
                {
                    FromEmail = FromEmail,
                    Subject = "test",
                    Tags = new List<string>() {"test-send-invalid"},
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com")
                    },
                    Text = "This is a test",
                    Subaccount = invalidSubaccount
                };

                var result = await ThrowsAsync<MandrillException>(() => Api.Messages.SendAsync(message));
                result.Should().NotBeNull();
                result.Name.Should().Be("Unknown_Subaccount");
                result.Message.Should().Contain(invalidSubaccount);
            }

            [Test]
            public async Task Can_send_async()
            {
                var message = new MandrillMessage
                {
                    FromEmail = FromEmail,
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
                AssertResults(result);

            }

#if NETFX
            [Test]
            public void Can_send_sync()
            {
                var message = new MandrillMessage
                {
                    FromEmail = FromEmail,
                    Subject = "test",
                    Tags = new List<string> { "test-send", "mandrill-net" },
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com")
                    },
                    Text = "This is a test",
                };
                var result = Api.Messages.Send(message, true);

                result.Should().HaveCount(1);
                AssertResults(result);

            }
#endif
            [Test]
            public async Task Can_send_scheduled()
            {
                var message = new MandrillMessage
                {
                    FromEmail = FromEmail,
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
            public async Task Throws_if_scheduled_is_not_utc()
            {
                var message = new MandrillMessage();

                var sendAtLocal = DateTime.SpecifyKind(DateTime.Now.AddHours(1), DateTimeKind.Local);
                var result = await ThrowsAsync<ArgumentException>(() => Api.Messages.SendAsync(message, sendAtUtc: sendAtLocal));

                result.ParamName.Should().Be("sendAtUtc");
            }
        }

        [Category("messages/send_raw.json")]
        internal class SendRaw : Messages
        {
            [Test]
            public async Task Can_send_raw_message()
            {
                var rawMessage = $"From: {FromEmail}\nTo: recipient.email@example.com\nSubject: Some Subject\n\nSome content.";
                var fromEmail = FromEmail;
                var fromName = "From Name";
                var to = new[] {"recipient.email@example.com"};
                bool async = false;
                string ipPool = "Main Pool";
                DateTime? sendAt = null;
                string returnPathDomain = null;


                var result = await Api.Messages.SendRawAsync(rawMessage, fromEmail, fromName, to, async, ipPool, sendAt, returnPathDomain);

                result.Should().HaveCount(1);
                AssertResults(result);

            }
        }

        [Category("messages/send_template.json")]
        internal class SendTemplate : Messages
        {
            protected string TestTemplateName;

            [SetUp]
            public override void Setup()
            {
                TestTemplateName = Guid.NewGuid().ToString();
                var result = Api.Templates.AddAsync(TestTemplateName, TemplateContent.Code, TemplateContent.Text, true).GetAwaiter().GetResult();
                result.Should().NotBeNull();
                base.Setup();
            }

            [TearDown]
            public void TearDown()
            {
                var result = Api.Templates.DeleteAsync(TestTemplateName).GetAwaiter().GetResult();
                result.Should().NotBeNull();
            }

            [Test]
            public async Task Can_send_template()
            {
                var message = new MandrillMessage
                {
                    FromEmail = FromEmail,
                    Subject = "test",
                    Tags = new List<string>() {"test-send-template", "mandrill-net"},
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com", "Test1 User"),
                        new MandrillMailAddress("test2@example.com", "Test2 User")
                    },
                };

                message.AddGlobalMergeVars("ORDERDATE", string.Format("{0:d}", DateTime.UtcNow));
                message.AddRcptMergeVars("test1@example.com", "INVOICEDETAILS", "invoice for test1@example.com");
                message.AddRcptMergeVars("test2@example.com", "INVOICEDETAILS", "invoice for test2@example.com");

                var result = await Api.Messages.SendTemplateAsync(message, TestTemplateName);

                result.Should().HaveCount(2);
                AssertResults(result);

            }
        }

        [Category("messages/send_template.json"), Category("handlebars")]
        internal class SendTemplate_Handlebars : Messages
        {
            protected string TestTemplateName;

            [SetUp]
            public override void Setup()
            {
                TestTemplateName = Guid.NewGuid().ToString();
                var result = Api.Templates.AddAsync(TestTemplateName, TemplateContent.HandleBarCode, null, true).GetAwaiter().GetResult();
                result.Should().NotBeNull();
                base.Setup();
            }

            [TearDown]
            public void TearDown()
            {
                var result = Api.Templates.DeleteAsync(TestTemplateName).GetAwaiter().GetResult();
                result.Should().NotBeNull();
            }

            [Test]
            public async Task Can_send_template_string_dictionary()
            {
                var message = new MandrillMessage
                {
                    FromEmail = FromEmail,
                    Subject = "test",
                    Tags = new List<string>() { "test-send-template", "mandrill-net", "handlebars" },
                    MergeLanguage = MandrillMessageMergeLanguage.Handlebars,
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com", "Test1 User"),
                        new MandrillMailAddress("test2@example.com", "Test2 User")
                    },
                };


                var data1 = new[]
                {
                    new Dictionary<string, object>
                    {
                        {"sku", "APL43"},
                        {"name", "apples"},
                        {"description", "Granny Smith Apples"},
                        {"price", "0.20"},
                        {"qty", "8"},
                        {"ordPrice", "1.60"},

                    },
                    new Dictionary<string, object>
                    {
                        {"sku", "ORA44"},
                        {"name", "Oranges"},
                        {"description", "Blood Oranges"},
                        {"price", "0.30"},
                        {"qty", "3"},
                        {"ordPrice", "0.93"},

                    }
                };

                var data2 = new[]
                {
                    new Dictionary<string, object>
                    {
                        {"sku", "APL54"},
                        {"name", "apples"},
                        {"description", "Red Delicious Apples"},
                        {"price", "0.22"},
                        {"qty", "9"},
                        {"ordPrice", "1.98"},

                    },
                    new Dictionary<string, object>
                    {
                        {"sku", "ORA53"},
                        {"name", "Oranges"},
                        {"description", "Sunkist Oranges"},
                        {"price", "0.20"},
                        {"qty", "1"},
                        {"ordPrice", "0.20"},

                    }
                };

                message.AddGlobalMergeVars("ORDERDATE", DateTime.UtcNow.ToString("d"));
                message.AddRcptMergeVars("test1@example.com", "PRODUCTS", data1);
                message.AddRcptMergeVars("test2@example.com", "PRODUCTS", data2);

                var result = await Api.Messages.SendTemplateAsync(message, TestTemplateName);

                result.Should().HaveCount(2);
                AssertResults(result);
            }

            [Test]
            public async Task Can_send_template_object_list()
            {
                var message = new MandrillMessage
                {
                    FromEmail = FromEmail,
                    Subject = "test",
                    Tags = new List<string>() { "test-send-template", "mandrill-net", "handlebars" },
                    MergeLanguage = MandrillMessageMergeLanguage.Handlebars,
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com", "Test1 User"),
                        new MandrillMailAddress("test2@example.com", "Test2 User")
                    },
                };


                var data1 = new[]
                {
                    new Dictionary<string, object>
                    {
                        {"sku", "APL43"},
                        {"name", "apples"},
                        {"description", "Granny Smith Apples"},
                        {"price", 0.20},
                        {"qty", 8},
                        {"ordPrice", 1.60},

                    },
                    new Dictionary<string, object>
                    {
                        {"sku", "ORA44"},
                        {"name", "Oranges"},
                        {"description", "Blood Oranges"},
                        {"price", 0.30},
                        {"qty", 3},
                        {"ordPrice", 0.93},

                    }
                };

                var data2 = new[]
                {
                    new Dictionary<string, object>
                    {
                        {"sku", "APL54"},
                        {"name", "apples"},
                        {"description", "Red Delicious Apples"},
                        {"price", 0.22},
                        {"qty", 9},
                        {"ordPrice", 1.98},

                    },
                    new Dictionary<string, object>
                    {
                        {"sku", "ORA53"},
                        {"name", "Oranges"},
                        {"description", "Sunkist Oranges"},
                        {"price", 0.20},
                        {"qty", 1},
                        {"ordPrice", 0.20},

                    }
                };

                message.AddGlobalMergeVars("ORDERDATE", DateTime.UtcNow.ToString("d"));
                message.AddRcptMergeVars("test1@example.com", "PRODUCTS", data1);
                message.AddRcptMergeVars("test2@example.com", "PRODUCTS",  data2);

                var result = await Api.Messages.SendTemplateAsync(message, TestTemplateName);

                result.Should().HaveCount(2);
                AssertResults(result);

            }


            [Test]
            public async Task Can_send_template_dynamic()
            {
                var message = new MandrillMessage
                {
                    FromEmail = FromEmail,
                    Subject = "test",
                    Tags = new List<string>() { "test-send-template", "mandrill-net", "handlebars" },
                    MergeLanguage = MandrillMessageMergeLanguage.Handlebars,
                    To = new List<MandrillMailAddress>()
                    {
                        new MandrillMailAddress("test1@example.com", "Test1 User"),
                        new MandrillMailAddress("test2@example.com", "Test2 User")
                    },
                };

                dynamic item1 = new ExpandoObject();
                item1.sku = "APL54";
                item1.name = "apples";
                item1.description = "Red Dynamic Apples";
                item1.price = 0.22;
                item1.qty = 9;
                item1.ordPrice = 1.98;
                item1.tags = new {id = "tag1", enabled = true};


                dynamic item2 = new ExpandoObject();
                item2.sku = "ORA54";
                item2.name = "oranges";
                item2.description = "Dynamic Oranges";
                item2.price = 0.33;
                item2.qty = 8;
                item2.ordPrice = 2.00;
                item2.tags = new { id = "tag2", enabled = false };


                message.AddGlobalMergeVars("ORDERDATE", DateTime.UtcNow.ToString("d"));
                message.AddGlobalMergeVars("PRODUCTS", new [] { item1, item2 });

                var result = await Api.Messages.SendTemplateAsync(message, TestTemplateName);

                result.Should().HaveCount(2);
                AssertResults(result);
            }
        }
    }
}
