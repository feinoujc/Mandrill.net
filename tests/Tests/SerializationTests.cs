using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
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
            var model = new TestModel {Id = Guid.NewGuid()};

            var json = JObject.FromObject(model, MandrillSerializer.Instance);

            json["_id"].Value<string>().Should().Be(model.Id.ToString("N"));
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


        private class TestModel
        {
            public TestModel()
            {
                RequiredList = new string[0];
            }
            [JsonProperty("_id")]
            public Guid Id { get; set; }

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