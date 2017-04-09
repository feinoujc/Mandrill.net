using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillSendersApi
    {
        Task<IList<MandrillSenderInfo>> ListAsync();
        Task<IList<MandrillSenderDomain>> DomainsAsync();
        Task<MandrillSenderDomain> AddDomainAsync(string domain);
        Task<MandrillSenderDomain> CheckDomainAsync(string domain);
        Task<MandrillSenderVerifyDomain> VerifyDomainAsync(string domain, string mailbox);
        Task<MandrillSenderInfo> InfoAsync(string address);
        Task<IList<MandrillSenderTimeSeries>> TimeSeriesAsync(string address);
    }
}
