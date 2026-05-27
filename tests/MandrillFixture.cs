using System.Threading.Tasks;
using Mandrill;
using Mandrill.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests
{
    public sealed class MandrillFixture : IAsyncLifetime
    {
        private ServiceProvider _services;

        public IMandrillApi Api => _services.GetRequiredService<IMandrillApi>();
        public string ApiKey { get; private set; }

        public Task InitializeAsync()
        {
            ApiKey = ResolveApiKey();
            var services = new ServiceCollection();
            services.AddLogging(b => b.SetMinimumLevel(LogLevel.Warning));
            services.AddMandrill(options => options.ApiKey = ApiKey);
            _services = services.BuildServiceProvider();
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            if (_services != null)
                await _services.DisposeAsync();
        }

        private static string ResolveApiKey()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<MandrillFixture>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            var apiKey = config["Mandrill:ApiKey"] ?? config["MANDRILL_API_KEY"];

            if (string.IsNullOrEmpty(apiKey))
            {
                Assert.Fail(
                    "Mandrill API key not configured. " +
                    "Set 'Mandrill:ApiKey' as a user secret or 'MANDRILL_API_KEY' as an environment variable.");
            }

            return apiKey;
        }
    }
}
