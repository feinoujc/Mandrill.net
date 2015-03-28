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
            return AsyncHelper.InvokeSync(api, users => users.PingAsync());
        }

        public static IList<MandrillSenderDemographics> Senders(this IMandrillUsersApi api)
        {
            return AsyncHelper.InvokeSync(api, users => users.SendersAsync());
        }

        public static MandrillUserInfo Info(this IMandrillUsersApi api)
        {
            return AsyncHelper.InvokeSync(api, users => users.InfoAsync());
        }
    }
}