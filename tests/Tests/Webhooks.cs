using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Mandrill.Model;
using NUnit.Framework;

namespace Tests
{
    [Category("webhooks")]
    internal class Webhooks : IntegrationTest
    {
        protected Uri WebhookUri { get; set; }
        private HashSet<int> _added = new HashSet<int>();

        [SetUp]
        public void SetUp()
        {
            _added.Clear();
            var configuredWebHook = Environment.GetEnvironmentVariable("MANDRILL_OUTBOUND_WEBHOOK") ?? "http://devnull-as-a-service.com/dev/null";

            WebhookUri = new Uri(configuredWebHook);

            //configure webhook api at http://requestb.in
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var id in _added)
            {
                var result = Api.WebHooks.DeleteAsync(id).GetAwaiter().GetResult();
            }
        }

        [Test]
        public async Task Can_add()
        {
            var result = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] {MandrillWebHookEventType.Unsub,});
            result.Should().NotBeNull();
            _added.Add(result.Id);

            result.Events.Should().NotBeEmpty();
            result.Events[0].Should().Be(MandrillWebHookEventType.Unsub);
        }

        [Test]
        public async Task Throws_when_bad_url()
        {
            await ThrowsAsync<MandrillException>(() => Api.WebHooks.AddAsync(new Uri("http://www.invalid_url.org")));
        }

        [Test]
        public async Task Can_list()
        {
            var result = await Api.WebHooks.ListAsync();
            result.Count.Should().BeGreaterOrEqualTo(0);
        }

        [Test]
        public async Task Can_delete()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] {MandrillWebHookEventType.Unsub,});
            _added.Add(added.Id);

            var result = await Api.WebHooks.DeleteAsync(added.Id);
            result.Id.Should().Be(added.Id);
            _added.Remove(result.Id);
        }

        [Test]
        public async Task Can_get()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] {MandrillWebHookEventType.Unsub,});
            _added.Add(added.Id);

            var result = await Api.WebHooks.InfoAsync(added.Id);
            result.Id.Should().Be(added.Id);
        }

        [Test]
        public async Task Can_update()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] {MandrillWebHookEventType.Unsub,});
            _added.Add(added.Id);

            var result = await Api.WebHooks.UpdateAsync(added.Id, WebhookUri, description: "An updated description", events: new[] {MandrillWebHookEventType.Unsub,});
            result.Id.Should().Be(added.Id);
        }
    }
}