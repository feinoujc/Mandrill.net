using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillUrlsApi
    {
        Task<IList<MandrillTrackingDomain>> TrackingDomainsAsync();
        Task<MandrillTrackingDomain> AddTrackingDomainAsync(string domain);
        Task<MandrillTrackingDomain> CheckTrackingDomainAsync(string domain);
        Task<MandrillTrackingDomain> DeleteTrackingDomainAsync(string domain);
    }
}
