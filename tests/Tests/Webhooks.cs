using System;
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

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var configuredWebHook = Environment.GetEnvironmentVariable("MANDRILL_OUTBOUND_WEBHOOK") ?? "http://devnull-as-a-service.com/dev/null";

            WebhookUri = new Uri(configuredWebHook);

            //configure webhook api at http://requestb.in
        }

        [Test]
        public async void Can_add()
        {
            var result = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] {MandrillWebHookEventType.Unsub,});
            result.Should().NotBeNull();
            result.Events.Should().NotBeEmpty();
            result.Events[0].Should().Be(MandrillWebHookEventType.Unsub);
        }

        [Test]
        public void Throws_when_bad_url()
        {
            Assert.Throws<MandrillException>(() => Api.WebHooks.AddAsync(new Uri("http://www.invalid_url.org")).Wait());
        }

        [Test]
        public async void Can_list()
        {
            var result = await Api.WebHooks.ListAsync();
            result.Count.Should().BeGreaterOrEqualTo(0);
        }

        [Test]
        public async void Can_delete()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] {MandrillWebHookEventType.Unsub,});
            var result = await Api.WebHooks.DeleteAsync(added.Id);
            result.Id.Should().Be(added.Id);
        }

        [Test]
        public async void Can_get()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] {MandrillWebHookEventType.Unsub,});
            var result = await Api.WebHooks.InfoAsync(added.Id);
            result.Id.Should().Be(added.Id);
        }

        [Test]
        public async void Can_update()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] {MandrillWebHookEventType.Unsub,});
            var result = await Api.WebHooks.UpdateAsync(added.Id, WebhookUri, description: "An updated description", events: new[] {MandrillWebHookEventType.Unsub,});
            result.Id.Should().Be(added.Id);
        }
    }
}