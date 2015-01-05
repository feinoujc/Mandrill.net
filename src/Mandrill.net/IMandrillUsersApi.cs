using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public interface IMandrillUsersApi
    {
        Task<string> PingAsync();
        Task<IList<MandrillSenderDemographics>> SendersAsync();
        Task<MandrillUserInfo> InfoAsync();
    }

    public static class MandrillUsersSynchronousExtensions
    {
        public static string Ping(this IMandrillUsersApi api)
        {
            return api.PingAsync().Result;
        }

        public static IList<MandrillSenderDemographics> Senders(this IMandrillUsersApi api)
        {
            return api.SendersAsync().Result;
        }

        public static MandrillUserInfo Info(this IMandrillUsersApi api)
        {
            return api.InfoAsync().Result;
        }
    }
}