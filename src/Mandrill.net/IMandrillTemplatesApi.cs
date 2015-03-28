using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public interface IMandrillTemplatesApi
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

    public static class MandrillTemplatesApiSynchronousExtensions
    {
        public static MandrillTemplateInfo Add(this IMandrillTemplatesApi api, string templateName, string code, string text, bool publish, string fromEmail = null, string fromName = null,
            string subject = null, string[] labels = null)
        {
            return AsyncHelper.InvokeSync(api, templates => templates.AddAsync(templateName, code, templateName, publish, fromEmail, fromName, subject, labels));
        }

        public static MandrillTemplateInfo Update(this IMandrillTemplatesApi api, string templateName, string code, string text,
            bool publish, string fromEmail = null, string fromName = null, string subject = null, string[] labels = null)
        {
            return AsyncHelper.InvokeSync(api, templates => templates.UpdateAsync(templateName, code, templateName, publish, fromEmail, fromName, subject, labels));
        }

        public static MandrillTemplateInfo Delete(this IMandrillTemplatesApi api, string templateName)
        {
            return AsyncHelper.InvokeSync(api, templates => templates.DeleteAsync(templateName));
        }

        public static IList<MandrillTemplateInfo> List(this IMandrillTemplatesApi api, string label = null)
        {
            return AsyncHelper.InvokeSync(api, templates => templates.ListAsync(label));
        }

        public static MandrillTemplateRenderResponse Render(this IMandrillTemplatesApi api, string templateName,
            List<MandrillTemplateContent> templateContent, List<MandrillMergeVar> mergeVars)
        {
            return AsyncHelper.InvokeSync(api, templates => templates.RenderAsync(templateName, templateContent, mergeVars));
        }

        public static MandrillTemplateInfo Info(this IMandrillTemplatesApi api, string templateName)
        {
            return AsyncHelper.InvokeSync(api, templates => templates.InfoAsync(templateName));
        }

        public static MandrillTemplateInfo Publish(this IMandrillTemplatesApi api, string templateName)
        {
            return AsyncHelper.InvokeSync(api, templates => templates.PublishAsync(templateName));
        }

        public static IList<MandrillMessageTimeSeries> TimeSeries(this IMandrillTemplatesApi api, string templateName)

        {
            return AsyncHelper.InvokeSync(api, templates => templates.TimeSeriesAsync(templateName));
        }
    }
}