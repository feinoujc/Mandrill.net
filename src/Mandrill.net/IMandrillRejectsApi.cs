using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillRejectsApi
    {
        Task<MandrillRejectAddResponse> AddAsync(string email, string comment = null, string subaccount = null);
        Task<MandrillRejectDeleteResponse> DeleteAsync(string email, string subaccount = null);
        Task<IList<MandrillRejectInfo>> ListAsync(string email = null, bool? includeExpired = null, string subaccount = null);
    }

#if NETFX
    public partial interface IMandrillRejectsApi
    {
        MandrillRejectAddResponse Add(string email, string comment = null, string subaccount = null);

        MandrillRejectDeleteResponse Delete(string email, string subaccount = null);

        IList<MandrillRejectInfo> List(string email = null,
            bool? includeExpired = null, string subaccount = null);

    }
#endif
}