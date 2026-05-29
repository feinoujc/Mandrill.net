#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillMcTemplatesApi
    {
        Task<MandrillMcTemplate> InfoAsync(int mcTemplateId);
        Task<IList<MandrillMcTemplate>> ListAsync(string? searchQuery = null);
        Task<MandrillTemplateRenderResponse> RenderAsync(int mcTemplateId, MandrillMcTemplateVersion? mcTemplateVersion = null, List<MandrillMergeVar>? mergeVars = null);
        Task<IList<MandrillMcTemplateTimeSeries>> TimeSeriesAsync(int mcTemplateId);
    }
}
