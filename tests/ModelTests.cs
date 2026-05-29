using System.Linq;
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
                var model = new MandrillMessage("from@mandrilldotnet.org", "to@mandrilldotnet.org", "test subject", "<body>test</body>");

                Assert.Equal("from@mandrilldotnet.org", model.FromEmail);
                Assert.Null(model.FromName);
                Assert.Equal("to@mandrilldotnet.org", model.To[0].Email);
                Assert.Null(model.To[0].Name);
                Assert.Null(model.Text);
                Assert.Equal("<body>test</body>", model.Html);
            }

            [Fact]
            public void Message_ctor_with_text_content()
            {
                var model = new MandrillMessage("from@mandrilldotnet.org", "to@mandrilldotnet.org", "test subject", "test");

                Assert.Equal("from@mandrilldotnet.org", model.FromEmail);
                Assert.Null(model.FromName);
                Assert.Equal("to@mandrilldotnet.org", model.To[0].Email);
                Assert.Null(model.To[0].Name);
                Assert.Null(model.Html);
                Assert.Equal("test", model.Text);
            }

            [Fact]
            public void Message_ctor_to_from_strings()
            {
                var model = new MandrillMessage("from@mandrilldotnet.org", "to@mandrilldotnet.org");

                Assert.Equal("from@mandrilldotnet.org", model.FromEmail);
                Assert.Null(model.FromName);
                Assert.Equal("to@mandrilldotnet.org", model.To[0].Email);
                Assert.Null(model.To[0].Name);
            }

            [Fact]
            public void Message_ctor_to_from_mail_class()
            {
                var model = new MandrillMessage(new MandrillMailAddress("from@mandrilldotnet.org", "From Name"),
                    new MandrillMailAddress("to@mandrilldotnet.org", "To Name"));

                Assert.Equal("from@mandrilldotnet.org", model.FromEmail);
                Assert.Equal("From Name", model.FromName);
                Assert.Equal("to@mandrilldotnet.org", model.To[0].Email);
                Assert.Equal("To Name", model.To[0].Name);
            }

            [Fact]
            public void Add_recipient_metadata_adds_by_recipient()
            {
                var model = new MandrillMessage();

                model.To.Add(new MandrillMailAddress("to1@mandrilldotnet.org", "To #1"));
                model.To.Add(new MandrillMailAddress("to2@mandrilldotnet.org", "To #2") { Type = MandrillMailAddressType.Cc });

                model.AddRecipientMetadata("to1@mandrilldotnet.org", "my-property", "1");
                model.AddRecipientMetadata("to2@mandrilldotnet.org", "my-property", "2");

                Assert.Equal("1", model.RecipientMetadata.Single(m => m.Rcpt == "to1@mandrilldotnet.org").Values["my-property"]);
                Assert.Equal("2", model.RecipientMetadata.Single(m => m.Rcpt == "to2@mandrilldotnet.org").Values["my-property"]);
            }

            [Fact]
            public void Add_recipient_merge_vars_adds_by_recipient()
            {
                var model = new MandrillMessage();

                model.AddTo("to1@mandrilldotnet.org", "To #1");
                model.AddTo("to2@mandrilldotnet.org", "To #2", MandrillMailAddressType.Cc);
                model.AddTo("to3@mandrilldotnet.org");


                model.AddRcptMergeVars("to1@mandrilldotnet.org", "my-property", new { field = 1 });
                model.AddRcptMergeVars("to2@mandrilldotnet.org", "my-property", new { field = 2 });

                Assert.Equal(1, model.MergeVars.Single(m => m.Rcpt == "to1@mandrilldotnet.org").Vars.Single(v => v.Name == "my-property").Content.field);
                Assert.Equal(2, model.MergeVars.Single(m => m.Rcpt == "to2@mandrilldotnet.org").Vars.Single(v => v.Name == "my-property").Content.field);
                Assert.DoesNotContain(model.MergeVars, m => m.Rcpt == "to3@mandrilldotnet.org");
            }

            [Fact]
            public void Add_metadata()
            {
                var model = new MandrillMessage();

                model.AddMetadata("meta1", "foo");

                Assert.Equal("foo", model.Metadata["META1"]);
            }

            [Fact]
            public void Add_header()
            {
                var model = new MandrillMessage();

                model.AddHeader("X-MY-HEADER", "foo");

                Assert.Equal("foo", model.Headers["x-my-header"]);
            }

            [Fact]
            public void Add_header_list()
            {
                var model = new MandrillMessage();

                model.AddHeader("X-MY-HEADER", new[] { "foo", "bar" });

                Assert.Equal(new[] { "foo", "bar" }, model.Headers["x-my-header"].Values);
            }

            [Fact]
            public void Reply_to_sets_header()
            {
                var model = new MandrillMessage();

                Assert.Null(model.ReplyTo);
                model.ReplyTo = "reply@mandrilldotnet.org";
                Assert.Equal("reply@mandrilldotnet.org", model.ReplyTo);
                Assert.Equal("reply@mandrilldotnet.org", model.Headers["reply-to"]);
            }
        }
    }
}
