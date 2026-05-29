#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mandrill
{
    internal partial class MandrillIpsApi : IMandrillIpsApi
    {
        public MandrillIpsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillIpInfo>> ListAsync()
        {
            return MandrillApi.PostAsync<MandrillIpRequest, IList<MandrillIpInfo>>("ips/list",
                new MandrillIpRequest());
        }

        public Task<MandrillIpInfo> InfoAsync(string ip)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, MandrillIpInfo>("ips/info",
                new MandrillIpRequest { Ip = ip });
        }

        public Task<MandrillIpProvisionResponse> ProvisionAsync(bool? warmup = null, string? pool = null)
        {
            return MandrillApi.PostAsync<MandrillIpProvisionRequest, MandrillIpProvisionResponse>("ips/provision",
                new MandrillIpProvisionRequest { Warmup = warmup, Pool = pool });
        }

        public Task<MandrillIpInfo> StartWarmupAsync(string ip)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, MandrillIpInfo>("ips/start-warmup",
                new MandrillIpRequest { Ip = ip });
        }

        public Task<MandrillIpInfo> CancelWarmupAsync(string ip)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, MandrillIpInfo>("ips/cancel-warmup",
                new MandrillIpRequest { Ip = ip });
        }

        public Task<MandrillIpInfo> SetPoolAsync(string ip, string pool, bool? createPool = null)
        {
            return MandrillApi.PostAsync<MandrillIpSetPoolRequest, MandrillIpInfo>("ips/set-pool",
                new MandrillIpSetPoolRequest { Ip = ip, Pool = pool, CreatePool = createPool });
        }

        public Task<MandrillIpDeleteResponse> DeleteAsync(string ip)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, MandrillIpDeleteResponse>("ips/delete",
                new MandrillIpRequest { Ip = ip });
        }

        public Task<IList<MandrillIpPool>> ListPoolsAsync()
        {
            return MandrillApi.PostAsync<MandrillIpRequest, IList<MandrillIpPool>>("ips/list-pools",
                new MandrillIpRequest());
        }

        public Task<MandrillIpPool> PoolInfoAsync(string pool)
        {
            return MandrillApi.PostAsync<MandrillIpPoolRequest, MandrillIpPool>("ips/pool-info",
                new MandrillIpPoolRequest { Pool = pool });
        }

        public Task<MandrillIpPool> CreatePoolAsync(string pool)
        {
            return MandrillApi.PostAsync<MandrillIpPoolRequest, MandrillIpPool>("ips/create-pool",
                new MandrillIpPoolRequest { Pool = pool });
        }

        public Task<MandrillIpDeletePoolResponse> DeletePoolAsync(string pool)
        {
            return MandrillApi.PostAsync<MandrillIpPoolRequest, MandrillIpDeletePoolResponse>("ips/delete-pool",
                new MandrillIpPoolRequest { Pool = pool });
        }

        public Task<MandrillIpCheckCustomDnsResponse> CheckCustomDnsAsync(string ip, string domain)
        {
            return MandrillApi.PostAsync<MandrillIpSetCustomDnsRequest, MandrillIpCheckCustomDnsResponse>("ips/check-custom-dns",
                new MandrillIpSetCustomDnsRequest { Ip = ip, Domain = domain });
        }

        public Task<MandrillIpInfo> SetCustomDnsAsync(string ip, string domain)
        {
            return MandrillApi.PostAsync<MandrillIpSetCustomDnsRequest, MandrillIpInfo>("ips/set-custom-dns",
                new MandrillIpSetCustomDnsRequest { Ip = ip, Domain = domain });
        }
    }
}
