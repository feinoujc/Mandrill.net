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

#if !DNXCORE50
    public partial interface IMandrillSubaccountsApi
    {
        IList<MandrillSubaccountResponse> List(string q = null);
        MandrillSubaccountResponse Add(string id, string name = null, string notes = null, int? customQuota = null);
        MandrillSubaccountInfo Info(string id);
        MandrillSubaccountResponse Update(string id, string name = null, string notes = null, int? customQuota = null);
        MandrillSubaccountResponse Delete(string id);
        MandrillSubaccountResponse Pause(string id);
        MandrillSubaccountResponse Resume(string id);
    }
#endif
}