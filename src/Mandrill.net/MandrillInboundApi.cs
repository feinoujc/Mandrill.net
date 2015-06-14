using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    internal class MandrillInboundApi : IMandrillInboundApi
    {
        public MandrillApi MandrillApi { get; set; }

        public MandrillInboundApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public async Task<IList<MandrillInboundInfo>> DomainsAsync()
        {
            return await MandrillApi.PostAsync<MandrillInboundRequest, IList<MandrillInboundInfo>>("inbound/domains.json",
                new MandrillInboundRequest());
        }


        public async Task<MandrillInboundInfo> AddDomainAsync(string domain)
        {
            if (domain == null) throw new ArgumentNullException("domain");
            return await MandrillApi.PostAsync<MandrillInboundRequest, MandrillInboundInfo>("inbound/add-domain.json",
                new MandrillInboundRequest() {Domain = domain});
        }

        public async Task<MandrillInboundInfo> CheckDomainAsync(string domain)
        {
            if (domain == null) throw new ArgumentNullException("domain");
            return await MandrillApi.PostAsync<MandrillInboundRequest, MandrillInboundInfo>("inbound/check-domain.json",
                new MandrillInboundRequest() {Domain = domain});
        }

        public async Task<MandrillInboundInfo> DeleteDomainAsync(string domain)
        {
            if (domain == null) throw new ArgumentNullException("domain");
            return await MandrillApi.PostAsync<MandrillInboundRequest, MandrillInboundInfo>("inbound/delete-domain.json",
                new MandrillInboundRequest() {Domain = domain});
        }

        public async Task<IList<MandrillInboundRoute>> RoutesAsync(string domain)
        {
            if (domain == null) throw new ArgumentNullException("domain");
            return await MandrillApi.PostAsync<MandrillInboundRouteRequest, IList<MandrillInboundRoute>>("inbound/routes.json",
                new MandrillInboundRouteRequest() {Domain = domain});
        }

        public async Task<MandrillInboundRoute> AddRouteAsync(string domain, string pattern, Uri url)
        {
            if (domain == null) throw new ArgumentNullException("domain");
            if (pattern == null) throw new ArgumentNullException("pattern");
            if (url == null) throw new ArgumentNullException("url");
            return await MandrillApi.PostAsync<MandrillInboundRouteRequest, MandrillInboundRoute>("inbound/add-route.json",
                new MandrillInboundRouteRequest()
                {
                    Domain = domain,
                    Pattern = pattern,
                    Url = url
                });
        }

        public async Task<MandrillInboundRoute> UpdateRouteAsync(string id, string pattern, Uri url)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (pattern == null) throw new ArgumentNullException("pattern");
            if (url == null) throw new ArgumentNullException("url");
            return await MandrillApi.PostAsync<MandrillInboundRouteRequest, MandrillInboundRoute>("inbound/update-route.json",
                new MandrillInboundRouteRequest()
                {
                    Id = id,
                    Pattern = pattern,
                    Url = url
                });
        }

        public async Task<MandrillInboundRoute> DeleteRouteAsync(string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            return await MandrillApi.PostAsync<MandrillInboundRouteRequest, MandrillInboundRoute>("inbound/delete-route.json",
                new MandrillInboundRouteRequest()
                {
                    Id = id,
                });
        }

        public async Task<IList<MandrillInboundSendResponse>> SendRawAsync(string rawMessage, IList<string> to = null, string mailFrom = null, string helo = null, string clientAddress = null)
        {
            if (rawMessage == null) throw new ArgumentNullException("rawMessage");
            return await MandrillApi.PostAsync<MandrillInboundSendRawRequest, IList<MandrillInboundSendResponse>>("inbound/send-raw.json",
                new MandrillInboundSendRawRequest()
                {
                    RawMessage = rawMessage,
                    To = to,
                    MailFrom = mailFrom,
                    Helo = helo,
                    ClientAddress = clientAddress
                });
        }
    }
}