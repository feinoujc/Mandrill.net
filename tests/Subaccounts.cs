using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill.Model;
using Xunit;
using Mandrill;

namespace Tests
{
    [Trait("Category", "subaccounts")]
    [Collection("subaccounts")]
    public class Subaccounts : IntegrationTest
    {
        HashSet<string> _added = new HashSet<string>();

        public override void Dispose()
        {
            foreach (var id in _added)
            {
                var result = Api.Subaccounts.DeleteAsync(id).GetAwaiter().GetResult();
            }
            base.Dispose();
        }

        [Trait("Category", "subaccounts/add.json")]
        public class Add : Subaccounts
        {
            [Fact]
            public async Task Can_add_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                var result = await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);
                result.Id.Should().Be(id);
                _added.Add(result.Id);
            }
        }

        [Trait("Category", "subaccounts/list.json")]
        public class List : Subaccounts
        {
            [Fact]
            public async Task Can_list_subaccount()
            {
                var results = await Api.Subaccounts.ListAsync(q:null);
                results.Count.Should().BeGreaterOrEqualTo(0);
            }
            [Fact]
            public async Task Can_filter_subaccount()
            {
                var results = await Api.Subaccounts.ListAsync(q: Guid.NewGuid().ToString("N"));
                results.Count.Should().Be(0);
            }
        }

        [Trait("Category", "subaccounts/update.json")]
        public class Update : Subaccounts
        {
            [Fact]
            public async Task Can_update_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.UpdateAsync(id, name: "test", notes: "update", customQuota: 5000);
                result.CustomQuota.Should().Be(5000);
                _added.Add(result.Id);
            }

        }

        [Trait("Category", "subaccounts/pause.json")]
        public class Pause : Subaccounts
        {
            [Fact]
            public async Task Can_pause_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.PauseAsync(id);
                result.Status.Should().Be(MandrillSubaccountStatus.Paused);
                _added.Add(result.Id);
            }

        }

        [Trait("Category", "subaccounts/resume.json")]
        public class Resume : Subaccounts
        {
            [Fact]
            public async Task Can_resume_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.PauseAsync(id);
                result.Status.Should().Be(MandrillSubaccountStatus.Paused);

                result = await Api.Subaccounts.ResumeAsync(id);
                result.Status.Should().Be(MandrillSubaccountStatus.Active);
                _added.Add(result.Id);
            }

        }

        [Trait("Category", "subaccounts/delete.json")]
        public class Delete : Subaccounts
        {
            [Fact]
            public async Task Can_delete_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.DeleteAsync(id);
                result.Id.Should().Be(id);
            }

        }

        [Trait("Category", "subaccounts/info.json")]
        public class Info : Subaccounts
        {
            [Fact]
            public async Task Can_get_info_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.InfoAsync(id);
                result.Id.Should().Be(id);
                result.Last30Days.Clicks.Should().Be(0);
                result.FirstSentAt.Should().Be((DateTime?) null);

                _added.Add(result.Id);


            }


        }
    }
}
