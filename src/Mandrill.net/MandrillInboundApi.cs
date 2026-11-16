#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
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

        public Task<IList<MandrillInboundInfo>> DomainsAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillInboundRequest, IList<MandrillInboundInfo>>("inbound/domains",
                new MandrillInboundRequest(), cancellationToken);
        }


        public Task<MandrillInboundInfo> AddDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return MandrillApi.PostAsync<MandrillInboundRequest, MandrillInboundInfo>("inbound/add-domain",
                new MandrillInboundRequest() { Domain = domain }, cancellationToken);
        }

        public Task<MandrillInboundInfo> CheckDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return MandrillApi.PostAsync<MandrillInboundRequest, MandrillInboundInfo>("inbound/check-domain",
                new MandrillInboundRequest() { Domain = domain }, cancellationToken);
        }

        public Task<MandrillInboundInfo> DeleteDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return MandrillApi.PostAsync<MandrillInboundRequest, MandrillInboundInfo>("inbound/delete-domain",
                new MandrillInboundRequest() { Domain = domain }, cancellationToken);
        }

        public Task<IList<MandrillInboundRoute>> RoutesAsync(string domain, CancellationToken cancellationToken = default)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            return MandrillApi.PostAsync<MandrillInboundRouteRequest, IList<MandrillInboundRoute>>(
                "inbound/routes",
                new MandrillInboundRouteRequest() { Domain = domain }, cancellationToken);
        }

        public Task<MandrillInboundRoute> AddRouteAsync(string domain, string pattern, Uri url, CancellationToken cancellationToken = default)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            if (url == null) throw new ArgumentNullException(nameof(url));
            return MandrillApi.PostAsync<MandrillInboundRouteRequest, MandrillInboundRoute>("inbound/add-route",
                new MandrillInboundRouteRequest()
                {
                    Domain = domain,
                    Pattern = pattern,
                    Url = url
                }, cancellationToken);
        }

        public Task<MandrillInboundRoute> UpdateRouteAsync(string id, string pattern, Uri url, CancellationToken cancellationToken = default)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            if (url == null) throw new ArgumentNullException(nameof(url));
            return MandrillApi.PostAsync<MandrillInboundRouteRequest, MandrillInboundRoute>("inbound/update-route",
                new MandrillInboundRouteRequest()
                {
                    Id = id,
                    Pattern = pattern,
                    Url = url
                }, cancellationToken);
        }

        public Task<MandrillInboundRoute> DeleteRouteAsync(string id, CancellationToken cancellationToken = default)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            return MandrillApi.PostAsync<MandrillInboundRouteRequest, MandrillInboundRoute>("inbound/delete-route",
                new MandrillInboundRouteRequest()
                {
                    Id = id,
                }, cancellationToken);
        }

        public Task<IList<MandrillInboundSendResponse>> SendRawAsync(string rawMessage, IList<string>? to = null,
            string? mailFrom = null, string? helo = null, string? clientAddress = null, CancellationToken cancellationToken = default)
        {
            if (rawMessage == null) throw new ArgumentNullException(nameof(rawMessage));
            return
                MandrillApi.PostAsync<MandrillInboundSendRawRequest, IList<MandrillInboundSendResponse>>(
                    "inbound/send-raw",
                    new MandrillInboundSendRawRequest()
                    {
                        RawMessage = rawMessage,
                        To = to,
                        MailFrom = mailFrom,
                        Helo = helo,
                        ClientAddress = clientAddress
                    }, cancellationToken);
        }
    }
}
