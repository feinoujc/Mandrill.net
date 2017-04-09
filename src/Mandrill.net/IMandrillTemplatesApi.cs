using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillTemplatesApi
    {
        Task<MandrillTemplateInfo> AddAsync(string templateName, string code, string text, bool publish, string fromEmail = null,
            string fromName = null, string subject = null, string[] labels = null);

        Task<MandrillTemplateInfo> UpdateAsync(string templateName, string code, string text, bool publish, string fromEmail = null,
            string fromName = null, string subject = null, string[] labels = null);

        Task<MandrillTemplateInfo> DeleteAsync(string templateName);
        Task<IList<MandrillTemplateInfo>> ListAsync(string label = null);
        Task<MandrillTemplateRenderResponse> RenderAsync(string templateName, List<MandrillTemplateContent> templateContent, List<MandrillMergeVar> mergeVars);
        Task<MandrillTemplateInfo> InfoAsync(string templateName);
        Task<MandrillTemplateInfo> PublishAsync(string templateName);
        Task<IList<MandrillMessageTimeSeries>> TimeSeriesAsync(string templateName);
    }
}
