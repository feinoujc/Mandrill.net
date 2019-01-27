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
}
