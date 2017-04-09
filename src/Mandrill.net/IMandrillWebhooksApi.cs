using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillWebHooksApi
    {
        Task<IList<MandrillWebHookInfo>> ListAsync();
        Task<MandrillWebHookInfo> AddAsync(Uri url, string description = null, IList<MandrillWebHookEventType> events = null);
        Task<MandrillWebHookInfo> InfoAsync(int id);
        Task<MandrillWebHookInfo> UpdateAsync(int id, Uri url, string description = null, IList<MandrillWebHookEventType> events = null);
        Task<MandrillWebHookInfo> DeleteAsync(int id);
    }
}
