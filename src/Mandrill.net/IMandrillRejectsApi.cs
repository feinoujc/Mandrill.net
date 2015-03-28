using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public interface IMandrillRejectsApi
    {
        Task<MandrillRejectAddResponse> AddAsync(string email, string comment = null, string subaccount = null);
        Task<MandrillRejectDeleteResponse> DeleteAsync(string email, string subaccount = null);
        Task<IList<MandrillRejectInfo>> ListAsync(string email = null, bool? includeExpired = null, string subaccount = null);
    }

    public static class MandrillRejectsApiSynchronousExtensions
    {
        public static MandrillRejectAddResponse Add(this IMandrillRejectsApi api, string email, string comment = null, string subaccount = null)
        {
            return AsyncHelper.InvokeSync(api, rejects => rejects.AddAsync(email, comment, subaccount));
        }

        public static MandrillRejectDeleteResponse Delete(this IMandrillRejectsApi api, string email, string subaccount = null)
        {
            return AsyncHelper.InvokeSync(api, rejects => rejects.DeleteAsync(email, subaccount));
        }

        public static IList<MandrillRejectInfo> List(this IMandrillRejectsApi api, string email = null,
            bool? includeExpired = null, string subaccount = null)
        {
            return AsyncHelper.InvokeSync(api, rejects => rejects.ListAsync(email, includeExpired, subaccount));
        }
    }
}