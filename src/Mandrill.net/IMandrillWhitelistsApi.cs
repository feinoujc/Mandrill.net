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
            return api.ListAsync(email).Result;
        }

        public static MandrillWhitelistInfo Add(this IMandrillWhitelistsApi api, string email)
        {
            return api.AddAsync(email).Result;
        }

        public static MandrillWhitelistInfo Delete(this IMandrillWhitelistsApi api, string email)
        {
            return api.DeleteAsync(email).Result;
        }
    }
}
