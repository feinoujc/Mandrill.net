#nullable enable
using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillExportsApi
    {
        Task<IList<MandrillExportInfo>> ListAsync(CancellationToken cancellationToken = default);
        Task<MandrillExportInfo> InfoAsync(string id, CancellationToken cancellationToken = default);
        Task<MandrillExportInfo> RejectsAsync(string notifyEmail, CancellationToken cancellationToken = default);
        Task<MandrillExportInfo> WhitelistAsync(string notifyEmail, CancellationToken cancellationToken = default);
        Task<MandrillExportInfo> AllowlistAsync(string? notifyEmail = null, CancellationToken cancellationToken = default);
        Task<MandrillExportInfo> ActivityAsync(string notifyEmail,
            DateOnly? dateFrom = null,
            DateOnly? dateTo = null,
            IList<string>? tags = null,
            IList<string>? senders = null,
            IList<string>? states = null,
            IList<string>? apiKeys = null, CancellationToken cancellationToken = default);
    }
}
