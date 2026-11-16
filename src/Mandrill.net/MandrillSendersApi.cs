using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
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

        public Task<IList<MandrillSenderInfo>> ListAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderInfo>>("senders/list",
                new MandrillSenderRequest(), cancellationToken);
        }

        public Task<IList<MandrillSenderDomain>> DomainsAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderDomain>>("senders/domains",
                new MandrillSenderRequest(), cancellationToken);
        }

        public Task<MandrillSenderDomain> AddDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/add-domain",
                new MandrillSenderRequest
                {
                    Domain = domain
                }, cancellationToken);
        }

        public Task<MandrillSenderDomain> CheckDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/check-domain",
                new MandrillSenderRequest
                {
                    Domain = domain
                }, cancellationToken);
        }

        public Task<MandrillSenderVerifyDomain> VerifyDomainAsync(string domain, string mailbox, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSenderVerifyDomainRequest, MandrillSenderVerifyDomain>("senders/verify-domain",
                new MandrillSenderVerifyDomainRequest
                {
                    Domain = domain,
                    Mailbox = mailbox
                }, cancellationToken);
        }

        public Task<MandrillSenderInfo> InfoAsync(string address, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderInfo>("senders/info",
                new MandrillSenderRequest
                {
                    Address = address
                }, cancellationToken);
        }

        public Task<IList<MandrillSenderTimeSeries>> TimeSeriesAsync(string address, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, IList<MandrillSenderTimeSeries>>("senders/time-series",
                new MandrillSenderRequest
                {
                    Address = address
                }, cancellationToken);
        }

        public Task<MandrillSenderDomain> DeleteDomainAsync(string domain, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillSenderRequest, MandrillSenderDomain>("senders/delete-domain",
                new MandrillSenderRequest
                {
                    Domain = domain
                }, cancellationToken);
        }
    }
}
