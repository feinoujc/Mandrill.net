using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public interface IMandrillSendersApi
    {
        Task<IList<MandrillSenderInfo>> ListAsync();
        Task<IList<MandrillSenderDomain>> DomainsAsync();
        Task<MandrillSenderDomain> AddDomainAsync(string domain);
        Task<MandrillSenderDomain> CheckDomainAsync(string domain);
        Task<MandrillSenderVerifyDomain> VerifyDomainAsync(string domain, string mailbox);
        Task<MandrillSenderInfo> InfoAsync(string address);
        Task<IList<MandrillSenderTimeSeries>> TimeSeriesAsync(string address);
    }

    public static class MandrillSendersApiSynchronousExtensions
    {
        public static IList<MandrillSenderInfo> List(this IMandrillSendersApi api)
        {
            return AsyncHelper.InvokeSync(api, senders => senders.ListAsync());
        }

        public static IList<MandrillSenderDomain> Domains(this IMandrillSendersApi api)
        {
            return AsyncHelper.InvokeSync(api, domains => domains.DomainsAsync());
        }

        public static MandrillSenderDomain AddDomain(this IMandrillSendersApi api, string domain)
        {
            return AsyncHelper.InvokeSync(api, add => add.AddDomainAsync(domain));
        }

        public static MandrillSenderDomain CheckDomain(this IMandrillSendersApi api, string domain)
        {
            return AsyncHelper.InvokeSync(api, check => check.CheckDomainAsync(domain));
        }

        public static MandrillSenderVerifyDomain VerifyDomain(this IMandrillSendersApi api, string domain, string mailbox)
        {
            return AsyncHelper.InvokeSync(api, verify => verify.VerifyDomainAsync(domain, mailbox));
        }

        public static MandrillSenderInfo Info(this IMandrillSendersApi api, string address)
        {
            return AsyncHelper.InvokeSync(api, info => info.InfoAsync(address));
        }

        public static IList<MandrillSenderTimeSeries> TimeSeries(this IMandrillSendersApi api, string address)
        {
            return AsyncHelper.InvokeSync(api, timeseries => timeseries.TimeSeriesAsync(address));
        }
    }
}
