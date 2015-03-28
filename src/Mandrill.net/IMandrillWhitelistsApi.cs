using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandrill
{
    public interface IMandrillWhitelistsApi
    {
        Task<IList<MandrillWhitelistInfo>> ListAsync(string email);
        Task<MandrillWhitelistInfo> AddAsync(string email);
        Task<MandrillWhitelistInfo> DeleteAsync(string email);
    }

    public static class MandrillWhitelistsApiSynchronousExtensions
    {
        public static IList<MandrillWhitelistInfo> List(this IMandrillWhitelistsApi api, string email)
        {
            return AsyncHelper.InvokeSync(api, whitelists => whitelists.ListAsync(email));
        }

        public static MandrillWhitelistInfo Add(this IMandrillWhitelistsApi api, string email)
        {
            return AsyncHelper.InvokeSync(api, whitelists => whitelists.AddAsync(email));
        }

        public static MandrillWhitelistInfo Delete(this IMandrillWhitelistsApi api, string email)
        {
            return AsyncHelper.InvokeSync(api, whitelists => whitelists.DeleteAsync(email));
        }
    }
}
