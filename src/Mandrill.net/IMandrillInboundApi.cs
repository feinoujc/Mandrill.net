using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillInboundApi
    {
        Task<IList<MandrillInboundInfo>> DomainsAsync();
        Task<MandrillInboundInfo> AddDomainAsync(string domain);
        Task<MandrillInboundInfo> CheckDomainAsync(string domain);
        Task<MandrillInboundInfo> DeleteDomainAsync(string domain);
        Task<IList<MandrillInboundRoute>> RoutesAsync(string domain);
        Task<MandrillInboundRoute> AddRouteAsync(string domain, string pattern, Uri url);
        Task<MandrillInboundRoute> UpdateRouteAsync(string id, string pattern, Uri url);
        Task<MandrillInboundRoute> DeleteRouteAsync(string id);

        Task<IList<MandrillInboundSendResponse>> SendRawAsync(string rawMessage, IList<string> to = null,
            string mailFrom = null,
            string helo = null, string clientAddress = null);
    }

#if NET45
    public partial interface IMandrillInboundApi
    {
        IList<MandrillInboundInfo> Domains();
        MandrillInboundInfo AddDomain(string domain);
        MandrillInboundInfo CheckDomain(string domain);
        MandrillInboundInfo DeleteDomain(string domain);
        IList<MandrillInboundRoute> Routes(string domain);
        MandrillInboundRoute AddRoute(string domain, string pattern, Uri url);
        MandrillInboundRoute UpdateRoute(string id, string pattern, Uri url);
        MandrillInboundRoute DeleteRoute(string id);

        IList<MandrillInboundSendResponse> SendRaw(string rawMessage,
            IList<string> to = null, string mailFrom = null, string helo = null,
            string clientAddress = null);
    }
#endif
}