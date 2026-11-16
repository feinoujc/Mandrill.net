using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillUsersApi
    {
        Task<string> PingAsync(CancellationToken cancellationToken = default);
        Task<MandrillPing2Response> Ping2Async(CancellationToken cancellationToken = default);
        Task<IList<MandrillSenderDemographics>> SendersAsync(CancellationToken cancellationToken = default);
        Task<MandrillUserInfo> InfoAsync(CancellationToken cancellationToken = default);
    }
}
