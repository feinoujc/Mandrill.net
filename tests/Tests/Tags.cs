using System;
using System.Collections.Generic;
using System.Linq;
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
                var results = await Api.Tags.ListAsync();

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.Tag).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Assert.Inconclusive("no tags found.");
                }
            }
        }

        [Category("tags/info.json")]
        class Info : Tags
        {
            [Test]
            public async void Can_retrieve_info()
            {
                var tag = (await Api.Tags.ListAsync()).LastOrDefault();
                if (tag != null)
                {
                    var result = await Api.Tags.InfoAsync(tag.Tag);
                    result.Should().NotBeNull();
                    result.Tag.Should().Be(tag.Tag);
                }
                else
                {
                    Assert.Inconclusive("no tags found");
                }
            }
        }

        [Category("tags/delete.json")]
        class Delete : Tags
        {
            [Test]
            public async void Can_delete_tag()
            {
                var tag = (await Api.Tags.ListAsync()).LastOrDefault();
                if (tag != null)
                {
                    var result = await Api.Tags.DeleteAsync(tag.Tag);
                    result.Should().NotBeNull();
                    result.Tag.Should().Be(tag.Tag);
                }
                else
                {
                    Assert.Inconclusive("no tags found");
                }
            }
        }
    }
}
