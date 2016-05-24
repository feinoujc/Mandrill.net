using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    internal partial class MandrillWebHooksApi : IMandrillWebHooksApi
    {
        public MandrillWebHooksApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }
        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillWebHookInfo>> ListAsync()
        {
            return MandrillApi.PostAsync<MandrillWebHookRequest, IList<MandrillWebHookInfo>>("webhooks/list.json",
                new MandrillWebHookRequest());
        }

        public Task<MandrillWebHookInfo> AddAsync(Uri url, string description = null, IList<MandrillWebHookEventType> events = null)
        {
            return MandrillApi.PostAsync<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/add.json",
                new MandrillWebHookRequest()
                {
                    Url = url,
                    Description = description,
                    Events = events
                });
        }

        public Task<MandrillWebHookInfo> InfoAsync(int id)
        {
            return MandrillApi.PostAsync<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/info.json",
                new MandrillWebHookRequest()
                {
                    Id = id
                });
        }

        public Task<MandrillWebHookInfo> UpdateAsync(int id, Uri url, string description = null, IList<MandrillWebHookEventType> events = null)
        {
            return MandrillApi.PostAsync<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/update.json",
                new MandrillWebHookRequest()
                {
                    Id = id,
                    Url = url,
                    Description = description,
                    Events = events
                });
        }

        public Task<MandrillWebHookInfo> DeleteAsync(int id)
        {
            return MandrillApi.PostAsync<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/delete.json",
                new MandrillWebHookRequest()
                {
                    Id = id,
                });
        }
    }

#if NET45
    internal partial class MandrillWebHooksApi
    {
       
        public IList<MandrillWebHookInfo> List()
        {
            return MandrillApi.Post<MandrillWebHookRequest, IList<MandrillWebHookInfo>>("webhooks/list.json",
                new MandrillWebHookRequest());
        }

        public MandrillWebHookInfo Add(Uri url, string description = null, IList<MandrillWebHookEventType> events = null)
        {
            return MandrillApi.Post<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/add.json",
                new MandrillWebHookRequest()
                {
                    Url = url,
                    Description = description,
                    Events = events
                });
        }

        public MandrillWebHookInfo Info(int id)
        {
            return MandrillApi.Post<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/info.json",
                new MandrillWebHookRequest()
                {
                    Id = id
                });
        }

        public MandrillWebHookInfo Update(int id, Uri url, string description = null, IList<MandrillWebHookEventType> events = null)
        {
            return MandrillApi.Post<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/update.json",
                new MandrillWebHookRequest()
                {
                    Id = id,
                    Url = url,
                    Description = description,
                    Events = events
                });
        }

        public MandrillWebHookInfo Delete(int id)
        {
            return MandrillApi.Post<MandrillWebHookRequest, MandrillWebHookInfo>("webhooks/delete.json",
                new MandrillWebHookRequest()
                {
                    Id = id,
                });
        }
    }
#endif
}