#nullable enable
using Mandrill.Model;
using System.Collections.Generic;
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

        public Task<MandrillMcTemplate> InfoAsync(int mcTemplateId)
        {
            return MandrillApi.PostAsync<MandrillMcTemplateRequest, MandrillMcTemplate>("mctemplates/info",
                new MandrillMcTemplateRequest { McTemplateId = mcTemplateId });
        }

        public Task<IList<MandrillMcTemplate>> ListAsync(string? searchQuery = null)
        {
            return MandrillApi.PostAsync<MandrillMcTemplateListRequest, IList<MandrillMcTemplate>>("mctemplates/list",
                new MandrillMcTemplateListRequest { SearchQuery = searchQuery });
        }

        public Task<MandrillTemplateRenderResponse> RenderAsync(int mcTemplateId, MandrillMcTemplateVersion? mcTemplateVersion = null, List<MandrillMergeVar>? mergeVars = null)
        {
            return MandrillApi.PostAsync<MandrillMcTemplateRenderRequest, MandrillTemplateRenderResponse>("mctemplates/render",
                new MandrillMcTemplateRenderRequest
                {
                    McTemplateId = mcTemplateId,
                    McTemplateVersion = mcTemplateVersion,
                    MergeVars = mergeVars
                });
        }

        public Task<IList<MandrillMcTemplateTimeSeries>> TimeSeriesAsync(int mcTemplateId)
        {
            return MandrillApi.PostAsync<MandrillMcTemplateRequest, IList<MandrillMcTemplateTimeSeries>>("mctemplates/time-series",
                new MandrillMcTemplateRequest { McTemplateId = mcTemplateId });
        }
    }
}
