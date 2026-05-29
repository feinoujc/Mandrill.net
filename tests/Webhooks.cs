using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Model;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "webhooks")]
    [Collection("webhooks")]
    public class Webhooks(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;
        protected Uri WebhookUri { get; set; } = new Uri(Environment.GetEnvironmentVariable("MANDRILL_OUTBOUND_WEBHOOK") ?? "https://httpbin.org/status/200");
        private HashSet<int> _added = new();

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual async Task DisposeAsync()
        {
            foreach (var id in _added)
            {
                await Api.WebHooks.DeleteAsync(id);
            }
        }

        [Fact]
        public async Task Can_add()
        {
            var result = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] { MandrillWebHookEventType.Unsub, });
            Assert.NotNull(result);
            _added.Add(result.Id);

            Assert.NotEmpty(result.Events);
            Assert.Equal(MandrillWebHookEventType.Unsub, result.Events[0]);
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
            Assert.True(result.Count >= 0);
        }

        [Fact]
        public async Task Can_delete()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] { MandrillWebHookEventType.Unsub, });
            _added.Add(added.Id);

            var result = await Api.WebHooks.DeleteAsync(added.Id);
            Assert.Equal(added.Id, result.Id);
            _added.Remove(result.Id);
        }

        [Fact]
        public async Task Can_get()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] { MandrillWebHookEventType.Unsub, });
            _added.Add(added.Id);

            var result = await Api.WebHooks.InfoAsync(added.Id);
            Assert.Equal(added.Id, result.Id);
        }

        [Fact]
        public async Task Can_update()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUri, "a test webhook", new[] { MandrillWebHookEventType.Unsub, });
            _added.Add(added.Id);

            var result = await Api.WebHooks.UpdateAsync(added.Id, WebhookUri, description: "An updated description", events: new[] { MandrillWebHookEventType.Unsub, });
            Assert.Equal(added.Id, result.Id);
        }
    }
}
