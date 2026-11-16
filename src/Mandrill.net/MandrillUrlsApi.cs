using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    internal partial class MandrillUrlsApi : IMandrillUrlsApi
    {
        public MandrillUrlsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillTrackingDomain>> TrackingDomainsAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTrackingDomainRequest, IList<MandrillTrackingDomain>>("urls/tracking-domains",
                new MandrillTrackingDomainRequest(), cancellationToken);
        }

        public Task<MandrillTrackingDomain> AddTrackingDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTrackingDomainRequest, MandrillTrackingDomain>("urls/add-tracking-domain",
                new MandrillTrackingDomainRequest { Domain = domain }, cancellationToken);
        }

        public Task<MandrillTrackingDomain> CheckTrackingDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTrackingDomainRequest, MandrillTrackingDomain>("urls/check-tracking-domain",
                new MandrillTrackingDomainRequest { Domain = domain }, cancellationToken);
        }

        public Task<MandrillTrackingDomain> DeleteTrackingDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTrackingDomainRequest, MandrillTrackingDomain>("urls/delete-tracking-domain",
                new MandrillTrackingDomainRequest { Domain = domain }, cancellationToken);
        }
    }
}
