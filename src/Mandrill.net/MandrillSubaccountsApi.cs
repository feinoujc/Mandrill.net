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

    internal partial class MandrillSubaccountsApi
    {
     
        public IList<MandrillSubaccountResponse> List(string q = null)
        {
            return MandrillApi.Post<MandrillSubaccountsRequest, IList<MandrillSubaccountResponse>>("subaccounts/list.json", new MandrillSubaccountsRequest()
            {
                Q = q
            });
        }

        public MandrillSubaccountResponse Add(string id, string name = null, string notes = null, int? customQuota = null)
        {
            return MandrillApi.Post<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/add.json", new MandrillSubaccountsRequest()
            {
                Id = id,
                Name = name,
                Notes = notes,
                CustomQuota = customQuota
            });
        }

        public MandrillSubaccountInfo Info(string id)
        {
            return MandrillApi.Post<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/info.json", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }

        public MandrillSubaccountResponse Update(string id, string name = null, string notes = null, int? customQuota = null)
        {
            return MandrillApi.Post<MandrillSubaccountsRequest, MandrillSubaccountResponse>("subaccounts/update.json", new MandrillSubaccountsRequest()
            {
                Id = id,
                Name = name,
                Notes = notes,
                CustomQuota = customQuota
            });
        }

        public MandrillSubaccountResponse Delete(string id)
        {
            return MandrillApi.Post<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/delete.json", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }

        public MandrillSubaccountResponse Pause(string id)
        {
            return MandrillApi.Post<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/pause.json", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }

        public MandrillSubaccountResponse Resume(string id)
        {
            return MandrillApi.Post<MandrillSubaccountsRequest, MandrillSubaccountInfo>("subaccounts/resume.json", new MandrillSubaccountsRequest()
            {
                Id = id,
            });
        }
    }
}