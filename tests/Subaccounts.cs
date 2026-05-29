using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Model;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "subaccounts")]
    [Collection("subaccounts")]
    public class Subaccounts(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;
        private HashSet<string> _added = new();

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual async Task DisposeAsync()
        {
            foreach (var id in _added)
            {
                await Api.Subaccounts.DeleteAsync(id);
            }
        }

        [Trait("Category", "subaccounts/add.json")]
        public class Add : Subaccounts
        {
            public Add(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_add_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                var result = await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);
                Assert.Equal(id, result.Id);
                _added.Add(result.Id);
            }
        }

        [Trait("Category", "subaccounts/list.json")]
        public class List : Subaccounts
        {
            public List(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_subaccount()
            {
                var results = await Api.Subaccounts.ListAsync(q: null);
                Assert.True(results.Count >= 0);
            }
            [Fact]
            public async Task Can_filter_subaccount()
            {
                var results = await Api.Subaccounts.ListAsync(q: Guid.NewGuid().ToString("N"));
                Assert.Empty(results);
            }
        }

        [Trait("Category", "subaccounts/update.json")]
        public class Update : Subaccounts
        {
            public Update(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_update_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.UpdateAsync(id, name: "test", notes: "update", customQuota: 5000);
                Assert.Equal(5000, result.CustomQuota);
                _added.Add(result.Id);
            }

        }

        [Trait("Category", "subaccounts/pause.json")]
        public class Pause : Subaccounts
        {
            public Pause(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_pause_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.PauseAsync(id);
                Assert.Equal(MandrillSubaccountStatus.Paused, result.Status);
                _added.Add(result.Id);
            }

        }

        [Trait("Category", "subaccounts/resume.json")]
        public class Resume : Subaccounts
        {
            public Resume(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_resume_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.PauseAsync(id);
                Assert.Equal(MandrillSubaccountStatus.Paused, result.Status);

                result = await Api.Subaccounts.ResumeAsync(id);
                Assert.Equal(MandrillSubaccountStatus.Active, result.Status);
                _added.Add(result.Id);
            }

        }

        [Trait("Category", "subaccounts/delete.json")]
        public class Delete : Subaccounts
        {
            public Delete(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_delete_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.DeleteAsync(id);
                Assert.Equal(id, result.Id);
            }

        }

        [Trait("Category", "subaccounts/info.json")]
        public class Info : Subaccounts
        {
            public Info(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_get_info_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.InfoAsync(id);
                Assert.Equal(id, result.Id);
                Assert.Equal(0, result.Last30Days.Clicks);
                Assert.Null(result.FirstSentAt);

                _added.Add(result.Id);
            }
        }
    }
}
