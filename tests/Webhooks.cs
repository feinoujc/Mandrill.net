using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Mandrill.Model;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "webhooks")]
    [Collection("webhooks")]
    public class Webhooks : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        private readonly MandrillFixture _fixture;
        private readonly HashSet<int> _added = [];

        protected IMandrillApi Api => _fixture.Api;
        protected ITestOutputHelper Output { get; }
        protected string WebhookUrl { get; }

        public Webhooks(MandrillFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            Output = output;
            var configured = Environment.GetEnvironmentVariable("MANDRILL_OUTBOUND_WEBHOOK") ?? "https://httpbin.org/status/200";
            WebhookUrl = new Uri(configured).ToString();
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual async Task DisposeAsync()
        {
            foreach (var id in _added)
                await Api.WebHooks.DeleteAsync(id);
        }

        [Fact]
        public async Task Can_add()
        {
            var result = await Api.WebHooks.AddAsync(WebhookUrl, "a test webhook", new List<string> { "unsub" });
            result.Should().NotBeNull();
            _added.Add(result.Id);

            result.Events.Should().NotBeEmpty();
            result.Events[0].Should().Be(MandrillWebHookEventType.Unsub);
        }

        [Fact]
        public async Task Throws_when_bad_url()
        {
            await Assert.ThrowsAsync<MandrillException>(() => Api.WebHooks.AddAsync("http://www.invalid_url.org"));
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
            var added = await Api.WebHooks.AddAsync(WebhookUrl, "a test webhook", new List<string> { "unsub" });
            _added.Add(added.Id);

            var result = await Api.WebHooks.DeleteAsync(added.Id);
            result.Id.Should().Be(added.Id);
            _added.Remove(result.Id);
        }

        [Fact]
        public async Task Can_get()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUrl, "a test webhook", new List<string> { "unsub" });
            _added.Add(added.Id);

            var result = await Api.WebHooks.InfoAsync(added.Id);
            result.Id.Should().Be(added.Id);
        }

        [Fact]
        public async Task Can_update()
        {
            var added = await Api.WebHooks.AddAsync(WebhookUrl, "a test webhook", new List<string> { "unsub" });
            _added.Add(added.Id);

            var result = await Api.WebHooks.UpdateAsync(added.Id, WebhookUrl, description: "An updated description", events: new List<string> { "unsub" });
            result.Id.Should().Be(added.Id);
        }
    }
}
