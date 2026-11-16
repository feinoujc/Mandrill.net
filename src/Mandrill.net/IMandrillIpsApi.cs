#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillIpsApi
    {
        Task<IList<MandrillIpInfo>> ListAsync(CancellationToken cancellationToken = default);
        Task<MandrillIpInfo> InfoAsync(string ip, CancellationToken cancellationToken = default);
        Task<MandrillIpProvisionResponse> ProvisionAsync(bool? warmup = null, string? pool = null, CancellationToken cancellationToken = default);
        Task<MandrillIpInfo> StartWarmupAsync(string ip, CancellationToken cancellationToken = default);
        Task<MandrillIpInfo> CancelWarmupAsync(string ip, CancellationToken cancellationToken = default);
        Task<MandrillIpInfo> SetPoolAsync(string ip, string pool, bool? createPool = null, CancellationToken cancellationToken = default);
        Task<MandrillIpDeleteResponse> DeleteAsync(string ip, CancellationToken cancellationToken = default);
        Task<IList<MandrillIpPool>> ListPoolsAsync(CancellationToken cancellationToken = default);
        Task<MandrillIpPool> PoolInfoAsync(string pool, CancellationToken cancellationToken = default);
        Task<MandrillIpPool> CreatePoolAsync(string pool, CancellationToken cancellationToken = default);
        Task<MandrillIpDeletePoolResponse> DeletePoolAsync(string pool, CancellationToken cancellationToken = default);
        Task<MandrillIpCheckCustomDnsResponse> CheckCustomDnsAsync(string ip, string domain, CancellationToken cancellationToken = default);
        Task<MandrillIpInfo> SetCustomDnsAsync(string ip, string domain, CancellationToken cancellationToken = default);
    }
}
