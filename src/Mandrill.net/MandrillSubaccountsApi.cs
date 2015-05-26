using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    internal class MandrillSubaccountsApi : IMandrillSubaccountsApi
    {
        public MandrillApi MandrillApi { get; set; }

        public MandrillSubaccountsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public async Task<IList<MandrillSubaccountResponse>> ListAsync(string q = null)
        {
            return await MandrillApi.PostAsync<MandrillSubaccountsRequest, IList<MandrillSubaccountResponse>>("subaccounts/list.json", new MandrillSubaccountsRequest()
            {
                Q = q
            });
        }

        public async Task<MandrillSubaccountResponse> AddAsync(string id, string name = null, string notes = null, int? customQuota = null)
        {
            return await MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/add.json", new MandrillSubaccountsRequest()
            {
                Id = id,
                Name = name,
                Notes = notes,
                CustomQuota = customQuota
            });
        }

        public async Task<MandrillSubaccountInfo> InfoAsync(string id)
        {
            return await MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/info.json", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }

        public async Task<MandrillSubaccountResponse> UpdateAsync(string id, string name = null, string notes = null, int? customQuota = null)
        {
            return await MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/update.json", new MandrillSubaccountsRequest()
            {
                Id = id,
                Name = name,
                Notes = notes,
                CustomQuota = customQuota
            });
        }

        public async Task<MandrillSubaccountResponse> DeleteAsync(string id)
        {
            return await MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/delete.json", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }

        public async Task<MandrillSubaccountResponse> PauseAsync(string id)
        {
            return await MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/pause.json", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }

        public async Task<MandrillSubaccountResponse> ResumeAsync(string id)
        {
            return await MandrillApi.PostAsync<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/resume.json", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }
    }
}