using System;
using System.Threading.Tasks;
using Mandrill;
using Xunit;

namespace Tests
{


    [Trait("Category", "integration")]
    public abstract class IntegrationTest : IDisposable
    {
        private static readonly Lazy<string> ApiKeyLazy = new Lazy<string>(() =>
        {
            var apiKey = Environment.GetEnvironmentVariable("MANDRILL_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                Assert.True(false, "You must set the user environment variable MANDRILL_API_KEY in order to run these tests. " +
                                            "Go to https://mandrillapp.com/ to obtain an api key.");
            }
            return apiKey;
        });

        private static readonly Lazy<MandrillApi> LazyApi = new Lazy<MandrillApi>(() => new MandrillApi(ApiKeyLazy.Value));

        protected string ApiKey => ApiKeyLazy.Value;

        protected MandrillApi Api => LazyApi.Value;

        public virtual void Dispose()
        {

        }
    }
}
