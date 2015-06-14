using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public interface IMandrillInboundApi
    {
        Task<IList<MandrillInboundInfo>> DomainsAsync();
        Task<MandrillInboundInfo> AddDomainAsync(string domain);
        Task<MandrillInboundInfo> CheckDomainAsync(string domain);
        Task<MandrillInboundInfo> DeleteDomainAsync(string domain);
        Task<IList<MandrillInboundRoute>> RoutesAsync(string domain);
        Task<MandrillInboundRoute> AddRouteAsync(string domain, string pattern, Uri url);
        Task<MandrillInboundRoute> UpdateRouteAsync(string id, string pattern, Uri url);
        Task<MandrillInboundRoute> DeleteRouteAsync(string id);

        Task<IList<MandrillInboundSendResponse>> SendRawAsync(string rawMessage, IList<string> to = null, string mailFrom = null,
            string helo = null, string clientAddress = null);
    }

    public static class MandrillMandrillInboundApiSynchronousExtensions
    {
        public static IList<MandrillInboundInfo> Domains(this IMandrillInboundApi api)
        {
            return AsyncHelper.InvokeSync(api, inboundApi => inboundApi.DomainsAsync());
        }

        public static MandrillInboundInfo AddDomain(this IMandrillInboundApi api, string domain)
        {
            return AsyncHelper.InvokeSync(api, inboundApi => inboundApi.AddDomainAsync(domain));
        }

        public static MandrillInboundInfo CheckDomain(this IMandrillInboundApi api, string domain)
        {
            return AsyncHelper.InvokeSync(api, inboundApi => inboundApi.CheckDomainAsync(domain));
        }

        public static MandrillInboundInfo DeleteDomain(this IMandrillInboundApi api, string domain)
        {
            return AsyncHelper.InvokeSync(api, inboundApi => inboundApi.DeleteDomainAsync(domain));
        }

        public static IList<MandrillInboundRoute> Routes(this IMandrillInboundApi api, string domain)
        {
            return AsyncHelper.InvokeSync(api, inboundApi => inboundApi.RoutesAsync(domain));
        }

        public static MandrillInboundRoute AddRoute(this IMandrillInboundApi api, string domain, string pattern, Uri url)
        {
            return AsyncHelper.InvokeSync(api, inboundApi => inboundApi.AddRouteAsync(domain, pattern, url));
        }

        public static MandrillInboundRoute UpdateRoute(this IMandrillInboundApi api, string id, string pattern, Uri url)
        {
            return AsyncHelper.InvokeSync(api, inboundApi => inboundApi.UpdateRouteAsync(id, pattern, url));
        }

        public static MandrillInboundRoute DeleteRoute(this IMandrillInboundApi api, string id)
        {
            return AsyncHelper.InvokeSync(api, inboundApi => inboundApi.DeleteRouteAsync(id));
        }

        public static IList<MandrillInboundSendResponse> SendRaw(this IMandrillInboundApi api, string rawMessage, IList<string> to = null, string mailFrom = null, string helo = null,
            string clientAddress = null)
        {
            return AsyncHelper.InvokeSync(api, inboundApi => inboundApi.SendRawAsync(rawMessage, to, mailFrom, helo, clientAddress));
        }
    }
}