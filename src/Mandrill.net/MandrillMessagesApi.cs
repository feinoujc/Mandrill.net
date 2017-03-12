using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    internal partial class MandrillMessagesApi : IMandrillMessagesApi
    {
        private const string SendAtDateFormat = "yyyy-MM-dd HH:mm:ss";
        private const string SearchDateFormat = "yyyy-MM-dd";

        internal MandrillMessagesApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillSendMessageResponse>> SendAsync(MandrillMessage message, bool async = false,
            string ipPool = null, DateTime? sendAtUtc = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (sendAtUtc != null && sendAtUtc.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("date must be in utc", nameof(sendAtUtc));
            }

            return
                MandrillApi.PostAsync<MandrillSendMessageRequest, IList<MandrillSendMessageResponse>>(
                    "messages/send.json", new MandrillSendMessageRequest
                    {
                        Message = message,
                        Async = async,
                        IpPool = ipPool,
                        SendAt = sendAtUtc?.ToString(SendAtDateFormat)
                    });
        }

        public Task<IList<MandrillSendMessageResponse>> SendTemplateAsync(MandrillMessage message, string templateName,
            IList<MandrillTemplateContent> templateContent = null, bool async = false,
            string ipPool = null, DateTime? sendAtUtc = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (templateName == null) throw new ArgumentNullException(nameof(templateName));

            if (sendAtUtc != null && sendAtUtc.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("date must be in utc", nameof(sendAtUtc));
            }

            return
                MandrillApi.PostAsync<MandrillSendMessageRequest, IList<MandrillSendMessageResponse>>(
                    "messages/send-template.json",
                    new MandrillSendTemplateRequest
                    {
                        Message = message,
                        TemplateName = templateName,
                        TemplateContent = templateContent?.ToList(),
                        Async = async,
                        IpPool = ipPool,
                        SendAt = sendAtUtc?.ToString(SendAtDateFormat)
                    });
        }

        public Task<IList<MandrillSendMessageResponse>> SendRawAsync(string rawMessage, string fromEmail = null,
            string fromName = null, IList<string> to = null, bool? async = null,
            string ipPool = null, DateTime? sendAtUtc = null, string returnPathDomain = null)
        {
            if (rawMessage == null) throw new ArgumentNullException(nameof(rawMessage));
            return
                MandrillApi.PostAsync<MandrillSendRawMessageRequest, IList<MandrillSendMessageResponse>>(
                    "messages/send-raw.json",
                    new MandrillSendRawMessageRequest
                    {
                        RawMessage = rawMessage,
                        FromEmail = fromEmail,
                        FromName = fromName,
                        To = to?.ToList(),
                        Async = async,
                        IpPool = ipPool,
                        SendAt = sendAtUtc?.ToString(SendAtDateFormat),
                        ReturnPathDomain = returnPathDomain
                    });
        }


        public Task<IList<MandrillMessageInfo>> SearchAsync(string query, DateTime? dateFrom = null,
            DateTime? dateTo = null, IList<string> tags = null, IList<string> senders = null,
            IList<string> apiKeys = null, int? limit = null)
        {
            return
                MandrillApi.PostAsync<MandrillMessageSearchRequest, IList<MandrillMessageInfo>>("messages/search.json",
                    new MandrillMessageSearchRequest
                    {
                        DateFrom = dateFrom?.ToString(SearchDateFormat),
                        DateTo = dateTo?.ToString(SearchDateFormat),
                        Query = query,
                        Tags = tags?.ToList(),
                        Senders = senders?.ToList(),
                        ApiKeys = apiKeys?.ToList(),
                        Limit = limit
                    });
        }


        public Task<IList<MandrillMessageTimeSeries>> SearchTimeSeriesAsync(string query, DateTime? dateFrom = null,
            DateTime? dateTo = null, IList<string> tags = null,
            IList<string> senders = null)
        {
            return
                MandrillApi.PostAsync<MandrillMessageSearchRequest, IList<MandrillMessageTimeSeries>>(
                    "messages/search-time-series.json",
                    new MandrillMessageSearchRequest
                    {
                        DateFrom = dateFrom?.ToString(SearchDateFormat),
                        DateTo = dateTo?.ToString(SearchDateFormat),
                        Query = query,
                        Tags = tags?.ToList(),
                        Senders = senders?.ToList()
                    });
        }

        public Task<MandrillMessageInfo> InfoAsync(string id)
        {
            return MandrillApi.PostAsync<MandrillMessageInfoRequest, MandrillMessageInfo>("messages/info.json",
                new MandrillMessageInfoRequest
                {
                    Id = id
                });
        }

        public Task<MandrillMessageContent> ContentAsync(string id)
        {
            return MandrillApi.PostAsync<MandrillMessageInfoRequest, MandrillMessageContent>("messages/content.json",
                new MandrillMessageInfoRequest
                {
                    Id = id
                });
        }

        public Task<MandrillMessage> ParseAsync(string rawMessage)
        {
            if (rawMessage == null) throw new ArgumentNullException(nameof(rawMessage));

            return MandrillApi.PostAsync<MandrillSendRawMessageRequest, MandrillMessage>("messages/parse.json",
                new MandrillSendRawMessageRequest
                {
                    RawMessage = rawMessage
                });
        }

        public Task<IList<MandrillMessageScheduleInfo>> ListScheduledAsync(string to = null)
        {
            return
                MandrillApi.PostAsync<MandrillScheduleRequest, IList<MandrillMessageScheduleInfo>>(
                    "messages/list-scheduled.json",
                    new MandrillScheduleRequest
                    {
                        To = to
                    });
        }

        public Task<MandrillMessageScheduleInfo> RescheduleAsync(string id, DateTime sendAtUtc)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (sendAtUtc.Kind != DateTimeKind.Utc)
                throw new ArgumentException("date must be in utc", nameof(sendAtUtc));

            return
                MandrillApi.PostAsync<MandrillScheduleRequest, MandrillMessageScheduleInfo>("messages/reschedule.json",
                    new MandrillScheduleRequest
                    {
                        Id = id,
                        SendAt = sendAtUtc.ToString(SendAtDateFormat)
                    });
        }

        public Task<MandrillMessageScheduleInfo> CancelScheduledAsync(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            return
                MandrillApi.PostAsync<MandrillScheduleRequest, MandrillMessageScheduleInfo>(
                    "messages/cancel-scheduled.json",
                    new MandrillScheduleRequest
                    {
                        Id = id,
                    });
        }
    }
}
