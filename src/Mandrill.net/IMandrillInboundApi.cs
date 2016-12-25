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
}