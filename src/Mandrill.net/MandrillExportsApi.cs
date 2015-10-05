using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mandrill
{
    internal partial class MandrillExportsApi : IMandrillExportsApi
    {
        private const string ActivityDateFormat = "yyyy-MM-dd";

        public MandrillExportsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }
        public MandrillApi MandrillApi { get; set; }

        public async Task<IList<MandrillExportInfo>> ListAsync()
        {
            return await MandrillApi.PostAsync<MandrillExportRequest, IList<MandrillExportInfo>>("exports/list.json",
                new MandrillExportRequest());
        }

        public async Task<MandrillExportInfo> InfoAsync(string id)
        {
            return await MandrillApi.PostAsync<MandrillExportRequest, MandrillExportInfo>("exports/info.json",
                new MandrillExportRequest
                {
                    Id = id
                });
        }

        public async Task<MandrillExportInfo> RejectsAsync(string notifyEmail)
        {
            return await MandrillApi.PostAsync<MandrillExportRequest, MandrillExportInfo>("exports/rejects.json",
                new MandrillExportRequest
                {
                    NotifyEmail = notifyEmail
                });
        }

        public async Task<MandrillExportInfo> WhitelistAsync(string notifyEmail)
        {
            return await MandrillApi.PostAsync<MandrillExportRequest, MandrillExportInfo>("exports/whitelist.json",
                new MandrillExportRequest
                {
                    NotifyEmail = notifyEmail
                });
        }

        public async Task<MandrillExportInfo> ActivityAsync(string notifyEmail,
            DateTime? dateFrom = null, 
            DateTime? dateTo = null,
            IList<string> tags = null, 
            IList<string> senders = null,
            IList<string> states = null,
            IList<string> apiKeys = null)
        {
            return await MandrillApi.PostAsync<MandrillExportRequest, MandrillExportInfo>("exports/activity",
                new MandrillExportRequest
                {   
                    NotifyEmail = notifyEmail,
                    DateFrom = dateFrom?.ToString(ActivityDateFormat),
                    DateTo = dateTo?.ToString(ActivityDateFormat),
                    Tags = tags?.ToList(),
                    Senders = senders?.ToList(),
                    States = states?.ToList(),
                    ApiKeys = apiKeys?.ToList()
                });
        }
    }

    internal partial class MandrillExportsApi
    {
        public IList<MandrillExportInfo> List()
        {
            return MandrillApi.Post<MandrillExportRequest, IList<MandrillExportInfo>>("exports/list.json",
                new MandrillExportRequest());
        }

        public MandrillExportInfo Info(string id)
        {
            return MandrillApi.Post<MandrillExportRequest, MandrillExportInfo>("exports/info.json",
                new MandrillExportRequest
                {
                    Id = id
                });
        }

        public MandrillExportInfo Rejects(string notifyEmail)
        {
            return MandrillApi.Post<MandrillExportRequest, MandrillExportInfo>("exports/rejects.json",
                new MandrillExportRequest
                {
                    NotifyEmail = notifyEmail
                });
        }

        public MandrillExportInfo Whitelist(string notifyEmail)
        {
            return MandrillApi.Post<MandrillExportRequest, MandrillExportInfo>("exports/whitelist.json",
                new MandrillExportRequest
                {
                    NotifyEmail = notifyEmail
                });
        }

        public MandrillExportInfo Activity(string notifyEmail,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            IList<string> tags = null,
            IList<string> senders = null,
            IList<string> states = null,
            IList<string> apiKeys = null)
        {
            return MandrillApi.Post<MandrillExportRequest, MandrillExportInfo>("exports/activity",
                new MandrillExportRequest
                {
                    NotifyEmail = notifyEmail,
                    DateFrom = dateFrom?.ToString(ActivityDateFormat),
                    DateTo = dateTo?.ToString(ActivityDateFormat),
                    Tags = tags?.ToList(),
                    Senders = senders?.ToList(),
                    States = states?.ToList(),
                    ApiKeys = apiKeys?.ToList()
                });
        }
    }
}
