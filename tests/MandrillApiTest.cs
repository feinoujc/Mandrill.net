using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Mandrill.Model;
using Newtonsoft.Json;
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
                api.HttpClient.BaseAddress.OriginalString.Should().Be("https://mandrillapp.com/api/1.0/");
                api.HttpClient.DefaultRequestHeaders.Accept.Count.Should().Be(1);
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
                api.HttpClient.BaseAddress.OriginalString.Should().Be("https://mycorp.proxy.local/");
                api.HttpClient.DefaultRequestHeaders.Accept.Count.Should().Be(2);
            }
        }

        [Fact]
        public async Task MandrillApi_ctor_non_default_client_controls_its_lifecycle()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://mycorp.proxy.local/");

            using (var api = new MandrillApi("api_key", client))
            {
                api.HttpClient.BaseAddress.OriginalString.Should().Be("https://mycorp.proxy.local/");
            }

            try
            {
                await client.GetAsync("test");
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

            await Assert.ThrowsAsync<ObjectDisposedException>(() => api.Tags.AllTimeSeriesAsync());

        }
    }
}
