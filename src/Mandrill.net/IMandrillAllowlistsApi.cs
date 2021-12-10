using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillAllowlistsApi
    {
        Task<IList<MandrillAllowlistInfo>> ListAsync(string email);
        Task<MandrillAllowlistInfo> AddAsync(string email);
        Task<MandrillAllowlistInfo> DeleteAsync(string email);
    }
}
