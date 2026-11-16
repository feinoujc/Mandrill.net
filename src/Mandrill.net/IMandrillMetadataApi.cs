#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillMetadataApi
    {
        Task<IList<MandrillMetadataInfo>> ListAsync(CancellationToken cancellationToken = default);
        Task<MandrillMetadataInfo> AddAsync(string name, string? viewTemplate = null, CancellationToken cancellationToken = default);
        Task<MandrillMetadataInfo> UpdateAsync(string name, string viewTemplate, CancellationToken cancellationToken = default);
        Task<MandrillMetadataInfo> DeleteAsync(string name, CancellationToken cancellationToken = default);
    }
}
