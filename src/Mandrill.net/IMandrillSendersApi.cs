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

#if !DNXCORE50
    public partial interface IMandrillSendersApi
    {
        IList<MandrillSenderInfo> List();
        IList<MandrillSenderDomain> Domains();
        MandrillSenderDomain AddDomain(string domain);
        MandrillSenderDomain CheckDomain(string domain);
        MandrillSenderVerifyDomain VerifyDomain(string domain, string mailbox);
        MandrillSenderInfo Info(string address);
        IList<MandrillSenderTimeSeries> TimeSeries(string address);
    }
#endif
}