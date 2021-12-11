using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mandrill.Extensions.DependencyInjection;
using Xunit;
using Mandrill;

namespace Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void TestAddMandrillWithoutApiKey()
        {
            // Arrange
            var services = new ServiceCollection().AddMandrill(options => { }).Services.BuildServiceProvider();

            // Act && Assert
            Assert.Throws<ArgumentNullException>(() => services.GetRequiredService<IMandrillApi>());
        }

        [Fact]
        public void TestAddMandrillReturnHttpClientBuilder()
        {
            // Arrange
            var collection = new ServiceCollection();

            // Act
            var builder = collection.AddMandrill(options => options.ApiKey = "FAKE_API_KEY");

            // Assert
            Assert.NotNull(builder);
            Assert.IsAssignableFrom<IHttpClientBuilder>(builder);
        }

        [Fact]
        public void TestAddMandrillRegisteredWithTransientLifeTime()
        {
            // Arrange
            var collection = new ServiceCollection();

            // Act
            var builder = collection.AddMandrill(options => options.ApiKey = "FAKE_API_KEY");

            // Assert
            var serviceDescriptor = collection.FirstOrDefault(x => x.ServiceType == typeof(IMandrillApi));
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
        }

        [Fact]
        public void TestAddMandrillCanResolveMandrillClientOptions()
        {
            // Arrange
            var services = new ServiceCollection().AddMandrill(options => options.ApiKey = "FAKE_API_KEY").Services.BuildServiceProvider();

            // Act
            var MandrillClientOptions = services.GetService<IOptions<MandrillClientOptions>>();

            // Assert
            Assert.NotNull(MandrillClientOptions);
        }

        [Fact]
        public void TestAddMandrillCanResolveMandrillClient()
        {
            // Arrange
            var services = new ServiceCollection().AddMandrill(options => options.ApiKey = "FAKE_API_KEY").Services.BuildServiceProvider();

            // Act
            var Mandrill = services.GetService<IMandrillApi>();

            // Assert
            Assert.NotNull(Mandrill);
        }

        [Theory]
        [InlineData(typeof(IMandrillAllowlistsApi))]
        [InlineData(typeof(IMandrillExportsApi))]
        [InlineData(typeof(IMandrillInboundApi))]
        [InlineData(typeof(IMandrillMessagesApi))]
        [InlineData(typeof(IMandrillRejectsApi))]
        [InlineData(typeof(IMandrillSendersApi))]
        [InlineData(typeof(IMandrillSubaccountsApi))]
        [InlineData(typeof(IMandrillTagsApi))]
        [InlineData(typeof(IMandrillTemplatesApi))]
        [InlineData(typeof(IMandrillUsersApi))]
        [InlineData(typeof(IMandrillWebHooksApi))]
        public void TestAddMandrillCanResolveMandrillService(Type service)
        {
            // Arrange
            var services = new ServiceCollection().AddMandrill(options => options.ApiKey = "FAKE_API_KEY").Services.BuildServiceProvider();

            // Act
            var instance = services.GetService(service);

            // Assert
            Assert.NotNull(instance);
        }
    }
}
