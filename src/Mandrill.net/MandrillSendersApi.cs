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

#if !DNXCORE50
    internal partial class MandrillSendersApi
    {
        public IList<MandrillSenderInfo> List()
        {
            return MandrillApi.Post<MandrillSenderRequest, IList<MandrillSenderInfo>>("senders/list.json",
                new MandrillSenderRequest());
        }

        public IList<MandrillSenderDomain> Domains()
        {
            return MandrillApi.Post<MandrillSenderRequest, IList<MandrillSenderDomain>>("senders/domains.json",
                new MandrillSenderRequest());
        }

        public MandrillSenderDomain AddDomain(string domain)
        {
            return MandrillApi.Post<MandrillSenderRequest, MandrillSenderDomain>("senders/add-domain.json",
                new MandrillSenderRequest
                {
                    Domain = domain
                });
        }

        public MandrillSenderDomain CheckDomain(string domain)
        {
            return MandrillApi.Post<MandrillSenderRequest, MandrillSenderDomain>("senders/check-domain.json",
                new MandrillSenderRequest
                {
                    Domain = domain
                });
        }

        public MandrillSenderVerifyDomain VerifyDomain(string domain, string mailbox)
        {
            return MandrillApi.Post<MandrillSenderVerifyDomainRequest, MandrillSenderVerifyDomain>("senders/verify-domain.json",
                new MandrillSenderVerifyDomainRequest
                {
                    Domain = domain,
                    Mailbox = mailbox
                });
        }

        public MandrillSenderInfo Info(string address)
        {
            return MandrillApi.Post<MandrillSenderRequest, MandrillSenderInfo>("senders/info.json",
                new MandrillSenderRequest
                {
                    Address = address
                });
        }

        public IList<MandrillSenderTimeSeries> TimeSeries(string address)
        {
            return MandrillApi.Post<MandrillSenderRequest, IList<MandrillSenderTimeSeries>>("senders/time-series.json",
                new MandrillSenderRequest
                {
                    Address = address
                });
        }
    }
#endif
}
