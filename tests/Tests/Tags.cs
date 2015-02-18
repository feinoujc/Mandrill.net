using System;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [Category("tags")]
    class Tags : IntegrationTest
    {

        [Category("tags/list.json")]
        class List : Tags
        {
            [Test]
            public async void Can_list_all()
            {
                string tag = "test tag";
                var results = await Api.Tags.ListAsync();
                results.Should().Contain(x => x.Tag == tag);
                results.Count.Should().BeGreaterOrEqualTo(1);
            }
        }

        [Category("tags/info.json")]
        class Info : Tags
        {
            [Test]
            public async void Can_retrieve_info()
            {
                string tag = "test tag";
                var result = await Api.Tags.InfoAsync(tag);
                result.Should().NotBeNull();
                result.Tag.Should().Be(tag);
            }
        }

        [Category("tags/delete.json")]
        class Delete : Tags
        {
            [Test]
            public async void Can_delete_tag()
            {
                string tag = "test tag";
                var result = await Api.Tags.DeleteAsync(tag);
                result.Should().NotBeNull();
                result.Tag.Should().Be(tag);
            }
        }
    }
}
