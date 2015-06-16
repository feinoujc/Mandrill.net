using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    internal class MandrillWebHooksApi : IMandrillWebHooksApi
    {
        public MandrillApi MandrillApi { get; set; }

        public MandrillWebHooksApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }


        public async Task<IList<MandrillWebHookInfo>> ListAsync()
        {
            return await MandrillApi.PostAsync<MandrillWebHookRequest, IList<MandrillWebHookInfo>>("webhooks/list.json",
                new MandrillWebHookRequest());
        }

        public async Task<MandrillWebHookInfo> AddAsync(Uri url, string description = null, IList<MandrillWebHookEventType> events = null)
        {
            return await MandrillApi.PostAsync<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/add.json",
                new MandrillWebHookRequest()
                {
                    Url = url,
                    Description = description,
                    Events = events
                });
        }

        public async Task<MandrillWebHookInfo> InfoAsync(int id)
        {
            return await MandrillApi.PostAsync<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/info.json",
                new MandrillWebHookRequest()
                {
                    Id = id
                });
        }

        public async Task<MandrillWebHookInfo> UpdateAsync(int id, Uri url, string description = null, IList<MandrillWebHookEventType> events = null)
        {
            return await MandrillApi.PostAsync<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/update.json",
                new MandrillWebHookRequest()
                {
                    Id = id,
                    Url = url,
                    Description = description,
                    Events = events
                });
        }

        public async Task<MandrillWebHookInfo> DeleteAsync(int id)
        {
            return await MandrillApi.PostAsync<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/delete.json",
                new MandrillWebHookRequest()
                {
                    Id = id,
                });
        }
    }
}