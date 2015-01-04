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
            return api.SendAsync(message, async, ipPool, sendAtUtc).Result;
        }

        public static IList<MandrillSendMessageResponse> SendTemplate(this IMandrillMessagesApi api, MandrillMessage message,
            string templateName, IList<MandrillTemplateContent> templateContent = null, bool async = false, string ipPool = null,
            DateTime? sendAtUtc = null)
        {
            return api.SendTemplateAsync(message, templateName, templateContent, async, ipPool, sendAtUtc).Result;
        }

        public static IList<MandrillSendMessageResponse>
            SendRaw(this IMandrillMessagesApi api, string rawMessage,
                string fromEmail = null, string fromName = null, IList<string> to = null, bool? async = null,
                string ipPool = null, DateTime? sendAtUtc = null, string returnPathDomain = null)
        {
            return api.SendRawAsync(rawMessage, fromEmail, fromName, to, async, ipPool, sendAtUtc, returnPathDomain).Result;
        }

        public static IList<MandrillMessageInfo> Search(this IMandrillMessagesApi api, string query, DateTime? dateFrom = null, DateTime?
            dateTo = null, IList<string> tags = null, IList<string> senders = null, IList<string> apiKeys = null, int? limit = null)

        {
            return api.SearchAsync(query, dateFrom, dateTo, tags, senders, apiKeys, limit).Result;
        }

        public static IList<MandrillMessageTimeSeries> SearchTimeSeries(this IMandrillMessagesApi api, string query,
            DateTime? dateFrom = null, DateTime? dateTo = null, IList<string> tags = null, IList<string> senders = null)
        {
            return api.SearchTimeSeriesAsync(query, dateFrom, dateTo, tags, senders).Result;
        }

        public static MandrillMessageInfo Info(this IMandrillMessagesApi api, string id)
        {
            return api.InfoAync(id).Result;
        }

        public static MandrillMessageContent Content(this IMandrillMessagesApi api, string id)
        {
            return api.ContentAsync(id).Result;
        }

        public static MandrillMessage Parse(this IMandrillMessagesApi api, string rawMessage)
        {
            return api.ParseAsync(rawMessage).Result;
        }

        public static IList<MandrillMessageScheduleInfo> ListScheduled(this IMandrillMessagesApi api, string to = null)
        {
            return api.ListScheduledAsync(to).Result;
        }

        public static MandrillMessageScheduleInfo Reschedule(this IMandrillMessagesApi api, string id, DateTime sendAtUtc)
        {
            return api.RescheduleAsync(id, sendAtUtc).Result;
        }

        public static MandrillMessageScheduleInfo CancelScheduled(this IMandrillMessagesApi api, string id)
        {
            return api.CancelScheduledAsync(id).Result;
        }
    }
}