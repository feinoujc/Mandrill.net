#nullable enable
using System.Collections.Generic;
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

        public Task<IList<MandrillSubaccountResponse>> ListAsync(string? q = null)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, IList<MandrillSubaccountResponse>>("subaccounts/list", new MandrillSubaccountsRequest()
            {
                Q = q
            });
        }

        public Task<MandrillSubaccountResponse> AddAsync(string id, string? name = null, string? notes = null, int? customQuota = null)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/add", new MandrillSubaccountsRequest()
            {
                Id = id,
                Name = name,
                Notes = notes,
                CustomQuota = customQuota
            });
        }

        public Task<MandrillSubaccountInfo> InfoAsync(string id)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/info", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }

        public Task<MandrillSubaccountResponse> UpdateAsync(string id, string? name = null, string? notes = null, int? customQuota = null)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/update", new MandrillSubaccountsRequest()
            {
                Id = id,
                Name = name,
                Notes = notes,
                CustomQuota = customQuota
            });
        }

        public Task<MandrillSubaccountResponse> DeleteAsync(string id)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/delete", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }

        public Task<MandrillSubaccountResponse> PauseAsync(string id)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/pause", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }

        public Task<MandrillSubaccountResponse> ResumeAsync(string id)
        {
            return MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/resume", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }
    }
}
