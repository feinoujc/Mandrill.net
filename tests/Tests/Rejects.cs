using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [Category("rejects")]
    internal class Rejects : IntegrationTest
    {
        private HashSet<string> _added = new HashSet<string>();

        public override void TearDown()
        {
            foreach (var id in _added)
            {
                var result = Api.Rejects.DeleteAsync(id).Result;
            }
            base.TearDown();
        }

        [Category("rejects/add.json")]
        private class Add : Rejects
        {
            [Test]
            public async Task Can_add_email_to_rejects()
            {
                var email = Guid.NewGuid().ToString("N") + "@example.com";
                var result = await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                result.Added.Should().BeTrue();
                _added.Add(email);
            }
        }

        [Category("rejects/delete.json")]
        private class Delete : Rejects
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
        private class List : Rejects
        {
            [Test]
            public async Task Can_list_filter_by_email()
            {
                var email = Guid.NewGuid().ToString("N") + "@example.com";
                await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                var results = await Api.Rejects.ListAsync(email, subaccount: null);
                results.Should().Contain(x => x.Email == email);
                results.Count.Should().Be(1);
                _added.Add(email);
            }


            [Test]
            public async Task Can_list_all()
            {
                var email = Guid.NewGuid().ToString("N") + "@example.com";
                await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                var results = await Api.Rejects.ListAsync(null, subaccount: null);
                results.Should().Contain(x => x.Email == email);
                results.Count.Should().BeGreaterOrEqualTo(1);
                _added.Add(email);
            }
        }
    }
}