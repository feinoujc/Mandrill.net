using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "rejects")]
    [Collection("rejects")]
    public class Rejects(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;
        private HashSet<string> _added = new();

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual async Task DisposeAsync()
        {
            foreach (var id in _added)
            {
                await Api.Rejects.DeleteAsync(id);
            }
        }

        [Trait("Category", "rejects/add.json")]
        public class Add : Rejects
        {
            public Add(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_add_email_to_rejects()
            {
                var email = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                var result = await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                result.Added.Should().BeTrue();
                _added.Add(email);
            }
        }

        [Trait("Category", "rejects/delete.json")]
        public class Delete : Rejects
        {
            public Delete(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_delete_email_from_rejects()
            {
                var email = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                var result = await Api.Rejects.DeleteAsync(email, subaccount: null);
                result.Deleted.Should().BeTrue();
            }
        }

        [Trait("Category", "rejects/list.json")]
        public class List : Rejects
        {
            public List(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_filter_by_email()
            {
                var email = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                var results = await Api.Rejects.ListAsync(email, subaccount: null);
                results.Should().Contain(x => x.Email == email);
                results.Count.Should().Be(1);
                _added.Add(email);
            }


            [Fact]
            public async Task Can_list_all()
            {
                var email = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                var results = await Api.Rejects.ListAsync(null, subaccount: null);
                results.Should().Contain(x => x.Email == email);
                results.Count.Should().BeGreaterOrEqualTo(1);
                _added.Add(email);
            }
        }
    }
}
