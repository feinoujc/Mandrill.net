using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public interface IMandrillSubaccountsApi
    {
        Task<IList<MandrillSubaccountResponse>> ListAsync(string q = null);
        Task<MandrillSubaccountResponse> AddAsync(string id, string name = null, string notes = null, int? customQuota = null);
        Task<MandrillSubaccountInfo> InfoAsync(string id);
        Task<MandrillSubaccountResponse> UpdateAsync(string id, string name = null, string notes = null, int? customQuota = null);
        Task<MandrillSubaccountResponse> DeleteAsync(string id);
        Task<MandrillSubaccountResponse> PauseAsync(string id);
        Task<MandrillSubaccountResponse> ResumeAsync(string id);
    }

    public static class MandrillSubaccountsApiSynchronousExtensions
    {
        public static IList<MandrillSubaccountResponse> List(this IMandrillSubaccountsApi subaccounts, string q = null)
        {
            return AsyncHelper.InvokeSync(subaccounts, api => api.ListAsync(q));
        }

        public static MandrillSubaccountResponse Add(this IMandrillSubaccountsApi subaccounts, string id, string name = null, string notes = null, int? customQuota = null)
        {
            return AsyncHelper.InvokeSync(subaccounts, api => api.AddAsync(id, name, notes, customQuota));
        }

        public static MandrillSubaccountInfo Info(this IMandrillSubaccountsApi subaccounts, string id)
        {
            return AsyncHelper.InvokeSync(subaccounts, api => api.InfoAsync(id));
        }

        public static MandrillSubaccountResponse Update(this IMandrillSubaccountsApi subaccounts, string id, string name = null, string notes = null, int? customQuota = null)
        {
            return AsyncHelper.InvokeSync(subaccounts, api => api.UpdateAsync(id, name, notes, customQuota));
        }

        public static MandrillSubaccountResponse Delete(this IMandrillSubaccountsApi subaccounts, string id)
        {
            return AsyncHelper.InvokeSync(subaccounts, api => api.DeleteAsync(id));
        }

        public static MandrillSubaccountResponse Pause(this IMandrillSubaccountsApi subaccounts, string id)
        {
            return AsyncHelper.InvokeSync(subaccounts, api => api.PauseAsync(id));
        }

        public static MandrillSubaccountResponse Resume(this IMandrillSubaccountsApi subaccounts, string id)
        {
            return AsyncHelper.InvokeSync(subaccounts, api => api.ResumeAsync(id));
        }
    }
}