using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [Category("whitelists")]
    class Whitelists : IntegrationTest
    {
        private HashSet<string> _added = new HashSet<string>();
        public override void TearDown()
        {
            foreach (var email in _added)
            {
                var result = Api.Whitelists.DeleteAsync(email).Result;
            }
            base.TearDown();
        }

        [Category("whitelists/list.json")]
        class List : Whitelists
        {
            [Test]
            public async Task Can_list_all()
            {
                string email = null;
                var results = await Api.Whitelists.ListAsync(email);

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.CreatedAt).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Assert.Inconclusive("no emails found on whitelist.");
                }
            }
            
            [Test]
            public async Task Can_list_all_filtered()
            {
                string email = null;
                var entirelist = await Api.Whitelists.ListAsync(email);

                //the api doesn't return results immediately, it may return no results
                var found = entirelist.OrderBy(x => x.CreatedAt).LastOrDefault();
                if (found != null)
                {
                    var result = await Api.Whitelists.ListAsync(found.Email);
                    string whitelistemail = result.FirstOrDefault().Email;
                    whitelistemail.Should().NotBeNullOrEmpty();
                    whitelistemail.Should().Be(found.Email);                    
                }
                else
                {
                    Assert.Inconclusive("no emails on whitelist found.");
                }
            }
        }

        [Category("whitelists/add.json")]
        class Add : Whitelists 
        {
            [Test]
            public async Task Can_add_whitelist()
            {
                var email = Guid.NewGuid().ToString("N") + "@example.com";
                var result = await Api.Whitelists.AddAsync(email);
                result.Added.Should().BeTrue();
                _added.Add(result.Email);
            }
        }

        [Category("whitelists/delete.json")]
        class Delete : Whitelists
        {
            [Test]
            public async Task Can_delete_whitelist()
            {
                var email = Guid.NewGuid().ToString("N") + "@example.com";
                await Api.Whitelists.AddAsync(email);

                var result = await Api.Whitelists.DeleteAsync(email);
                result.Deleted.Should().BeTrue();
            }
        }
    }
}
