using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Mandrill
{
    public class MandrillSendersApi : IMandrillSendersApi
    {
        public MandrillSendersApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; set; }

        public async Task<IList<MandrillSenderInfo>> ListAsync()
        {
            return await MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderInfo>>("senders/list.json",
                new MandrillSenderRequest());
        }

        public async Task<IList<MandrillSenderDomain>> DomainsAsync()
        {
            return await MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderDomain>>("senders/domains.json",
                new MandrillSenderRequest());
        }

        public async Task<MandrillSenderDomain> AddDomainAsync(string domain)
        {
            return await MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/add-domain.json",
                new MandrillSenderRequest
                {
                    Domain = domain
                });
        }

        public async Task<MandrillSenderDomain> CheckDomainAsync(string domain)
        {
            return await MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/check-domain.json",
                new MandrillSenderRequest
                {
                    Domain = domain
                });
        }

        public async Task<MandrillSenderVerifyDomain> VerifyDomainAsync(string domain, string mailbox)
        {
            return await MandrillApi.PostAsync<MandrillSenderVerifyDomainRequest, MandrillSenderVerifyDomain>("senders/verify-domain.json",
                new MandrillSenderVerifyDomainRequest
                {
                    Domain = domain,
                    Mailbox = mailbox
                });
        }

        public async Task<MandrillSenderInfo> InfoAsync(string address)
        {
            return await MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderInfo>("senders/info.json",
                new MandrillSenderRequest
                {
                    Address = address
                });
        }

        public async Task<IList<MandrillSenderTimeSeries>> TimeSeriesAsync(string address)
        {
            return await MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderTimeSeries>>("senders/time-series.json",
                new MandrillSenderRequest
                {
                    Address = address
                });
        }
    }
}
