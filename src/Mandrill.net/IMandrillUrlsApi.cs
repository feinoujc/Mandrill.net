using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillUrlsApi
    {
        Task<IList<MandrillTrackingDomain>> TrackingDomainsAsync(CancellationToken cancellationToken = default);
        Task<MandrillTrackingDomain> AddTrackingDomainAsync(string domain, CancellationToken cancellationToken = default);
        Task<MandrillTrackingDomain> CheckTrackingDomainAsync(string domain, CancellationToken cancellationToken = default);
        Task<MandrillTrackingDomain> DeleteTrackingDomainAsync(string domain, CancellationToken cancellationToken = default);
    }
}
