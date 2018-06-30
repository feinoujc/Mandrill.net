using System.Linq;
using FluentAssertions;
using Mandrill.Model;
using Xunit;

namespace Tests
{
    [Trait("Category", "model")]
    public class ModelTest
    {
        public class MandrillMessageTests : ModelTest
        {
            [Fact]
            public void Message_ctor_with_html_content()
            {
                var model = new MandrillMessage("from@example.com", "to@example.com", "test subject", "<body>test</body>");

                model.FromEmail.Should().Be("from@example.com");
                model.FromName.Should().BeNull();
                model.To[0].Email.Should().Be("to@example.com");
                model.To[0].Name.Should().BeNull();
                model.Text.Should().BeNull();
                model.Html.Should().Be("<body>test</body>");
            }

            [Fact]
            public void Message_ctor_with_text_content()
            {
                var model = new MandrillMessage("from@example.com", "to@example.com", "test subject", "test");

                model.FromEmail.Should().Be("from@example.com");
                model.FromName.Should().BeNull();
                model.To[0].Email.Should().Be("to@example.com");
                model.To[0].Name.Should().BeNull();
                model.Html.Should().BeNull();
                model.Text.Should().Be("test");
            }

            [Fact]
            public void Message_ctor_to_from_strings()
            {
                var model = new MandrillMessage("from@example.com", "to@example.com");

                model.FromEmail.Should().Be("from@example.com");
                model.FromName.Should().BeNull();
                model.To[0].Email.Should().Be("to@example.com");
                model.To[0].Name.Should().BeNull();
            }

            [Fact]
            public void Message_ctor_to_from_mail_class()
            {
                var model = new MandrillMessage(new MandrillMailAddress("from@example.com", "From Name"),
                    new MandrillMailAddress("to@example.com", "To Name"));

                model.FromEmail.Should().Be("from@example.com");
                model.FromName.Should().Be("From Name");
                model.To[0].Email.Should().Be("to@example.com");
                model.To[0].Name.Should().Be("To Name");
            }

            [Fact]
            public void Add_recipient_metadata_adds_by_recipient()
            {
                var model = new MandrillMessage();

                model.To.Add(new MandrillMailAddress("to1@example.com", "To #1"));
                model.To.Add(new MandrillMailAddress("to2@example.com", "To #2") { Type = MandrillMailAddressType.Cc });

                model.AddRecipientMetadata("to1@example.com", "my-property", "1");
                model.AddRecipientMetadata("to2@example.com", "my-property", "2");

                model.RecipientMetadata.Single(m => m.Rcpt == "to1@example.com").Values["my-property"].Should().Be("1");
                model.RecipientMetadata.Single(m => m.Rcpt == "to2@example.com").Values["my-property"].Should().Be("2");
            }

            [Fact]
            public void Add_recipient_merge_vars_adds_by_recipient()
            {
                var model = new MandrillMessage();

                model.AddTo("to1@example.com", "To #1");
                model.AddTo("to2@example.com", "To #2", MandrillMailAddressType.Cc);
                model.AddTo("to3@example.com");


                model.AddRcptMergeVars("to1@example.com", "my-property", new { field = 1 });
                model.AddRcptMergeVars("to2@example.com", "my-property", new { field = 2 });

                Assert.Equal(1, model.MergeVars.Single(m => m.Rcpt == "to1@example.com").Vars.Single(v => v.Name == "my-property").Content.field);
                Assert.Equal(2, model.MergeVars.Single(m => m.Rcpt == "to2@example.com").Vars.Single(v => v.Name == "my-property").Content.field);
                Assert.DoesNotContain(model.MergeVars, m => m.Rcpt == "to3@example.com");
            }

            [Fact]
            public void Add_metadata()
            {
                var model = new MandrillMessage();

                model.AddMetadata("meta1", "foo");

                model.Metadata["META1"].Should().Be("foo");
            }

            [Fact]
            public void Add_header()
            {
                var model = new MandrillMessage();

                model.AddHeader("X-MY-HEADER", "foo");

                model.Headers["x-my-header"].Should().Be("foo");
            }

            [Fact]
            public void Add_header_list()
            {
                var model = new MandrillMessage();


                model.AddHeader("X-MY-HEADER", new[] { "foo", "bar" });

                model.Headers["x-my-header"].Should().BeEquivalentTo(new[] { "foo", "bar" });
            }

            [Fact]
            public void Reply_to_sets_header()
            {
                var model = new MandrillMessage();

                model.ReplyTo.Should().BeNull();
                model.ReplyTo = "reply@example.com";
                model.ReplyTo.Should().Be("reply@example.com");
                model.Headers["reply-to"].Should().Be("reply@example.com");
            }
        }
    }
}
