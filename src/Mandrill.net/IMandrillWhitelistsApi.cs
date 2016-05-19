using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillWhitelistsApi
    {
        Task<IList<MandrillWhitelistInfo>> ListAsync(string email);
        Task<MandrillWhitelistInfo> AddAsync(string email);
        Task<MandrillWhitelistInfo> DeleteAsync(string email);
    }
#if NETFX

    public partial interface IMandrillWhitelistsApi
    {
        IList<MandrillWhitelistInfo> List(string email);
        MandrillWhitelistInfo Add(string email);
        MandrillWhitelistInfo Delete(string email);
    }
#endif
}
