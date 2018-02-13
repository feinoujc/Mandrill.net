using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Mandrill.Model;
using Xunit;

namespace Tests
{
    [Trait("Category", "webhooks")]
    [Collection("webhooks")]
    public class Webhooks : IntegrationTest
    {
        protected Uri WebhookUri { get; set; }
        private HashSet<int> _added = new HashSet<int>();

        public Webhooks()
        {
            _added.Clear();
            var configuredWebHook = Environment.GetEnvironmentVariable("MANDRILL_OUTBOUND_WEBHOOK") ?? "https://reqres.in/api/mandrill-webhook-test";

            WebhookUri = new Uri(configuredWebHook);

            //configure webhook api at http://requestb.in
        }

        public override void Dispose()
        {
            foreach (var id in _added)
            {
                var result = Api.WebHooks.DeleteAsync(id).GetAwaiter().GetResult();
            }
            base.Dispose();
        }

        [Fact]
        public async Task Can_add()
        {
            var result = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] { MandrillWebHookEventType.Unsub, });
            result.Should().NotBeNull();
            _added.Add(result.Id);

            result.Events.Should().NotBeEmpty();
            result.Events[0].Should().Be(MandrillWebHookEventType.Unsub);
        }

        [Fact]
        public async Task Throws_when_bad_url()
        {
            await Assert.ThrowsAsync<MandrillException>(() => Api.WebHooks.AddAsync(new Uri("http://www.invalid_url.org")));
        }

        [Fact]
        public async Task Can_list()
        {
            var result = await Api.WebHooks.ListAsync();
            result.Count.Should().BeGreaterOrEqualTo(0);
        }

        [Fact]
        public async Task Can_delete()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] { MandrillWebHookEventType.Unsub, });
            _added.Add(added.Id);

            var result = await Api.WebHooks.DeleteAsync(added.Id);
            result.Id.Should().Be(added.Id);
            _added.Remove(result.Id);
        }

        [Fact]
        public async Task Can_get()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] { MandrillWebHookEventType.Unsub, });
            _added.Add(added.Id);

            var result = await Api.WebHooks.InfoAsync(added.Id);
            result.Id.Should().Be(added.Id);
        }

        [Fact]
        public async Task Can_update()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] { MandrillWebHookEventType.Unsub, });
            _added.Add(added.Id);

            var result = await Api.WebHooks.UpdateAsync(added.Id, WebhookUri, description: "An updated description", events: new[] { MandrillWebHookEventType.Unsub, });
            result.Id.Should().Be(added.Id);
        }
    }
}
