using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public interface IMandrillMessagesApi
    {
        Task<IList<MandrillSendMessageResponse>> SendAsync(MandrillMessage message, bool async = false, string ipPool = null, DateTime? sendAtUtc = null);

        Task<IList<MandrillSendMessageResponse>> SendTemplateAsync(MandrillMessage message, string templateName, IList<MandrillTemplateContent> templateContent = null, bool async = false,
            string ipPool = null, DateTime? sendAtUtc = null);

        Task<IList<MandrillSendMessageResponse>> SendRawAsync(string rawMessage, string fromEmail = null, string fromName = null, IList<string> to = null, bool? async = null,
            string ipPool = null, DateTime? sendAtUtc = null, string returnPathDomain = null);

        Task<IList<MandrillMessageInfo>> SearchAsync(string query, DateTime? dateFrom = null, DateTime? dateTo = null, IList<string> tags = null, IList<string> senders = null,
            IList<string> apiKeys = null, int? limit = null);

        Task<IList<MandrillMessageTimeSeries>> SearchTimeSeriesAsync(string query, DateTime? dateFrom = null, DateTime? dateTo = null, IList<string> tags = null,
            IList<string> senders = null);

        Task<MandrillMessageInfo> InfoAync(string id);
        Task<MandrillMessageContent> ContentAsync(string id);
        Task<MandrillMessage> ParseAsync(string rawMessage);
        Task<IList<MandrillMessageScheduleInfo>> ListScheduledAsync(string to = null);
        Task<MandrillMessageScheduleInfo> RescheduleAsync(string id, DateTime sendAtUtc);
        Task<MandrillMessageScheduleInfo> CancelScheduledAsync(string id);
    }

    public static class MandrillMessagesApiApiSynchronousExtensions
    {
        public static IList<MandrillSendMessageResponse> Send(this IMandrillMessagesApi api, MandrillMessage message,
            bool async = false, string ipPool = null, DateTime? sendAtUtc = null)
        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.SendAsync(message, async, ipPool, sendAtUtc));
        }

        public static IList<MandrillSendMessageResponse> SendTemplate(this IMandrillMessagesApi api, MandrillMessage message,
            string templateName, IList<MandrillTemplateContent> templateContent = null, bool async = false, string ipPool = null,
            DateTime? sendAtUtc = null)
        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.SendTemplateAsync(message, templateName, templateContent, async, ipPool, sendAtUtc));
        }

        public static IList<MandrillSendMessageResponse>
            SendRaw(this IMandrillMessagesApi api, string rawMessage,
                string fromEmail = null, string fromName = null, IList<string> to = null, bool? async = null,
                string ipPool = null, DateTime? sendAtUtc = null, string returnPathDomain = null)
        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.SendRawAsync(rawMessage, fromEmail, fromName, to, async, ipPool, sendAtUtc, returnPathDomain));
        }

        public static IList<MandrillMessageInfo> Search(this IMandrillMessagesApi api, string query, DateTime? dateFrom = null, DateTime?
            dateTo = null, IList<string> tags = null, IList<string> senders = null, IList<string> apiKeys = null, int? limit = null)

        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.SearchAsync(query, dateFrom, dateTo, tags, senders, apiKeys, limit));
        }

        public static IList<MandrillMessageTimeSeries> SearchTimeSeries(this IMandrillMessagesApi api, string query,
            DateTime? dateFrom = null, DateTime? dateTo = null, IList<string> tags = null, IList<string> senders = null)
        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.SearchTimeSeriesAsync(query, dateFrom, dateTo, tags, senders));
        }

        public static MandrillMessageInfo Info(this IMandrillMessagesApi api, string id)
        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.InfoAync(id));
        }

        public static MandrillMessageContent Content(this IMandrillMessagesApi api, string id)
        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.ContentAsync(id));
        }

        public static MandrillMessage Parse(this IMandrillMessagesApi api, string rawMessage)
        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.ParseAsync(rawMessage));
        }

        public static IList<MandrillMessageScheduleInfo> ListScheduled(this IMandrillMessagesApi api, string to = null)
        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.ListScheduledAsync(to));
        }

        public static MandrillMessageScheduleInfo Reschedule(this IMandrillMessagesApi api, string id, DateTime sendAtUtc)
        {
            return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.RescheduleAsync(id, sendAtUtc));
        }

        public static MandrillMessageScheduleInfo CancelScheduled(this IMandrillMessagesApi api, string id)
        {
           return AsyncHelper.InvokeSync(api, messagesApi => messagesApi.CancelScheduledAsync(id));
        }
    }
}