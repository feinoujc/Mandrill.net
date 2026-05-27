using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "allowlists")]
    [Collection("allowlists")]
    public class Allowlists(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        private readonly HashSet<string> _added = [];

        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual async Task DisposeAsync()
        {
            foreach (var email in _added)
                await Api.Allowlists.DeleteAsync(email);
        }

        [Trait("Category", "allowlists/list.json")]
        public class List(MandrillFixture fixture, ITestOutputHelper output) : Allowlists(fixture, output)
        {
            [Fact]
            public async Task Can_list_all()
            {
                string email = null;
                var results = await Api.Allowlists.ListAsync(email);

                var found = results.OrderBy(x => x.CreatedAt).FirstOrDefault();
                if (found != null)
                    results.Count.Should().BeGreaterOrEqualTo(1);
                else
                    Output.WriteLine("no emails found on allowlist.");
            }

            [Fact]
            public async Task Can_list_all_filtered()
            {
                string email = null;
                var entirelist = await Api.Allowlists.ListAsync(email);

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
        public class Add(MandrillFixture fixture, ITestOutputHelper output) : Allowlists(fixture, output)
        {
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
        public class Delete(MandrillFixture fixture, ITestOutputHelper output) : Allowlists(fixture, output)
        {
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
