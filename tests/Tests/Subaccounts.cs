using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill.Model;
using NUnit.Framework;
using Mandrill;

namespace Tests
{
    [Category("subaccounts")]
    internal class Subaccounts : IntegrationTest
    {
        [Category("subaccounts/add.json")]
        private class Add : Subaccounts
        {
            [Test]
            public async void Can_add_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                var result = await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);
                result.Id.Should().Be(id);
            }
        }

        [Category("subaccounts/list.json")]
        private class List : Subaccounts
        {
            [Test]
            public async void Can_list_subaccount()
            {
                var results = await Api.Subaccounts.ListAsync(q:null);
                results.Count.Should().BeGreaterOrEqualTo(0);
            }
            [Test]
            public async void Can_filter_subaccount()
            {
                var results = await Api.Subaccounts.ListAsync(q: Guid.NewGuid().ToString("N"));
                results.Count.Should().Be(0);
            }
        }

        [Category("subaccounts/update.json")]
        private class Update : Subaccounts
        {
            [Test]
            public async void Can_update_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.UpdateAsync(id, name: "test", notes: "update", customQuota: 5000);
                result.CustomQuota.Should().Be(5000);
            }
          
        }

        [Category("subaccounts/pause.json")]
        private class Pause : Subaccounts
        {
            [Test]
            public async void Can_pause_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.PauseAsync(id);
                result.Status.Should().Be(MandrillSubaccountStatus.Paused);
            }

        }

        [Category("subaccounts/resume.json")]
        private class Resume : Subaccounts
        {
            [Test]
            public async void Can_resume_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.PauseAsync(id);
                result.Status.Should().Be(MandrillSubaccountStatus.Paused);

                result = await Api.Subaccounts.ResumeAsync(id);
                result.Status.Should().Be(MandrillSubaccountStatus.Active);
            }

        }

        [Category("subaccounts/delete.json")]
        private class Delete : Subaccounts
        {
            [Test]
            public async void Can_delete_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.DeleteAsync(id);
                result.Id.Should().Be(id);
            }

        }

        [Category("subaccounts/info.json")]
        private class Info : Subaccounts
        {
            [Test]
            public async void Can_get_info_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                await Api.Subaccounts.AddAsync(id, name: "test", notes: notes, customQuota: null);

                var result = await Api.Subaccounts.InfoAsync(id);
                result.Id.Should().Be(id);
                result.Last30Days.Clicks.Should().Be(0);
            }

        }
    }
}
