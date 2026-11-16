#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillInboundApi
    {
        Task<IList<MandrillInboundInfo>> DomainsAsync(CancellationToken cancellationToken = default);
        Task<MandrillInboundInfo> AddDomainAsync(string domain, CancellationToken cancellationToken = default);
        Task<MandrillInboundInfo> CheckDomainAsync(string domain, CancellationToken cancellationToken = default);
        Task<MandrillInboundInfo> DeleteDomainAsync(string domain, CancellationToken cancellationToken = default);
        Task<IList<MandrillInboundRoute>> RoutesAsync(string domain, CancellationToken cancellationToken = default);
        Task<MandrillInboundRoute> AddRouteAsync(string domain, string pattern, Uri url, CancellationToken cancellationToken = default);
        Task<MandrillInboundRoute> UpdateRouteAsync(string id, string pattern, Uri url, CancellationToken cancellationToken = default);
        Task<MandrillInboundRoute> DeleteRouteAsync(string id, CancellationToken cancellationToken = default);

        Task<IList<MandrillInboundSendResponse>> SendRawAsync(string rawMessage, IList<string>? to = null,
            string? mailFrom = null,
            string? helo = null, string? clientAddress = null, CancellationToken cancellationToken = default);
    }
}
