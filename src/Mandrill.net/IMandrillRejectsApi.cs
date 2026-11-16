#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillRejectsApi
    {
        Task<MandrillRejectAddResponse> AddAsync(string email, string? comment = null, string? subaccount = null, CancellationToken cancellationToken = default);
        Task<MandrillRejectDeleteResponse> DeleteAsync(string email, string? subaccount = null, CancellationToken cancellationToken = default);
        Task<IList<MandrillRejectInfo>> ListAsync(string? email = null, bool? includeExpired = null, string? subaccount = null, CancellationToken cancellationToken = default);

        Task<MandrillSmsReject> AddSmsAsync(string phone, string? comment = null, string? subaccount = null, CancellationToken cancellationToken = default);
        Task<MandrillSmsReject> DeleteSmsAsync(string phone, string? subaccount = null, CancellationToken cancellationToken = default);
        Task<IList<MandrillSmsRejectInfo>> ListSmsAsync(string? phone = null, bool? includeExpired = null, string? subaccount = null, CancellationToken cancellationToken = default);
    }
}
