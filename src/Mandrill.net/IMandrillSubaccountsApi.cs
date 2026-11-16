#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillSubaccountsApi
    {
        Task<IList<MandrillSubaccountResponse>> ListAsync(string? q = null, CancellationToken cancellationToken = default);
        Task<MandrillSubaccountResponse> AddAsync(string id, string? name = null, string? notes = null, int? customQuota = null, CancellationToken cancellationToken = default);
        Task<MandrillSubaccountInfo> InfoAsync(string id, CancellationToken cancellationToken = default);
        Task<MandrillSubaccountResponse> UpdateAsync(string id, string? name = null, string? notes = null, int? customQuota = null, CancellationToken cancellationToken = default);
        Task<MandrillSubaccountResponse> DeleteAsync(string id, CancellationToken cancellationToken = default);
        Task<MandrillSubaccountResponse> PauseAsync(string id, CancellationToken cancellationToken = default);
        Task<MandrillSubaccountResponse> ResumeAsync(string id, CancellationToken cancellationToken = default);
    }
}
