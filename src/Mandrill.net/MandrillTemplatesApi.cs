#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;
using Mandrill.Serialization;

namespace Mandrill
{
    internal partial class MandrillTemplatesApi : IMandrillTemplatesApi
    {
        internal MandrillTemplatesApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<MandrillTemplateInfo> AddAsync(string templateName, string code, string text, bool publish, string? fromEmail = null,
            string? fromName = null, string? subject = null, string[]? labels = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/add", new MandrillTemplateRequest
            {
                Name = templateName,
                Code = code,
                Text = text,
                Publish = publish,
                FromEmail = fromEmail,
                FromName = fromName,
                Subject = subject,
                Labels = labels?.ToList()
            }, cancellationToken);
        }

        public Task<MandrillTemplateInfo> UpdateAsync(string templateName, string code, string text, bool publish, string? fromEmail = null,
            string? fromName = null, string? subject = null, string[]? labels = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/update", new MandrillTemplateRequest
            {
                Name = templateName,
                Code = code,
                Text = text,
                Publish = publish,
                FromEmail = fromEmail,
                FromName = fromName,
                Subject = subject,
                Labels = labels?.ToList()
            }, cancellationToken);
        }

        public Task<MandrillTemplateInfo> DeleteAsync(string templateName, CancellationToken cancellationToken = default)
        {
            if (templateName == null) throw new ArgumentNullException(nameof(templateName));
            return MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/delete", new MandrillTemplateRequest
            {
                Name = templateName,
            }, cancellationToken);
        }

        public Task<IList<MandrillTemplateInfo>> ListAsync(string? label = null, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTemplateListRequest, IList<MandrillTemplateInfo>>("templates/list", new MandrillTemplateListRequest
            {
                Label = label,
            }, cancellationToken);
        }

        public Task<MandrillTemplateRenderResponse> RenderAsync(string templateName, List<MandrillTemplateContent> templateContent, List<MandrillMergeVar> mergeVars, CancellationToken cancellationToken = default)
        {
            if (templateName == null) throw new ArgumentNullException(nameof(templateName));

            return MandrillApi.PostAsync<MandrillTemplateRenderRequest, MandrillTemplateRenderResponse>("templates/render",
                new MandrillTemplateRenderRequest
                {
                    TemplateName = templateName,
                    TemplateContent = templateContent,
                    MergeVars = mergeVars,
                }, cancellationToken);
        }

        public Task<MandrillTemplateInfo> InfoAsync(string templateName, CancellationToken cancellationToken = default)
        {
            if (templateName == null) throw new ArgumentNullException(nameof(templateName));
            return MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/info", new MandrillTemplateRequest
            {
                Name = templateName,
            }, cancellationToken);
        }

        public Task<MandrillTemplateInfo> PublishAsync(string templateName, CancellationToken cancellationToken = default)
        {
            if (templateName == null) throw new ArgumentNullException(nameof(templateName));
            return MandrillApi.PostAsync<MandrillTemplateRequest, MandrillTemplateInfo>("templates/publish", new MandrillTemplateRequest
            {
                Name = templateName,
            }, cancellationToken);
        }

        public Task<IList<MandrillMessageTimeSeries>> TimeSeriesAsync(string templateName, CancellationToken cancellationToken = default)
        {
            if (templateName == null) throw new ArgumentNullException(nameof(templateName));
            return MandrillApi.PostAsync<MandrillTemplateRequest, IList<MandrillMessageTimeSeries>>("templates/time-series", new MandrillTemplateRequest
            {
                Name = templateName,
            }, cancellationToken);
        }
    }
}
