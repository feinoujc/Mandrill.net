#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    internal partial class MandrillMetadataApi : IMandrillMetadataApi
    {
        public MandrillMetadataApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillMetadataInfo>> ListAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillMetadataDeleteRequest, IList<MandrillMetadataInfo>>("metadata/list",
                new MandrillMetadataDeleteRequest(), cancellationToken);
        }

        public Task<MandrillMetadataInfo> AddAsync(string name, string? viewTemplate = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillMetadataAddRequest, MandrillMetadataInfo>("metadata/add",
                new MandrillMetadataAddRequest { Name = name, ViewTemplate = viewTemplate }, cancellationToken);
        }

        public Task<MandrillMetadataInfo> UpdateAsync(string name, string viewTemplate, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillMetadataUpdateRequest, MandrillMetadataInfo>("metadata/update",
                new MandrillMetadataUpdateRequest { Name = name, ViewTemplate = viewTemplate }, cancellationToken);
        }

        public Task<MandrillMetadataInfo> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillMetadataDeleteRequest, MandrillMetadataInfo>("metadata/delete",
                new MandrillMetadataDeleteRequest { Name = name }, cancellationToken);
        }
    }
}
