using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Mandrill
{
    internal partial class MandrillSendersApi : IMandrillSendersApi
    {
        public MandrillSendersApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillSenderInfo>> ListAsync()
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderInfo>>("senders/list",
                new MandrillSenderRequest());
        }

        public Task<IList<MandrillSenderDomain>> DomainsAsync()
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderDomain>>("senders/domains",
                new MandrillSenderRequest());
        }

        public Task<MandrillSenderDomain> AddDomainAsync(string domain)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/add-domain",
                new MandrillSenderRequest
                {
                    Domain = domain
                });
        }

        public Task<MandrillSenderDomain> CheckDomainAsync(string domain)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/check-domain",
                new MandrillSenderRequest
                {
                    Domain = domain
                });
        }

        public Task<MandrillSenderVerifyDomain> VerifyDomainAsync(string domain, string mailbox)
        {
            return MandrillApi.PostAsync<MandrillSenderVerifyDomainRequest, MandrillSenderVerifyDomain>("senders/verify-domain",
                new MandrillSenderVerifyDomainRequest
                {
                    Domain = domain,
                    Mailbox = mailbox
                });
        }

        public Task<MandrillSenderInfo> InfoAsync(string address)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderInfo>("senders/info",
                new MandrillSenderRequest
                {
                    Address = address
                });
        }

        public Task<IList<MandrillSenderTimeSeries>> TimeSeriesAsync(string address)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderTimeSeries>>("senders/time-series",
                new MandrillSenderRequest
                {
                    Address = address
                });
        }

        public Task<MandrillSenderDomain> DeleteDomainAsync(string domain)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/delete-domain",
                new MandrillSenderRequest
                {
                    Domain = domain
                });
        }
    }
}
