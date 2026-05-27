#nullable enable
#pragma warning disable CS8618
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill;

internal partial class MandrillUsersApi
{
    public async Task<string> PingAsync(CancellationToken cancellationToken = default)
    {
        return await MandrillApi.PostAsync<UsersPingRequest, string>("users/ping.json", new UsersPingRequest(), cancellationToken);
    }

    public Task<MandrillUserPing2Response> Ping2Async(CancellationToken cancellationToken = default)
    {
        return MandrillApi.PostAsync<UsersPing2Request, MandrillUserPing2Response>("users/ping2.json", new UsersPing2Request(), cancellationToken);
    }

    internal class UsersPingRequest : MandrillRequestBase
    {
    }

    internal class UsersPing2Request : MandrillRequestBase
    {
    }
}
