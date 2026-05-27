#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill;

public partial interface IMandrillMessagesApi
{
    Task<IList<MandrillSendMessageResponse>> SendAsync(MandrillMessage message, bool? async = default, string? ipPool = default, string? sendAt = default, CancellationToken cancellationToken = default);

    Task<IList<MandrillSendMessageResponse>> SendTemplateAsync(string templateName, IList<MandrillTemplateContent> templateContent, MandrillMessage message, bool? async = default, string? ipPool = default, string? sendAt = default, CancellationToken cancellationToken = default);

    Task<IList<MandrillMessageInfo>> SearchAsync(string? query = default, string? dateFrom = default, string? dateTo = default, IList<string>? tags = default, IList<string>? senders = default, IList<string>? apiKeys = default, int? limit = default, CancellationToken cancellationToken = default);

    Task<IList<MandrillMessageTimeSeries>> SearchTimeSeriesAsync(string? query = default, string? dateFrom = default, string? dateTo = default, IList<string>? tags = default, IList<string>? senders = default, CancellationToken cancellationToken = default);

    Task<MandrillMessageInfo> ParseAsync(string rawMessage, CancellationToken cancellationToken = default);

    Task<IList<MandrillSendMessageResponse>> SendRawAsync(string rawMessage, string? fromEmail = default, string? fromName = default, IList<string>? to = default, bool? async = default, string? ipPool = default, string? sendAt = default, string? returnPathDomain = default, CancellationToken cancellationToken = default);

    Task<MandrillMessageScheduleInfo> RescheduleAsync(string id, string sendAt, CancellationToken cancellationToken = default);
}
