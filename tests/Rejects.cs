using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                Assert.True(result.Added);
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
                Assert.True(result.Deleted);
            }
        }

        [Trait("Category", "rejects/add-sms.json")]
        public class AddSms : Rejects
        {
            public AddSms(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact(Skip = "Requires SMS feature enabled on account")]
            public async Task Can_add_phone_to_sms_rejects()
            {
                var result = await Api.Rejects.AddSmsAsync("+10000000000", comment: "test");
                Assert.True(result.Added);
            }
        }

        [Trait("Category", "rejects/delete-sms.json")]
        public class DeleteSms : Rejects
        {
            public DeleteSms(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact(Skip = "Requires SMS feature enabled on account")]
            public async Task Can_delete_phone_from_sms_rejects()
            {
                await Api.Rejects.AddSmsAsync("+10000000000");
                var result = await Api.Rejects.DeleteSmsAsync("+10000000000");
                Assert.True(result.Deleted);
            }
        }

        [Trait("Category", "rejects/list-sms.json")]
        public class ListSms : Rejects
        {
            public ListSms(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact(Skip = "Requires SMS feature enabled on account")]
            public async Task Can_list_sms_rejects()
            {
                var results = await Api.Rejects.ListSmsAsync();
                Assert.NotNull(results);
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
                Assert.Contains(results, x => x.Email == email);
                Assert.Single(results);
                _added.Add(email);
            }


            [Fact]
            public async Task Can_list_all()
            {
                var email = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                await Api.Rejects.AddAsync(email, comment: "test", subaccount: null);
                var results = await Api.Rejects.ListAsync(null, subaccount: null);
                Assert.Contains(results, x => x.Email == email);
                Assert.True(results.Count >= 1);
                _added.Add(email);
            }
        }
    }
}
