#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillMessagesApi
    {
        Task<IList<MandrillSendMessageResponse>> SendAsync(MandrillMessage message, bool async = false,
            string? ipPool = null, DateTime? sendAtUtc = null);

        Task<IList<MandrillSendMessageResponse>> SendTemplateAsync(MandrillMessage message, string templateName,
            IList<MandrillTemplateContent>? templateContent = null, bool async = false,
            string? ipPool = null, DateTime? sendAtUtc = null);

        Task<IList<MandrillSendMessageResponse>> SendRawAsync(string rawMessage, string? fromEmail = null,
            string? fromName = null, IList<string>? to = null, bool? async = null,
            string? ipPool = null, DateTime? sendAtUtc = null, string? returnPathDomain = null);

        Task<IList<MandrillMessageInfo>> SearchAsync(string query, DateOnly? dateFrom = null, DateOnly? dateTo = null,
            IList<string>? tags = null, IList<string>? senders = null,
            IList<string>? apiKeys = null, int? limit = null);

        Task<IList<MandrillMessageTimeSeries>> SearchTimeSeriesAsync(string query, DateOnly? dateFrom = null,
            DateOnly? dateTo = null, IList<string>? tags = null,
            IList<string>? senders = null);

        Task<MandrillMessageInfo> InfoAsync(string id);
        Task<MandrillMessageContent> ContentAsync(string id);
        Task<MandrillParsedMessage> ParseAsync(string rawMessage);
        Task<IList<MandrillMessageScheduleInfo>> ListScheduledAsync(string? to = null);
        Task<MandrillMessageScheduleInfo> RescheduleAsync(string id, DateTime sendAtUtc);
        Task<MandrillMessageScheduleInfo> CancelScheduledAsync(string id);

        Task<IList<MandrillSmsMessage>> SendSmsAsync(MandrillSmsDetails sms,
            IList<MandrillRcptMergeVar>? mergeVars = null,
            IList<MandrillMergeVar>? globalMergeVars = null,
            bool? async = null);

        Task<IList<MandrillSendMessageResponse>> SendMcTemplateAsync(MandrillMessage message,
            int mcTemplateId,
            MandrillMcTemplateVersion? mcTemplateVersion = null,
            bool async = false,
            string? ipPool = null,
            DateTime? sendAtUtc = null);
    }
}
