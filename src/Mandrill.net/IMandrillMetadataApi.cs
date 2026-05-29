#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillMetadataApi
    {
        Task<IList<MandrillMetadataInfo>> ListAsync();
        Task<MandrillMetadataInfo> AddAsync(string name, string? viewTemplate = null);
        Task<MandrillMetadataInfo> UpdateAsync(string name, string viewTemplate);
        Task<MandrillMetadataInfo> DeleteAsync(string name);
    }
}
