using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "allowlists")]
    [Collection("allowlists")]
    public class Allowlists : IntegrationTest
    {
        private HashSet<string> _added = new HashSet<string>();

        public Allowlists(ITestOutputHelper output) : base(output)
        {
        }

        public override void Dispose()
        {
            foreach (var email in _added)
            {
                var result = Api.Allowlists.DeleteAsync(email).GetAwaiter().GetResult();
            }
        }

        [Trait("Category", "allowlists/list.json")]
        public class List : Allowlists
        {
            public List(ITestOutputHelper output) : base(output)
            {
            }

            [Fact]
            public async Task Can_list_all()
            {
                string email = null;
                var results = await Api.Allowlists.ListAsync(email);

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.CreatedAt).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Output.WriteLine("no emails found on allowlist.");
                }
            }

            [Fact]
            public async Task Can_list_all_filtered()
            {
                string email = null;
                var entirelist = await Api.Allowlists.ListAsync(email);

                //the api doesn't return results immediately, it may return no results
                var found = entirelist.OrderBy(x => x.CreatedAt).LastOrDefault();
                if (found != null)
                {
                    var result = await Api.Allowlists.ListAsync(found.Email);
                    string allowlistemail = result.FirstOrDefault().Email;
                    allowlistemail.Should().NotBeNullOrEmpty();
                    allowlistemail.Should().Be(found.Email);
                }
                else
                {
                    Output.WriteLine("no emails on allowlist found.");
                }
            }
        }

        [Trait("Category", "allowlists/add.json")]
        public class Add : Allowlists
        {
            public Add(ITestOutputHelper output) : base(output)
            {
            }

            [Fact]
            public async Task Can_add_allowlist()
            {
                var email = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                var result = await Api.Allowlists.AddAsync(email);
                result.Added.Should().BeTrue();
                _added.Add(result.Email);
            }
        }

        [Trait("Category", "allowlists/delete.json")]
        public class Delete : Allowlists
        {
            public Delete(ITestOutputHelper output) : base(output)
            {
            }

            [Fact]
            public async Task Can_delete_allowlist()
            {
                var email = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                await Api.Allowlists.AddAsync(email);

                var result = await Api.Allowlists.DeleteAsync(email);
                result.Deleted.Should().BeTrue();
            }
        }
    }
}
