#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillMcTemplatesApi
    {
        Task<MandrillMcTemplate> InfoAsync(int mcTemplateId, CancellationToken cancellationToken = default);
        Task<IList<MandrillMcTemplate>> ListAsync(string? searchQuery = null, CancellationToken cancellationToken = default);
        Task<MandrillTemplateRenderResponse> RenderAsync(int mcTemplateId, MandrillMcTemplateVersion? mcTemplateVersion = null, List<MandrillMergeVar>? mergeVars = null, CancellationToken cancellationToken = default);
        Task<IList<MandrillMcTemplateTimeSeries>> TimeSeriesAsync(int mcTemplateId, CancellationToken cancellationToken = default);
    }
}
