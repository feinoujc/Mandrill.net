#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillWebHooksApi
    {
        Task<IList<MandrillWebHookInfo>> ListAsync(CancellationToken cancellationToken = default);
        Task<MandrillWebHookInfo> AddAsync(Uri url, string? description = null, IList<MandrillWebHookEventType>? events = null, CancellationToken cancellationToken = default);
        Task<MandrillWebHookInfo> InfoAsync(int id, CancellationToken cancellationToken = default);
        Task<MandrillWebHookInfo> UpdateAsync(int id, Uri url, string? description = null, IList<MandrillWebHookEventType>? events = null, CancellationToken cancellationToken = default);
        Task<MandrillWebHookInfo> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
