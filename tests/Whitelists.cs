using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Tests
{
    [Trait("Category", "whitelists")]
    [Collection("whitelists")]
    public class Whitelists : IntegrationTest
    {
        private HashSet<string> _added = new HashSet<string>();

        public override void Dispose()
        {
            foreach (var email in _added)
            {
                var result = Api.Whitelists.DeleteAsync(email).GetAwaiter().GetResult();
            }
        }

        [Trait("Category", "whitelists/list.json")]
        public class List : Whitelists
        {
            [Fact]
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
                    Console.Error.WriteLine("no emails found on whitelist.");
                }
            }

            [Fact]
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
                    Console.Error.WriteLine("no emails on whitelist found.");
                }
            }
        }

        [Trait("Category", "whitelists/add.json")]
        public class Add : Whitelists
        {
            [Fact]
            public async Task Can_add_whitelist()
            {
                var email = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                var result = await Api.Whitelists.AddAsync(email);
                result.Added.Should().BeTrue();
                _added.Add(result.Email);
            }
        }

        [Trait("Category", "whitelists/delete.json")]
        public class Delete : Whitelists
        {
            [Fact]
            public async Task Can_delete_whitelist()
            {
                var email = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                await Api.Whitelists.AddAsync(email);

                var result = await Api.Whitelists.DeleteAsync(email);
                result.Deleted.Should().BeTrue();
            }
        }
    }
}
