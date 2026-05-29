using Mandrill.Model;
using System.Collections.Generic;
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

        public Task<IList<MandrillTrackingDomain>> TrackingDomainsAsync()
        {
            return MandrillApi.PostAsync<MandrillTrackingDomainRequest, IList<MandrillTrackingDomain>>("urls/tracking-domains.json",
                new MandrillTrackingDomainRequest());
        }

        public Task<MandrillTrackingDomain> AddTrackingDomainAsync(string domain)
        {
            return MandrillApi.PostAsync<MandrillTrackingDomainRequest, MandrillTrackingDomain>("urls/add-tracking-domain.json",
                new MandrillTrackingDomainRequest { Domain = domain });
        }

        public Task<MandrillTrackingDomain> CheckTrackingDomainAsync(string domain)
        {
            return MandrillApi.PostAsync<MandrillTrackingDomainRequest, MandrillTrackingDomain>("urls/check-tracking-domain.json",
                new MandrillTrackingDomainRequest { Domain = domain });
        }

        public Task<MandrillTrackingDomain> DeleteTrackingDomainAsync(string domain)
        {
            return MandrillApi.PostAsync<MandrillTrackingDomainRequest, MandrillTrackingDomain>("urls/delete-tracking-domain.json",
                new MandrillTrackingDomainRequest { Domain = domain });
        }
    }
}
