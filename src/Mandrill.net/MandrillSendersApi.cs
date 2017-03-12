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
            return MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderInfo>>("senders/list.json",
                new MandrillSenderRequest());
        }

        public Task<IList<MandrillSenderDomain>> DomainsAsync()
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderDomain>>("senders/domains.json",
                new MandrillSenderRequest());
        }

        public Task<MandrillSenderDomain> AddDomainAsync(string domain)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/add-domain.json",
                new MandrillSenderRequest
                {
                    Domain = domain
                });
        }

        public Task<MandrillSenderDomain> CheckDomainAsync(string domain)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/check-domain.json",
                new MandrillSenderRequest
                {
                    Domain = domain
                });
        }

        public Task<MandrillSenderVerifyDomain> VerifyDomainAsync(string domain, string mailbox)
        {
            return MandrillApi.PostAsync<MandrillSenderVerifyDomainRequest, MandrillSenderVerifyDomain>("senders/verify-domain.json",
                new MandrillSenderVerifyDomainRequest
                {
                    Domain = domain,
                    Mailbox = mailbox
                });
        }

        public Task<MandrillSenderInfo> InfoAsync(string address)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderInfo>("senders/info.json",
                new MandrillSenderRequest
                {
                    Address = address
                });
        }

        public Task<IList<MandrillSenderTimeSeries>> TimeSeriesAsync(string address)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderTimeSeries>>("senders/time-series.json",
                new MandrillSenderRequest
                {
                    Address = address
                });
        }
    }
}
