using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mandrill.Extensions.DependencyInjection;
public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Adds the <see cref="System.Net.Http.IHttpClientFactory"/> with <see cref="MandrllApi"/> and related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configureOptions">A delegate that is used to configure a <see cref="MandrillClientOptions"/>.</param>
    /// <returns>An <see cref="T:Microsoft.Extensions.DependencyInjection.IHttpClientBuilder" /> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddMandrill(this IServiceCollection services, Action<IServiceProvider, MandrillClientOptions> configureOptions)
    {
        services.AddOptions<MandrillClientOptions>().Configure<IServiceProvider>((options, resolver) => configureOptions(resolver, options))
            .PostConfigure(options =>
            {
                // validation
                if (string.IsNullOrWhiteSpace(options.ApiKey))
                {
                    throw new ArgumentNullException(nameof(options.ApiKey));
                }
            });

        services.TryAddTransient<MandrillApi>(resolver => resolver.GetRequiredService<InjectableMandrillClient>());
        services.AddMandrillService(api => api.Allowlists);
        services.AddMandrillService(api => api.Exports);
        services.AddMandrillService(api => api.Inbound);
        services.AddMandrillService(api => api.Messages);
        services.AddMandrillService(api => api.Rejects);
        services.AddMandrillService(api => api.Senders);
        services.AddMandrillService(api => api.Subaccounts);
        services.AddMandrillService(api => api.Tags);
        services.AddMandrillService(api => api.Templates);
        services.AddMandrillService(api => api.Users);
        services.AddMandrillService(api => api.WebHooks);

        return services.AddHttpClient<InjectableMandrillClient>();
    }
    /// <summary>
    /// Adds the <see cref="System.Net.Http.IHttpClientFactory"/> with <see cref="MandrillApi"/> and related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configureOptions">A delegate that is used to configure a <see cref="MandrillClientOptions"/>.</param>
    /// <returns>An <see cref="T:Microsoft.Extensions.DependencyInjection.IHttpClientBuilder" /> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddMandrill(this IServiceCollection services, Action<MandrillClientOptions> configureOptions)
    {
        return services.AddMandrill((_, options) => configureOptions(options));
    }

    internal static void AddMandrillService<T>(this IServiceCollection services, Func<MandrillApi, T> accessor) where T : class
    {
        services.TryAddTransient<T>(resolver => accessor(resolver.GetRequiredService<InjectableMandrillClient>()));
    }
}
