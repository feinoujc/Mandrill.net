#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillTemplatesApi
    {
        Task<MandrillTemplateInfo> AddAsync(string templateName, string code, string text, bool publish, string? fromEmail = null,
            string? fromName = null, string? subject = null, string[]? labels = null, CancellationToken cancellationToken = default);

        Task<MandrillTemplateInfo> UpdateAsync(string templateName, string code, string text, bool publish, string? fromEmail = null,
            string? fromName = null, string? subject = null, string[]? labels = null, CancellationToken cancellationToken = default);

        Task<MandrillTemplateInfo> DeleteAsync(string templateName, CancellationToken cancellationToken = default);
        Task<IList<MandrillTemplateInfo>> ListAsync(string? label = null, CancellationToken cancellationToken = default);
        Task<MandrillTemplateRenderResponse> RenderAsync(string templateName, List<MandrillTemplateContent> templateContent, List<MandrillMergeVar> mergeVars, CancellationToken cancellationToken = default);
        Task<MandrillTemplateInfo> InfoAsync(string templateName, CancellationToken cancellationToken = default);
        Task<MandrillTemplateInfo> PublishAsync(string templateName, CancellationToken cancellationToken = default);
        Task<IList<MandrillMessageTimeSeries>> TimeSeriesAsync(string templateName, CancellationToken cancellationToken = default);
    }
}
