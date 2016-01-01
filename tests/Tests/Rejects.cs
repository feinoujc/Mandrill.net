using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [Category("rejects")]
    class Rejects : IntegrationTest
    {
        [Category("rejects/add.json")]
        class Add:Rejects
        {
            [Test]
            public async Task Can_add_email_to_rejects()
            {
                var email = Guid.NewGuid().ToString("N") + "@example.com";
                var result = await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                result.Added.Should().BeTrue();
            }
        }

        [Category("rejects/delete.json")]
        class Delete : Rejects
        {
            [Test]
            public async Task Can_delete_email_from_rejects()
            {
                var email = Guid.NewGuid().ToString("N") + "@example.com";
                await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                var result = await Api.Rejects.DeleteAsync(email, subaccount: null);
                result.Deleted.Should().BeTrue();
            }
        }

        [Category("rejects/list.json")]
        class List : Rejects
        {
            [Test]
            public async Task Can_list_filter_by_email()
            {
                var email = Guid.NewGuid().ToString("N") + "@example.com";
                await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                var results = await Api.Rejects.ListAsync(email, subaccount: null);
                results.Should().Contain(x => x.Email == email);
                results.Count.Should().Be(1);
            }


            [Test]
            public async Task Can_list_all()
            {
                var email = Guid.NewGuid().ToString("N") + "@example.com";
                await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                var results = await Api.Rejects.ListAsync(null, subaccount: null);
                results.Should().Contain(x => x.Email == email);
                results.Count.Should().BeGreaterOrEqualTo(1);
            }
        }
    }
}
