using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public interface IMandrillWebHooksApi
    {
        Task<IList<MandrillWebHookInfo>> ListAsync();
        Task<MandrillWebHookInfo> AddAsync(Uri url, string description = null, IList<MandrillWebHookEventType> events = null);
        Task<MandrillWebHookInfo> InfoAsync(int id);
        Task<MandrillWebHookInfo> UpdateAsync(int id, Uri url, string description = null, IList<MandrillWebHookEventType> events = null);
        Task<MandrillWebHookInfo> DeleteAsync(int id);
    }

    public static class MandrillWebHooksApiSynchronousExtensions
    {
        public static IList<MandrillWebHookInfo> List(this IMandrillWebHooksApi api)
        {
            return AsyncHelper.InvokeSync(api, hooksApi => hooksApi.ListAsync());
        }

        public static MandrillWebHookInfo Add(this IMandrillWebHooksApi api, Uri url, string description = null, IList<MandrillWebHookEventType> events = null)
        {
            return AsyncHelper.InvokeSync(api, hooksApi => hooksApi.AddAsync(url, description, events));
        }

        public static MandrillWebHookInfo Info(this IMandrillWebHooksApi api, int id)
        {
            return AsyncHelper.InvokeSync(api, hooksApi => hooksApi.InfoAsync(id));
        }

        public static MandrillWebHookInfo Update(this IMandrillWebHooksApi api, int id, Uri url, string description = null, IList<MandrillWebHookEventType> events = null)
        {
            return AsyncHelper.InvokeSync(api, hooksApi => hooksApi.UpdateAsync(id, url, description, events));
        }

        public static MandrillWebHookInfo Delete(this IMandrillWebHooksApi api, int id)
        {
            return AsyncHelper.InvokeSync(api, hooksApi => hooksApi.DeleteAsync(id));
        }
    }
}