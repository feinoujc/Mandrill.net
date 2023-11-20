using System;
using Mandrill;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Mandrill.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tests
{


    [Trait("Category", "integration")]
    public abstract class IntegrationTest : IDisposable
    {
        protected ITestOutputHelper Output { get; }
        private ServiceProvider _services;


        protected IntegrationTest(ITestOutputHelper output)
        {
            Output = output;
            var registry = new ServiceCollection();
            registry.AddLogging((builder) => builder.AddXunit(this.Output));
            _services = registry.AddMandrill(options => options.ApiKey = ApiKeyLazy.Value).Services.BuildServiceProvider();
        }
        private static readonly Lazy<string> ApiKeyLazy = new Lazy<string>(() =>
        {
            var apiKey = Environment.GetEnvironmentVariable("MANDRILL_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                Assert.Fail("You must set the user environment variable MANDRILL_API_KEY in order to run these tests. " +
                                            "Go to https://mandrillapp.com/ to obtain an api key.");
            }
            return apiKey;
        });

        protected string ApiKey => ApiKeyLazy.Value;

        protected IMandrillApi Api => _services.GetRequiredService<IMandrillApi>();

        public virtual void Dispose()
        {

        }
    }
}
