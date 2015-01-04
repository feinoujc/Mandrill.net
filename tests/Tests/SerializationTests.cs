using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using FluentAssertions;
using Mandrill.Model;
using Mandrill.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tests
{
    [Category("serialization")]
    internal class SerializationTests
    {
        [Test]
        public void Can_serialize_dates_as_unix_ts_by_default()
        {
            var date = DateTime.UtcNow;
            var expected = date.ToUnixTime();
            var model = new TestModel {Ts = date};

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["ts"].Value<long>().Should().Be(expected);
        }

        [Test]
        public void Can_set_property_name_by_convention()
        {
            var model = new TestModel {SomePropertyName = "foo"};

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["some_property_name"].Value<string>().Should().Be("foo");
        }

        [Test]
        public void Skips_empty_arrays()
        {
            var model = new TestModel {List1 = new string[0]};

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["list1"].Should().BeNull();
        }

        [Test]
        public void Includes_non_empty_arrays()
        {
            var model = new TestModel
            {
                List2 = new[]
                {
                    new TestSubModel {Name = "foo", Value = "baz"},
                    new TestSubModel {Name = "bar", Value = "bara"}
                }
            };

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["list2"].ToObject<IList<TestSubModel>>(MandrillSerializer.Instance)
                .Should()
                .HaveCount(2);
        }

        [Test]
        public void Includes_empty_required_arrays()
        {
            var model = new TestModel
            {
                List2 = new[]
                {
                    new TestSubModel {Name = "foo", Value = "baz"},
                    new TestSubModel {Name = "bar", Value = "bara"}
                }
            };

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["list2"].ToObject<IList<TestSubModel>>(MandrillSerializer.Instance)
                .Should()
                .HaveCount(2);
        }


        [Test]
        public void Can_covert_guid_in_short_format()
        {
            var model = new TestModel {Id = Guid.NewGuid().ToString("N")};

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["_id"].Value<string>().Should().Be(model.Id);
        }


        [Test]
        public void Skips_null_values()
        {
            var model = new TestModel {Value1 = null};

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["value1"].Should().BeNull();
        }

        [Test]
        public void Skips_empty_dictionary()
        {
            var model = new TestModel {Dictionary = null};

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["dictionary"].Should().BeNull();
        }

        [Test]
        public void includes_non_empty_dictionary()
        {
            var model = new TestModel
            {
                Dictionary = new Dictionary<string, string> {{"key1", "value1"}, {"key2", "value2"}}
            };

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["dictionary"].Should().NotBeNull();
            var dictionary = json["dictionary"].ToObject<Dictionary<string, string>>();
            dictionary["key1"].Should().Be("value1");
            dictionary["key2"].Should().Be("value2");
        }

        [Test]
        public void Can_deserialize_message()
        {
            string json = @"{
        ""html"": ""<p>Example HTML content</p>"",
        ""text"": ""Example text content"",
        ""subject"": ""example subject"",
        ""from_email"": ""message.from_email@example.com"",
        ""from_name"": ""Example Name"",
        ""to"": [
            {
                ""email"": ""recipient.email@example.com"",
                ""name"": ""Recipient Name"",
                ""type"": ""to""
            }
        ],
        ""headers"": {
            ""Reply-To"": ""message.reply@example.com""
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
        ""bcc_address"": ""message.bcc_address@example.com"",
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
                ""rcpt"": ""recipient.email@example.com"",
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
        ""google_analytics_campaign"": ""message.from_email@example.com"",
        ""metadata"": {
            ""website"": ""www.example.com""
        },
        ""recipient_metadata"": [
            {
                ""rcpt"": ""recipient.email@example.com"",
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


            var message = JToken.Load(new JsonTextReader(new StringReader(json))).ToObject<MandrillMessage>(MandrillSerializer.Instance);
            json = JObject.FromObject(message, MandrillSerializer.Instance).ToString();
            message = JToken.Load(new JsonTextReader(new StringReader(json))).ToObject<MandrillMessage>(MandrillSerializer.Instance);

            message.Html.Should().Be("<p>Example HTML content</p>");
            message.Text.Should().Be("Example text content");
            message.Subject.Should().Be("example subject");
            message.FromEmail.Should().Be("message.from_email@example.com");
            message.FromName.Should().Be("Example Name");
            message.To.Should().HaveCount(1);
            message.To[0].Email.Should().Be("recipient.email@example.com");
            message.To[0].Name.Should().Be("Recipient Name");
            message.To[0].Type.Should().Be(MandrillMailAddressType.To);
            message.Headers.Should().HaveCount(1);
            message.Headers["Reply-to"].Should().Be("message.reply@example.com");
            message.Important.Should().BeFalse();
            message.BccAddress.Should().Be("message.bcc_address@example.com");
            message.Merge.Should().BeTrue();
            message.MergeLanguage.Should().Be("mailchimp");
            message.GlobalMergeVars.Should().HaveCount(1);
            message.GlobalMergeVars[0].Name.Should().Be("merge1");
            message.GlobalMergeVars[0].Content.Should().Be("merge1 content");
            message.RecipientMetadata.Should().HaveCount(1);
            message.RecipientMetadata[0].Rcpt.Should().Be("recipient.email@example.com");
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

        [Test]
        public void Can_serialize_web_hook()
        {
            string json = Properties.Resources.mandrill_webhook_example_json;

            var events = MandrillMessageEvent.ParseMandrillEvents(json);

            events.Should().NotBeNullOrEmpty();
            events.Should().HaveCount(14);

            Console.WriteLine(JArray.FromObject(events, MandrillSerializer.Instance).ToString());
        
        }


        private class TestModel
        {
            public TestModel()
            {
                RequiredList = new string[0];
            }

            [JsonProperty("_id")]
            public string Id { get; set; }

            public DateTime Ts { get; set; }
            public string SomePropertyName { get; set; }
            public string Value1 { get; set; }
            public IList<string> List1 { get; set; }
            public IList<TestSubModel> List2 { get; set; }
            public IDictionary<string, string> Dictionary { get; set; }

            [Required]
            public IList<string> RequiredList { get; set; }
        }

        private class TestSubModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}