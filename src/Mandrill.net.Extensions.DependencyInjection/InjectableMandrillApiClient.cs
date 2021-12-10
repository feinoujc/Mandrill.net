using Microsoft.Extensions.Options;
using System.Net.Http;

namespace Mandrill.Extensions.DependencyInjection;

/// <summary>
/// A wrapped MandrillApi with single constructor to inject an <see cref="HttpClient"/> whose lifetime is managed externally, e.g. by an DI container.
/// </summary>
internal class InjectableMandrillClient : MandrillApi
{
    public InjectableMandrillClient(HttpClient httpClient, IOptions<MandrillClientOptions> options)
        : base(options.Value.ApiKey, httpClient)
    {
    }
}
