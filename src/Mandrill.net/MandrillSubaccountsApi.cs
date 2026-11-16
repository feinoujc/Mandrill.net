#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    internal partial class MandrillSubaccountsApi : IMandrillSubaccountsApi
    {
        public MandrillSubaccountsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillSubaccountResponse>> ListAsync(string? q = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, IList<MandrillSubaccountResponse>>("subaccounts/list", new MandrillSubaccountsRequest()
            {
                Q = q
            }, cancellationToken);
        }

        public Task<MandrillSubaccountResponse> AddAsync(string id, string? name = null, string? notes = null, int? customQuota = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/add", new MandrillSubaccountsRequest()
            {
                Id = id,
                Name = name,
                Notes = notes,
                CustomQuota = customQuota
            }, cancellationToken);
        }

        public Task<MandrillSubaccountInfo> InfoAsync(string id, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/info", new MandrillSubaccountsRequest()
            {
                Id = id,
            }, cancellationToken);
        }

        public Task<MandrillSubaccountResponse> UpdateAsync(string id, string? name = null, string? notes = null, int? customQuota = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/update", new MandrillSubaccountsRequest()
            {
                Id = id,
                Name = name,
                Notes = notes,
                CustomQuota = customQuota
            }, cancellationToken);
        }

        public Task<MandrillSubaccountResponse> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/delete", new MandrillSubaccountsRequest()
            {
                Id = id,
            }, cancellationToken);
        }

        public Task<MandrillSubaccountResponse> PauseAsync(string id, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/pause", new MandrillSubaccountsRequest()
            {
                Id = id,
            }, cancellationToken);
        }

        public Task<MandrillSubaccountResponse> ResumeAsync(string id, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/resume", new MandrillSubaccountsRequest()
            {
                Id = id,
            }, cancellationToken);
        }
    }
}
