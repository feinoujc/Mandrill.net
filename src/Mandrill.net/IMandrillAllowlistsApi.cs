using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillAllowlistsApi
    {
        Task<IList<MandrillAllowlistInfo>> ListAsync(string email, CancellationToken cancellationToken = default);
        Task<MandrillAllowlistAddResponse> AddAsync(string email, CancellationToken cancellationToken = default);
        Task<MandrillAllowlistDeleteResponse> DeleteAsync(string email, CancellationToken cancellationToken = default);
    }
}
