#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill;

public partial interface IMandrillUsersApi
{
    Task<string> PingAsync(CancellationToken cancellationToken = default);

    Task<MandrillUserPing2Response> Ping2Async(CancellationToken cancellationToken = default);
}
