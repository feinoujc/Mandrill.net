using System;
using System.Linq;
using FluentAssertions;
using Mandrill.Model;
using NUnit.Framework;

namespace Tests
{
    [Category("messages")]
    internal class Messages : IntegrationTest
    {

        [Category("messages/content.json")]
        internal class Content : Messages
        {
            [Test]
            public async void Can_retrieve_content()
            {
                var results = await Api.Messages.SearchAsync("email:example.com");

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.Ts).FirstOrDefault();
                if (found != null)
                {
                    var result = await Api.Messages.Content(found.Id);

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
                var mandrillException = Assert.Throws<MandrillException>(async () => await Api.Messages.Content(Guid.NewGuid()));
                mandrillException.Message.Should().Contain("Unknown_Message");
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
                    var result = await Api.Messages.Info(found.Id);

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
                var mandrillException = Assert.Throws<MandrillException>(async () => await Api.Messages.Info(Guid.NewGuid()));
                mandrillException.Message.Should().Contain("Unknown_Message");
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
                    Tags = new[] {"test-send", "mandrill-net"},
                    To = new[]
                    {
                        new MandrillToAddress("test1@example.com"),
                        new MandrillToAddress("test2@example.com", "A test")
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
                    Tags = new[] {"test-send-invalid"},
                    To = new[]
                    {
                        new MandrillToAddress("test1@example.com")
                    },
                    Text = "This is a test",
                    Subaccount = invalidSubaccount
                };

                var result = Assert.Throws<MandrillException>(async () => await Api.Messages.SendAsync(message));
                result.Should().NotBeNull();
                result.Message.Should().Contain("Unknown_Subaccount");
                result.Message.Should().Contain(invalidSubaccount);
            }

            [Test]
            public async void Can_send_async()
            {
                var message = new MandrillMessage
                {
                    FromEmail = "mandrill.net@example.com",
                    Subject = "test",
                    Tags = new[] {"test-send", "mandrill-net"},
                    To = new[]
                    {
                        new MandrillToAddress("test1@example.com")
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
                    Tags = new[] {"test-send", "mandrill-net"},
                    To = new[]
                    {
                        new MandrillToAddress("test1@example.com")
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
                    Tags = new[] {"test-send-template", "mandrill-net"},
                    To = new[]
                    {
                        new MandrillToAddress("test1@example.com", "Test1 User"),
                        new MandrillToAddress("test2@example.com", "Test2 User")
                    },
                };
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