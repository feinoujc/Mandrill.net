#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
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

        public Task<IList<MandrillIpInfo>> ListAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, IList<MandrillIpInfo>>("ips/list",
                new MandrillIpRequest(), cancellationToken);
        }

        public Task<MandrillIpInfo> InfoAsync(string ip, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, MandrillIpInfo>("ips/info",
                new MandrillIpRequest { Ip = ip }, cancellationToken);
        }

        public Task<MandrillIpProvisionResponse> ProvisionAsync(bool? warmup = null, string? pool = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpProvisionRequest, MandrillIpProvisionResponse>("ips/provision",
                new MandrillIpProvisionRequest { Warmup = warmup, Pool = pool }, cancellationToken);
        }

        public Task<MandrillIpInfo> StartWarmupAsync(string ip, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, MandrillIpInfo>("ips/start-warmup",
                new MandrillIpRequest { Ip = ip }, cancellationToken);
        }

        public Task<MandrillIpInfo> CancelWarmupAsync(string ip, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, MandrillIpInfo>("ips/cancel-warmup",
                new MandrillIpRequest { Ip = ip }, cancellationToken);
        }

        public Task<MandrillIpInfo> SetPoolAsync(string ip, string pool, bool? createPool = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpSetPoolRequest, MandrillIpInfo>("ips/set-pool",
                new MandrillIpSetPoolRequest { Ip = ip, Pool = pool, CreatePool = createPool }, cancellationToken);
        }

        public Task<MandrillIpDeleteResponse> DeleteAsync(string ip, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, MandrillIpDeleteResponse>("ips/delete",
                new MandrillIpRequest { Ip = ip }, cancellationToken);
        }

        public Task<IList<MandrillIpPool>> ListPoolsAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpRequest, IList<MandrillIpPool>>("ips/list-pools",
                new MandrillIpRequest(), cancellationToken);
        }

        public Task<MandrillIpPool> PoolInfoAsync(string pool, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpPoolRequest, MandrillIpPool>("ips/pool-info",
                new MandrillIpPoolRequest { Pool = pool }, cancellationToken);
        }

        public Task<MandrillIpPool> CreatePoolAsync(string pool, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpPoolRequest, MandrillIpPool>("ips/create-pool",
                new MandrillIpPoolRequest { Pool = pool }, cancellationToken);
        }

        public Task<MandrillIpDeletePoolResponse> DeletePoolAsync(string pool, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpPoolRequest, MandrillIpDeletePoolResponse>("ips/delete-pool",
                new MandrillIpPoolRequest { Pool = pool }, cancellationToken);
        }

        public Task<MandrillIpCheckCustomDnsResponse> CheckCustomDnsAsync(string ip, string domain, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpSetCustomDnsRequest, MandrillIpCheckCustomDnsResponse>("ips/check-custom-dns",
                new MandrillIpSetCustomDnsRequest { Ip = ip, Domain = domain }, cancellationToken);
        }

        public Task<MandrillIpInfo> SetCustomDnsAsync(string ip, string domain, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillIpSetCustomDnsRequest, MandrillIpInfo>("ips/set-custom-dns",
                new MandrillIpSetCustomDnsRequest { Ip = ip, Domain = domain }, cancellationToken);
        }
    }
}
