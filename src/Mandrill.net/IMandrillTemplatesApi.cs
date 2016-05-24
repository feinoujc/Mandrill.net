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

    #if NET45

    public partial interface IMandrillTemplatesApi
    {
        MandrillTemplateInfo Add(string templateName, string code, string text, bool publish, string fromEmail = null,
            string fromName = null, string subject = null, string[] labels = null);

        MandrillTemplateInfo Update(string templateName, string code, string text, bool publish, string fromEmail = null,
            string fromName = null, string subject = null, string[] labels = null);

        MandrillTemplateInfo Delete(string templateName);
        IList<MandrillTemplateInfo> List(string label = null);
        MandrillTemplateRenderResponse Render(string templateName, List<MandrillTemplateContent> templateContent, List<MandrillMergeVar> mergeVars);
        MandrillTemplateInfo Info(string templateName);
        MandrillTemplateInfo Publish(string templateName);
        IList<MandrillMessageTimeSeries> TimeSeries(string templateName);
    }
#endif
}