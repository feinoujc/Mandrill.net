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
            public void Can_add_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                var result = Api.Subaccounts.Add(id, name: "test", notes: notes, customQuota: null);
                result.Id.Should().Be(id);
            }
        }

        [Category("subaccounts/list.json")]
        private class List : Subaccounts
        {
            [Test]
            public void Can_list_subaccount()
            {
                var results = Api.Subaccounts.List(q:null);
                results.Count.Should().BeGreaterOrEqualTo(0);
            }
            [Test]
            public void Can_filter_subaccount()
            {
                var results = Api.Subaccounts.List(q: Guid.NewGuid().ToString("N"));
                results.Count.Should().Be(0);
            }
        }

        [Category("subaccounts/update.json")]
        private class Update : Subaccounts
        {
            [Test]
            public void Can_update_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                Api.Subaccounts.Add(id, name: "test", notes: notes, customQuota: null);

                var result = Api.Subaccounts.Update(id, name: "test", notes: "update", customQuota: 5000);
                result.CustomQuota.Should().Be(5000);
            }
          
        }

        [Category("subaccounts/pause.json")]
        private class Pause : Subaccounts
        {
            [Test]
            public void Can_pause_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                Api.Subaccounts.Add(id, name: "test", notes: notes, customQuota: null);

                var result = Api.Subaccounts.Pause(id);
                result.Status.Should().Be(MandrillSubaccountStatus.Paused);
            }

        }

        [Category("subaccounts/resume.json")]
        private class Resume : Subaccounts
        {
            [Test]
            public void Can_resume_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                Api.Subaccounts.Add(id, name: "test", notes: notes, customQuota: null);

                var result = Api.Subaccounts.Pause(id);
                result.Status.Should().Be(MandrillSubaccountStatus.Paused);

                result = Api.Subaccounts.Resume(id);
                result.Status.Should().Be(MandrillSubaccountStatus.Active);
            }

        }

        [Category("subaccounts/delete.json")]
        private class Delete : Subaccounts
        {
            [Test]
            public void Can_delete_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                Api.Subaccounts.Add(id, name: "test", notes: notes, customQuota: null);

                var result = Api.Subaccounts.Delete(id);
                result.Id.Should().Be(id);
            }

        }

        [Category("subaccounts/info.json")]
        private class Info : Subaccounts
        {
            [Test]
            public void Can_get_info_subaccount()
            {
                var id = Guid.NewGuid().ToString("N");
                var notes = "created by test at " + DateTime.UtcNow.ToString("s");
                Api.Subaccounts.Add(id, name: "test", notes: notes, customQuota: null);

                var result = Api.Subaccounts.Info(id);
                result.Id.Should().Be(id);
                result.Last30Days.Clicks.Should().Be(0);
            }

        }
    }
}
