using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using FluentAssertions;
using Mandrill.Model;
using Mandrill.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "serialization")]
    public class SerializationTests
    {
        public ITestOutputHelper Output { get; }

        public SerializationTests(ITestOutputHelper output)
        {
            Output = output;
        }

        // ── helpers ─────────────────────────────────────────────────────────────

        private static JsonElement Serialize<T>(T value)
        {
            var json = JsonSerializer.Serialize(value, MandrillSerializer.Instance);
            return JsonDocument.Parse(json).RootElement.Clone();
        }

        private static T Deserialize<T>(string json)
            => JsonSerializer.Deserialize<T>(json, MandrillSerializer.Instance);

        // ── tests ────────────────────────────────────────────────────────────────

        [Fact]
        public void Can_serialize_dates_as_unix_ts_by_default()
        {
            var date = DateTime.UtcNow;
            var expected = ToUnixTime(date);
            var model = new TestModel { Ts = date };

            var json = Serialize(model);

            json.GetProperty("ts").GetInt64().Should().Be(expected);
        }

        [Fact]
        public void Can_serialize_content_as_string()
        {
            var message = new MandrillMessage();
            message.GlobalMergeVars.Add(new MandrillMergeVar { Name = "test", Content = "some content" });

            var json = Serialize(message);

            json.GetProperty("global_merge_vars").GetArrayLength().Should().BeGreaterThan(0);
            json.GetProperty("global_merge_vars")[0].GetProperty("content").GetString().Should().Be("some content");
        }

        [Fact]
        public void Can_serialize_content_as_complex_associative_array()
        {
            var message = new MandrillMessage();

            var data = new IDictionary<string, object>[]
            {
                new Dictionary<string, object> { { "sku", "apples" }, { "unit_price", 0.20 } },
                new Dictionary<string, object> { { "sku", "oranges" }, { "unit_price", 0.40 } }
            };

            message.GlobalMergeVars.Add(new MandrillMergeVar { Name = "test", Content = data.ToList() });

            var json = Serialize(message);

            json.GetProperty("global_merge_vars").GetArrayLength().Should().BeGreaterThan(0);
            var contentRaw = json.GetProperty("global_merge_vars")[0].GetProperty("content").GetRawText();
            var result = JsonSerializer.Deserialize<List<Dictionary<string, JsonNode>>>(contentRaw, MandrillSerializer.Instance);

            result[0]["sku"].GetValue<string>().Should().Be("apples");
            result[0]["unit_price"].GetValue<double>().Should().Be(0.20);
            result[1]["sku"].GetValue<string>().Should().Be("oranges");
            result[1]["unit_price"].GetValue<double>().Should().Be(0.40);
        }

        [Fact]
        public void Can_serialize_content_including_nulls_by_default()
        {
            var message = new MandrillMessage();
            var data = new { FirstName = "test", LastName = (string)null, Items = new string[0] };

            message.GlobalMergeVars.Add(new MandrillMergeVar { Name = "test", Content = data });

            var json = Serialize(message);

            json.GetProperty("global_merge_vars").GetArrayLength().Should().BeGreaterThan(0);
            var result = json.GetProperty("global_merge_vars")[0].GetProperty("content");

            result.GetProperty("first_name").GetString().Should().Be("test");
            result.TryGetProperty("last_name", out var lastName).Should().BeTrue();
            lastName.ValueKind.Should().Be(JsonValueKind.Null);
            result.TryGetProperty("items", out var items).Should().BeTrue();
            items.GetArrayLength().Should().Be(0);
        }

        [Fact]
        public void Can_serialize_merge_var_content_using_custom_settings()
        {
            var message = new MandrillMessage();
            var data = new ContentModel { FirstName = "test", LastName = null };

            message.GlobalMergeVars.Add(new MandrillMergeVar { Name = "test", Content = data });

            var json = Serialize(message);

            json.GetProperty("global_merge_vars").GetArrayLength().Should().BeGreaterThan(0);
            var result = json.GetProperty("global_merge_vars")[0].GetProperty("content");

            result.GetProperty("FirstName").GetString().Should().Be("test");
            result.TryGetProperty("LastName", out var lastName).Should().BeTrue();
            lastName.ValueKind.Should().Be(JsonValueKind.Null);
        }

        [Fact]
        public void Can_serialize_merge_var_with_null_content()
        {
            var message = new MandrillMessage();
            string data = null;

            message.GlobalMergeVars.Add(new MandrillMergeVar { Name = "test", Content = data });

            var json = Serialize(message);

            json.GetProperty("global_merge_vars").GetArrayLength().Should().BeGreaterThan(0);
            var result = json.GetProperty("global_merge_vars")[0].GetProperty("content");

            Output.WriteLine(JsonSerializer.Serialize(message, new JsonSerializerOptions(MandrillSerializer.Instance) { WriteIndented = true }));
            result.ValueKind.Should().Be(JsonValueKind.Null);
        }

        [Fact]
        public void Can_set_property_name_by_convention()
        {
            var model = new TestModel { SomePropertyName = "foo" };

            var json = Serialize(model);

            json.GetProperty("some_property_name").GetString().Should().Be("foo");
        }

        [Fact]
        public void Skips_empty_arrays()
        {
            var model = new TestModel { List1 = new string[0] };

            var json = Serialize(model);

            json.TryGetProperty("list1", out _).Should().BeFalse();
        }

        [Fact]
        public void Includes_non_empty_arrays()
        {
            var model = new TestModel
            {
                List2 = new[]
                {
                    new TestSubModel { Name = "foo", Value = "baz" },
                    new TestSubModel { Name = "bar", Value = "bara" }
                }
            };

            var json = Serialize(model);

            var items = JsonSerializer.Deserialize<IList<TestSubModel>>(
                json.GetProperty("list2").GetRawText(), MandrillSerializer.Instance);
            items.Should().HaveCount(2);
        }

        [Fact]
        public void Includes_empty_required_arrays()
        {
            var model = new TestModel
            {
                List2 = new[]
                {
                    new TestSubModel { Name = "foo", Value = "baz" },
                    new TestSubModel { Name = "bar", Value = "bara" }
                }
            };

            var json = Serialize(model);

            var items = JsonSerializer.Deserialize<IList<TestSubModel>>(
                json.GetProperty("list2").GetRawText(), MandrillSerializer.Instance);
            items.Should().HaveCount(2);
        }

        [Fact]
        public void Can_covert_guid_in_short_format()
        {
            var model = new TestModel { Id = Guid.NewGuid().ToString("N") };

            var json = Serialize(model);

            json.GetProperty("_id").GetString().Should().Be(model.Id);
        }

        [Fact]
        public void Skips_null_values()
        {
            var model = new TestModel { Value1 = null };

            var json = Serialize(model);

            json.TryGetProperty("value1", out _).Should().BeFalse();
        }

        [Fact]
        public void Enums_camel_case()
        {
            var model = new[] { new TestModel { Enum = TestEnum.Reject }, new TestModel { Enum = TestEnum.SoftBounce } };

            var json = Serialize(model);

            json[0].GetProperty("enum").GetString().Should().Be("reject");
            json[1].GetProperty("enum").GetString().Should().Be("soft_bounce");
        }

        [Fact]
        public void Skips_empty_dictionary()
        {
            var model = new TestModel { Dictionary = null };

            var json = Serialize(model);

            json.TryGetProperty("dictionary", out _).Should().BeFalse();
        }

        [Fact]
        public void includes_non_empty_dictionary()
        {
            var model = new TestModel
            {
                Dictionary = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } }
            };

            var json = Serialize(model);

            json.TryGetProperty("dictionary", out var dictEl).Should().BeTrue();
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(dictEl.GetRawText());
            dictionary["key1"].Should().Be("value1");
            dictionary["key2"].Should().Be("value2");
        }

        [Fact]
        public void Can_deserialize_message()
        {
            string json = @"{
        ""html"": ""<p>Example HTML content</p>"",
        ""text"": ""Example text content"",
        ""subject"": ""example subject"",
        ""from_email"": ""message.from_email@mandrilldotnet.org"",
        ""from_name"": ""Example Name"",
        ""to"": [
            {
                ""email"": ""recipient.email@mandrilldotnet.org"",
                ""name"": ""Recipient Name"",
                ""type"": ""to""
            }
        ],
        ""headers"": {
            ""Reply-To"": ""message.reply@mandrilldotnet.org""
        },
        ""important"": false,
        ""track_opens"": null,
        ""track_clicks"": null,
        ""auto_text"": null,
        ""auto_html"": null,
        ""inline_css"": null,
        ""url_strip_qs"": null,
        ""preserve_recipients"": null,
        ""view_content_link"": null,
        ""bcc_address"": ""message.bcc_address@mandrilldotnet.org"",
        ""tracking_domain"": null,
        ""signing_domain"": null,
        ""return_path_domain"": null,
        ""merge"": true,
        ""merge_language"": ""mailchimp"",
        ""global_merge_vars"": [
            {
                ""name"": ""merge1"",
                ""content"": ""merge1 content""
            }
        ],
        ""merge_vars"": [
            {
                ""rcpt"": ""recipient.email@mandrilldotnet.org"",
                ""vars"": [
                    {
                        ""name"": ""merge2"",
                        ""content"": ""merge2 content""
                    }
                ]
            }
        ],
        ""tags"": [
            ""password-resets""
        ],
        ""subaccount"": ""customer-123"",
        ""google_analytics_domains"": [
            ""example.com""
        ],
        ""google_analytics_campaign"": ""message.from_email@mandrilldotnet.org"",
        ""metadata"": {
            ""website"": ""www.example.com""
        },
        ""recipient_metadata"": [
            {
                ""rcpt"": ""recipient.email@mandrilldotnet.org"",
                ""values"": {
                    ""user_id"": 123456
                }
            }
        ],
        ""attachments"": [
            {
                ""type"": ""text/plain"",
                ""name"": ""myfile.txt"",
                ""content"": ""bWFuZHJpbGwubmV0""
            }
        ],
        ""images"": [
            {
                ""type"": ""image/png"",
                ""name"": ""IMAGECID"",
                ""content"": ""bWFuZHJpbGwubmV0""
            }
        ]
    }";

            var message = Deserialize<MandrillMessage>(json);
            json = JsonSerializer.Serialize(message, MandrillSerializer.Instance);
            message = Deserialize<MandrillMessage>(json);

            message.Html.Should().Be("<p>Example HTML content</p>");
            message.Text.Should().Be("Example text content");
            message.Subject.Should().Be("example subject");
            message.FromEmail.Should().Be("message.from_email@mandrilldotnet.org");
            message.FromName.Should().Be("Example Name");
            message.To.Should().HaveCount(1);
            message.To[0].Email.Should().Be("recipient.email@mandrilldotnet.org");
            message.To[0].Name.Should().Be("Recipient Name");
            message.To[0].Type.Should().Be(MandrillMailAddressType.To);
            message.Headers.Should().HaveCount(1);
            message.Headers["Reply-To"].Should().Be("message.reply@mandrilldotnet.org");
            message.Important.Should().BeFalse();
            message.BccAddress.Should().Be("message.bcc_address@mandrilldotnet.org");
            message.Merge.Should().BeTrue();
            message.MergeLanguage.Should().Be(MandrillMessageMergeLanguage.Mailchimp);
            message.GlobalMergeVars.Should().HaveCount(1);
            message.GlobalMergeVars[0].Name.Should().Be("merge1");
            ((string)message.GlobalMergeVars[0].Content).Should().Be("merge1 content");
            message.RecipientMetadata.Should().HaveCount(1);
            message.RecipientMetadata[0].Rcpt.Should().Be("recipient.email@mandrilldotnet.org");
            message.RecipientMetadata[0].Values.Should().HaveCount(1);
            message.RecipientMetadata[0].Values["user_id"] = "123456";
            message.Tags.Should().HaveCount(1);
            message.Tags[0].Should().Be("password-resets");
            message.Subaccount.Should().Be("customer-123");
            message.GoogleAnalyticsDomains.Should().HaveCount(1);
            message.GoogleAnalyticsDomains[0].Should().Be("example.com");
            message.Metadata.Should().HaveCount(1);
            message.Metadata["website"].Should().Be("www.example.com");
            message.Attachments.Should().HaveCount(1);
            message.Attachments[0].Content.Should().NotBeNullOrEmpty();
            message.Attachments[0].Name.Should().Be("myfile.txt");
            Convert.ToBase64String(message.Attachments[0].Content).Should().Be("bWFuZHJpbGwubmV0");
            message.Images.Should().HaveCount(1);
            message.Images[0].Content.Should().NotBeNullOrEmpty();
            message.Images[0].Name.Should().Be("IMAGECID");
            Convert.ToBase64String(message.Images[0].Content).Should().Be("bWFuZHJpbGwubmV0");
        }

        [Fact]
        public void Can_serialize_message_web_hook()
        {
            string json = TestData.mandrill_webhook_example;

            var events = MandrillMessageEvent.ParseMandrillEvents(json);

            events.Should().NotBeNullOrEmpty();
            events.Should().HaveCount(14);

            Output.WriteLine(JsonSerializer.Serialize(events, MandrillSerializer.Instance));
        }

        [Fact]
        public void Can_serialize_message_web_hook_with_invalid_metadata()
        {
            string json = TestData.mandrill_webhook_invalid_metadata;

            var events = MandrillMessageEvent.ParseMandrillEvents(json);

            events.Should().NotBeNullOrEmpty();
            events.Should().HaveCount(2);

            Assert.Collection(events,
                e =>
                {
                    e.Msg.Metadata.Should().NotBeNull();
                    e.Msg.Metadata.Should().HaveCount(0);
                },
                e =>
                {
                    e.Msg.Metadata.Should().NotBeNull();
                    e.Msg.Metadata.Should().HaveCount(1);
                    e.Msg.Metadata["user_id"].Should().Be("111");
                });

            Output.WriteLine(JsonSerializer.Serialize(events, MandrillSerializer.Instance));
        }

        [Fact]
        public void Can_serialize_message_web_hook_with_invalid_longitude_latitude()
        {
            string json = TestData.mandrill_webhook_invalid;

            var events = MandrillMessageEvent.ParseMandrillEvents(json);

            events.Should().NotBeNullOrEmpty();
            events.Should().HaveCount(14);

            Output.WriteLine(JsonSerializer.Serialize(events, MandrillSerializer.Instance));
        }

        [Fact]
        public void Can_serialize_inbound_web_hook()
        {
            string json = TestData.mandrill_inbound;

            var events = MandrillInboundEvent.ParseMandrillEvents(json);

            events.Should().NotBeNullOrEmpty();
            events.Should().HaveCount(2);

            events[0].Msg.Headers.Should().NotBeEmpty();
            events[0].Msg.Headers["Content-Type"].Should()
                .Be("multipart/alternative; boundary=\"_av-7r7zDhHxVEAo2yMWasfuFw\"");

            events[0].Msg.To[0][0].Should().Be("test@inbound.example.com");

            events[0].Msg.Cc[0][0].Should().Be("testCc@inbound.example.com");

            events[1].Msg.Attachments.Count.Should().Be(1);
            events[1].Msg.Attachments.First().Value.Content.Should().NotBeEmpty();
            events[1].Msg.Images.Count.Should().Be(1);
            events[1].Msg.Images.First().Value.Content.Length.Should().BeGreaterThan(0);

            events[0].Msg.FromName.Should().Be("Example Sender");
            events[1].Msg.FromName.Should().BeNullOrEmpty();

            Output.WriteLine(JsonSerializer.Serialize(events, MandrillSerializer.Instance));
        }

        [Fact]
        public void Can_serialize_inbound_web_hook_with_empty_headers()
        {
            string json = TestData.mandrill_inbound_empty_headers;

            var events = MandrillInboundEvent.ParseMandrillEvents(json);

            events.Should().NotBeNullOrEmpty();
            events.Should().HaveCount(1);

            events[0].Msg.Headers.Should().BeEmpty();

            Output.WriteLine(JsonSerializer.Serialize(events, MandrillSerializer.Instance));
        }

        [Fact]
        public void Can_serialize_sync_web_hook()
        {
            string json = TestData.sample_sync_event;

            var events = MandrillSyncEvent.ParseSyncEvents(json);

            events.Should().NotBeNullOrEmpty();
            events.Should().HaveCount(1);

            events[0].Action.Should().Be(MandrillSyncAction.Add);
            events[0].Type.Should().Be(MandrillSyncType.Blacklist);
        }

        [Fact]
        public void Can_serialize_case_insensitive_header_dictionary()
        {
            string json = TestData.mandrill_inbound;

            var events = MandrillInboundEvent.ParseMandrillEvents(json);

            events.Should().NotBeNullOrEmpty();
            events.Should().HaveCount(2);

            events[0].Msg.Headers.Should().NotBeEmpty();
            events[0].Msg.Headers["Content-Type"].Should().NotBeNull();
            events[0].Msg.Headers["Content-Type"].Should().Be(events[0].Msg.Headers["CONTENT-TYPE"]);
        }

        [Fact]
        public void Can_verify_webhook_signature()
        {
            var formData = new NameValueCollection();
            formData["mandrill_events"] = TestData.sample_webhook;

            var result = WebHookSignatureHelper.VerifyWebHookSignature("NnvRYvKo0gA99/YGgRSb2JS4c/Y=", "f7YEknp5hLvZVw6BNSaM6g", new Uri("http://requestb.in/wvhpa9wv"), formData);
            Assert.True(result);

            var badResult = WebHookSignatureHelper.VerifyWebHookSignature("NnvRYvKo0gA99/YGgRSb2JS4c/Y=", "f7YEknp5hLvZVw6BNSaM6g", new Uri("http://requestb.in/wvhpa9wv?oof=1"), formData);
            Assert.False(badResult);
        }

        // ── private test models ──────────────────────────────────────────────────

        private class TestModel
        {
            public TestModel()
            {
                RequiredList = new string[0];
            }

            [JsonPropertyName("_id")]
            public string Id { get; set; }

            public DateTime? Ts { get; set; }
            public string SomePropertyName { get; set; }
            public string Value1 { get; set; }
            public IList<string> List1 { get; set; }
            public IList<TestSubModel> List2 { get; set; }
            public IDictionary<string, string> Dictionary { get; set; }

            public IList<string> RequiredList { get; set; }

            public TestEnum Enum { get; set; }
        }

        private enum TestEnum
        {
            Reject,
            SoftBounce
        }

        private class TestSubModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        static DateTime FromUnixTime(Int64 self)
        {
            var ret = new DateTime(1970, 1, 1);
            return ret.AddSeconds(self);
        }

        static Int64 ToUnixTime(DateTime self)
        {
            if (self == DateTime.MinValue) return 0;
            var epoc = new DateTime(1970, 1, 1);
            var delta = self - epoc;
            return (long)delta.TotalSeconds;
        }

        private class ContentModel
        {
            [JsonPropertyName("FirstName")]
            public string FirstName { get; set; }

            [JsonPropertyName("LastName")]
            public string LastName { get; set; }
        }
    }
}
