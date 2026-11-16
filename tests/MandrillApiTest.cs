using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Model;
using Xunit;

namespace Tests
{
    [Trait("Category", "api")]
    public class MandrillApiTest
    {
        [Fact]
        public void MandrillApi_ctor_non_default_client_is_configured()
        {
            var client = new HttpClient();

            using (var api = new MandrillApi("api_key", client))
            {
                Assert.Equal("https://mandrillapp.com/api/1.3/", api.HttpClient.BaseAddress.OriginalString);
                Assert.Single(api.HttpClient.DefaultRequestHeaders.Accept);
            }
        }

        [Fact]
        public void MandrillApi_ctor_non_default_client_retains_custom_settings()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://mycorp.proxy.local/");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("custom", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.1));

            using (var api = new MandrillApi("api_key", client))
            {
                Assert.Equal("https://mycorp.proxy.local/", api.HttpClient.BaseAddress.OriginalString);
                Assert.Equal(2, api.HttpClient.DefaultRequestHeaders.Accept.Count);
            }
        }

        [Fact]
        public async Task MandrillApi_ctor_non_default_client_controls_its_lifecycle()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://mycorp.proxy.local/");

            using (var api = new MandrillApi("api_key", client))
            {
                Assert.Equal("https://mycorp.proxy.local/", api.HttpClient.BaseAddress.OriginalString);
            }

            try
            {
                await client.GetAsync("test", TestContext.Current.CancellationToken);
            }
            catch (ObjectDisposedException)
            {
                Assert.Fail("Should not have been disposed");
            }
            catch (HttpRequestException)
            {
                // no op
            }
        }

        [Fact]
        public async Task MandrillApi_ctor_default_client_is_dispsed()
        {
            var api = new MandrillApi("api_key");
            api.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(() => api.Tags.AllTimeSeriesAsync(TestContext.Current.CancellationToken));

        }

        [Fact]
        public async Task MandrillApi_forwards_cancellation_token_to_http_pipeline()
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
            var handler = new BlockingHandler();
            using var api = new MandrillApi("api_key", new HttpClient(handler));

            var pending = api.Messages.InfoAsync("id", cts.Token);
            await handler.RequestReceived.Task.WaitAsync(TestContext.Current.CancellationToken);
            Assert.True(handler.ObservedToken.CanBeCanceled);

            cts.Cancel();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => pending);
        }

        [Fact]
        public async Task MandrillApi_already_cancelled_token_throws()
        {
            var handler = new BlockingHandler();
            using var api = new MandrillApi("api_key", new HttpClient(handler));

            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                () => api.Tags.ListAsync(new CancellationToken(canceled: true)));
            if (handler.RequestReceived.Task.IsCompleted)
                Assert.True(handler.ObservedToken.IsCancellationRequested);
        }

        private sealed class BlockingHandler : HttpMessageHandler
        {
            public TaskCompletionSource RequestReceived { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
            public CancellationToken ObservedToken { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                ObservedToken = cancellationToken;
                RequestReceived.TrySetResult();
                await Task.Delay(Timeout.Infinite, cancellationToken);
                throw new InvalidOperationException("unreachable");
            }
        }
    }
}
