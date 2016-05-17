using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillMessagesApi
    {
        Task<IList<MandrillSendMessageResponse>> SendAsync(MandrillMessage message, bool async = false,
            string ipPool = null, DateTime? sendAtUtc = null);

        Task<IList<MandrillSendMessageResponse>> SendTemplateAsync(MandrillMessage message, string templateName,
            IList<MandrillTemplateContent> templateContent = null, bool async = false,
            string ipPool = null, DateTime? sendAtUtc = null);

        Task<IList<MandrillSendMessageResponse>> SendRawAsync(string rawMessage, string fromEmail = null,
            string fromName = null, IList<string> to = null, bool? async = null,
            string ipPool = null, DateTime? sendAtUtc = null, string returnPathDomain = null);

        Task<IList<MandrillMessageInfo>> SearchAsync(string query, DateTime? dateFrom = null, DateTime? dateTo = null,
            IList<string> tags = null, IList<string> senders = null,
            IList<string> apiKeys = null, int? limit = null);

        Task<IList<MandrillMessageTimeSeries>> SearchTimeSeriesAsync(string query, DateTime? dateFrom = null,
            DateTime? dateTo = null, IList<string> tags = null,
            IList<string> senders = null);

        Task<MandrillMessageInfo> InfoAsync(string id);
        Task<MandrillMessageContent> ContentAsync(string id);
        Task<MandrillMessage> ParseAsync(string rawMessage);
        Task<IList<MandrillMessageScheduleInfo>> ListScheduledAsync(string to = null);
        Task<MandrillMessageScheduleInfo> RescheduleAsync(string id, DateTime sendAtUtc);
        Task<MandrillMessageScheduleInfo> CancelScheduledAsync(string id);
    }

#if !NETSTANDARD13
    public partial interface IMandrillMessagesApi
    {
        IList<MandrillSendMessageResponse> Send(MandrillMessage message,
            bool async = false, string ipPool = null, DateTime? sendAtUtc = null);

        IList<MandrillSendMessageResponse> SendTemplate(MandrillMessage message,
            string templateName, IList<MandrillTemplateContent> templateContent = null, bool async = false,
            string ipPool = null,
            DateTime? sendAtUtc = null);

        IList<MandrillSendMessageResponse>
            SendRaw(string rawMessage,
                string fromEmail = null, string fromName = null, IList<string> to = null, bool? async = null,
                string ipPool = null, DateTime? sendAtUtc = null, string returnPathDomain = null);

        IList<MandrillMessageInfo> Search(string query, DateTime? dateFrom = null, DateTime?
            dateTo = null, IList<string> tags = null, IList<string> senders = null, IList<string> apiKeys = null,
            int? limit = null);

        IList<MandrillMessageTimeSeries> SearchTimeSeries(string query,
            DateTime? dateFrom = null, DateTime? dateTo = null, IList<string> tags = null, IList<string> senders = null);

        MandrillMessageInfo Info(string id);
        MandrillMessageContent Content(string id);
        MandrillMessage Parse(string rawMessage);
        IList<MandrillMessageScheduleInfo> ListScheduled(string to = null);
        MandrillMessageScheduleInfo Reschedule(string id, DateTime sendAtUtc);
        MandrillMessageScheduleInfo CancelScheduled(string id);
    }
#endif
}
