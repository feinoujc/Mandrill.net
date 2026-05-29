#nullable enable
using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mandrill
{
    internal partial class MandrillExportsApi : IMandrillExportsApi
    {

        public MandrillExportsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }
        public MandrillApi MandrillApi { get; set; }

        public Task<IList<MandrillExportInfo>> ListAsync()
        {
            return MandrillApi.PostAsync<MandrillExportRequest, IList<MandrillExportInfo>>("exports/list",
                new MandrillExportRequest());
        }

        public Task<MandrillExportInfo> InfoAsync(string id)
        {
            return MandrillApi.PostAsync<MandrillExportRequest, MandrillExportInfo>("exports/info",
                new MandrillExportRequest
                {
                    Id = id
                });
        }

        public Task<MandrillExportInfo> RejectsAsync(string notifyEmail)
        {
            return MandrillApi.PostAsync<MandrillExportRequest, MandrillExportInfo>("exports/rejects",
                new MandrillExportRequest
                {
                    NotifyEmail = notifyEmail
                });
        }

        public Task<MandrillExportInfo> AllowlistAsync(string? notifyEmail = null)
        {
            return MandrillApi.PostAsync<MandrillExportRequest, MandrillExportInfo>("exports/allowlist",
                new MandrillExportRequest
                {
                    NotifyEmail = notifyEmail
                });
        }

        public Task<MandrillExportInfo> WhitelistAsync(string notifyEmail)
        {
            return MandrillApi.PostAsync<MandrillExportRequest, MandrillExportInfo>("exports/whitelist",
                new MandrillExportRequest
                {
                    NotifyEmail = notifyEmail
                });
        }

        public Task<MandrillExportInfo> ActivityAsync(string notifyEmail,
            DateOnly? dateFrom = null,
            DateOnly? dateTo = null,
            IList<string>? tags = null,
            IList<string>? senders = null,
            IList<string>? states = null,
            IList<string>? apiKeys = null)
        {
            return MandrillApi.PostAsync<MandrillExportRequest, MandrillExportInfo>("exports/activity",
                new MandrillExportRequest
                {
                    NotifyEmail = notifyEmail,
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                    Tags = tags?.ToList(),
                    Senders = senders?.ToList(),
                    States = states?.ToList(),
                    ApiKeys = apiKeys?.ToList()
                });
        }
    }
}
