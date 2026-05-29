#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillIpsApi
    {
        Task<IList<MandrillIpInfo>> ListAsync();
        Task<MandrillIpInfo> InfoAsync(string ip);
        Task<MandrillIpProvisionResponse> ProvisionAsync(bool? warmup = null, string? pool = null);
        Task<MandrillIpInfo> StartWarmupAsync(string ip);
        Task<MandrillIpInfo> CancelWarmupAsync(string ip);
        Task<MandrillIpInfo> SetPoolAsync(string ip, string pool, bool? createPool = null);
        Task<MandrillIpDeleteResponse> DeleteAsync(string ip);
        Task<IList<MandrillIpPool>> ListPoolsAsync();
        Task<MandrillIpPool> PoolInfoAsync(string pool);
        Task<MandrillIpPool> CreatePoolAsync(string pool);
        Task<MandrillIpDeletePoolResponse> DeletePoolAsync(string pool);
        Task<MandrillIpCheckCustomDnsResponse> CheckCustomDnsAsync(string ip, string domain);
        Task<MandrillIpInfo> SetCustomDnsAsync(string ip, string domain);
    }
}
