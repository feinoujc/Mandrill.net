using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillSubaccountsApi
    {
        Task<IList<MandrillSubaccountResponse>> ListAsync(string q = null);
        Task<MandrillSubaccountResponse> AddAsync(string id, string name = null, string notes = null, int? customQuota = null);
        Task<MandrillSubaccountInfo> InfoAsync(string id);
        Task<MandrillSubaccountResponse> UpdateAsync(string id, string name = null, string notes = null, int? customQuota = null);
        Task<MandrillSubaccountResponse> DeleteAsync(string id);
        Task<MandrillSubaccountResponse> PauseAsync(string id);
        Task<MandrillSubaccountResponse> ResumeAsync(string id);
    }
}
