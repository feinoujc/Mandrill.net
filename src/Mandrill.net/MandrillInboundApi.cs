using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    internal partial class MandrillInboundApi : IMandrillInboundApi
    {
        public MandrillInboundApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillInboundInfo>> DomainsAsync()
        {
            return MandrillApi.PostAsync<MandrillInboundRequest, IList<MandrillInboundInfo>>("inbound/domains.json",
                new MandrillInboundRequest());
        }


        public Task<MandrillInboundInfo> AddDomainAsync(string domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return MandrillApi.PostAsync<MandrillInboundRequest, MandrillInboundInfo>("inbound/add-domain.json",
                new MandrillInboundRequest() {Domain = domain});
        }

        public Task<MandrillInboundInfo> CheckDomainAsync(string domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return MandrillApi.PostAsync<MandrillInboundRequest, MandrillInboundInfo>("inbound/check-domain.json",
                new MandrillInboundRequest() {Domain = domain});
        }

        public Task<MandrillInboundInfo> DeleteDomainAsync(string domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return MandrillApi.PostAsync<MandrillInboundRequest, MandrillInboundInfo>("inbound/delete-domain.json",
                new MandrillInboundRequest() {Domain = domain});
        }

        public Task<IList<MandrillInboundRoute>> RoutesAsync(string domain)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return MandrillApi.PostAsync<MandrillInboundRouteRequest, IList<MandrillInboundRoute>>(
                "inbound/routes.json",
                new MandrillInboundRouteRequest() {Domain = domain});
        }

        public Task<MandrillInboundRoute> AddRouteAsync(string domain, string pattern, Uri url)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            if (url == null) throw new ArgumentNullException(nameof(url));
            return MandrillApi.PostAsync<MandrillInboundRouteRequest, MandrillInboundRoute>("inbound/add-route.json",
                new MandrillInboundRouteRequest()
                {
                    Domain = domain,
                    Pattern = pattern,
                    Url = url
                });
        }

        public Task<MandrillInboundRoute> UpdateRouteAsync(string id, string pattern, Uri url)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            if (url == null) throw new ArgumentNullException(nameof(url));
            return MandrillApi.PostAsync<MandrillInboundRouteRequest, MandrillInboundRoute>("inbound/update-route.json",
                new MandrillInboundRouteRequest()
                {
                    Id = id,
                    Pattern = pattern,
                    Url = url
                });
        }

        public Task<MandrillInboundRoute> DeleteRouteAsync(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            return MandrillApi.PostAsync<MandrillInboundRouteRequest, MandrillInboundRoute>("inbound/delete-route.json",
                new MandrillInboundRouteRequest()
                {
                    Id = id,
                });
        }

        public Task<IList<MandrillInboundSendResponse>> SendRawAsync(string rawMessage, IList<string> to = null,
            string mailFrom = null, string helo = null, string clientAddress = null)
        {
            if (rawMessage == null) throw new ArgumentNullException(nameof(rawMessage));
            return
                MandrillApi.PostAsync<MandrillInboundSendRawRequest, IList<MandrillInboundSendResponse>>(
                    "inbound/send-raw.json",
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
