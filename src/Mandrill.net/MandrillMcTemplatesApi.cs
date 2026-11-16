#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    internal partial class MandrillMcTemplatesApi : IMandrillMcTemplatesApi
    {
        public MandrillMcTemplatesApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<MandrillMcTemplate> InfoAsync(int mcTemplateId, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillMcTemplateRequest, MandrillMcTemplate>("mctemplates/info",
                new MandrillMcTemplateRequest { McTemplateId = mcTemplateId }, cancellationToken);
        }

        public Task<IList<MandrillMcTemplate>> ListAsync(string? searchQuery = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillMcTemplateListRequest, IList<MandrillMcTemplate>>("mctemplates/list",
                new MandrillMcTemplateListRequest { SearchQuery = searchQuery }, cancellationToken);
        }

        public Task<MandrillTemplateRenderResponse> RenderAsync(int mcTemplateId, MandrillMcTemplateVersion? mcTemplateVersion = null, List<MandrillMergeVar>? mergeVars = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillMcTemplateRenderRequest, MandrillTemplateRenderResponse>("mctemplates/render",
                new MandrillMcTemplateRenderRequest
                {
                    McTemplateId = mcTemplateId,
                    McTemplateVersion = mcTemplateVersion,
                    MergeVars = mergeVars
                }, cancellationToken);
        }

        public Task<IList<MandrillMcTemplateTimeSeries>> TimeSeriesAsync(int mcTemplateId, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillMcTemplateRequest, IList<MandrillMcTemplateTimeSeries>>("mctemplates/time-series",
                new MandrillMcTemplateRequest { McTemplateId = mcTemplateId }, cancellationToken);
        }
    }
}
