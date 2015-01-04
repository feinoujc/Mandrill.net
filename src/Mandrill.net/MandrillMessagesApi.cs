using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    class MandrillMessagesApi : IMandrillMessagesApi
    {
        private const string SendAtDateFormat = "yyyy-MM-dd HH:mm:ss";
        private const string SearchDateFormat = "yyyy-MM-dd";

        internal MandrillMessagesApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public async Task<IList<MandrillSendMessageResponse>> SendAsync(MandrillMessage message, bool async = false, string ipPool = null, DateTime? sendAtUtc = null)
        {
            if (message == null) throw new ArgumentNullException("message");

            if (sendAtUtc != null && sendAtUtc.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("date must be in utc", "sendAtUtc");
            }

            return await MandrillApi.PostAsync<MandrillSendMessageRequest, List<MandrillSendMessageResponse>>("messages/send.json", new MandrillSendMessageRequest
            {
                Message = message,
                Async = async,
                IpPool = ipPool,
                SendAt = sendAtUtc != null ? sendAtUtc.Value.ToString(SendAtDateFormat) : null
            });
        }

        public async Task<IList<MandrillSendMessageResponse>> SendTemplateAsync(MandrillMessage message, string templateName, IList<MandrillTemplateContent> templateContent = null, bool async = false,
            string ipPool = null, DateTime? sendAtUtc = null)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (templateName == null) throw new ArgumentNullException("templateName");

            if (sendAtUtc != null && sendAtUtc.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("date must be in utc", "sendAtUtc");
            }

            return await MandrillApi.PostAsync<MandrillSendMessageRequest, List<MandrillSendMessageResponse>>("messages/send-template.json",
                new MandrillSendTemplateRequest
                {
                    Message = message,
                    TemplateName = templateName,
                    TemplateContent = templateContent,
                    Async = async,
                    IpPool = ipPool,
                    SendAt = sendAtUtc != null ? sendAtUtc.Value.ToString(SendAtDateFormat) : null
                });
        }

        public async Task<IList<MandrillSendMessageResponse>> SendRawAsync(string rawMessage, string fromEmail = null, string fromName = null, IList<string> to = null, bool? async = null,
            string ipPool = null, DateTime? sendAtUtc = null, string returnPathDomain = null)
        {
            if (rawMessage == null) throw new ArgumentNullException("rawMessage");
            return await MandrillApi.PostAsync<MandrillSendRawMessageRequest, List<MandrillSendMessageResponse>>("messages/send-raw.json",
                new MandrillSendRawMessageRequest
                {
                    RawMessage = rawMessage,
                    FromEmail = fromEmail,
                    FromName = fromName,
                    To = to,
                    Async = async,
                    IpPool = ipPool,
                    SendAt = sendAtUtc != null ? sendAtUtc.Value.ToString(SendAtDateFormat) : null,
                    ReturnPathDomain = returnPathDomain
                });
        }


        public async Task<IList<MandrillMessageInfo>> SearchAsync(string query, DateTime? dateFrom = null, DateTime? dateTo = null, IList<string> tags = null, IList<string> senders = null,
            IList<string> apiKeys = null, int? limit = null)
        {
            return await MandrillApi.PostAsync<MandrillMessageSearchRequest, List<MandrillMessageInfo>>("messages/search.json",
                new MandrillMessageSearchRequest
                {
                    DateFrom = dateFrom != null ? dateFrom.Value.ToString(SearchDateFormat) : null,
                    DateTo = dateTo != null ? dateTo.Value.ToString(SearchDateFormat) : null,
                    Query = query,
                    Tags = tags,
                    Senders = senders,
                    ApiKeys = apiKeys,
                    Limit = limit
                });
        }


        public async Task<IList<MandrillMessageTimeSeries>> SearchTimeSeriesAsync(string query, DateTime? dateFrom = null, DateTime? dateTo = null, IList<string> tags = null,
            IList<string> senders = null)
        {
            return await MandrillApi.PostAsync<MandrillMessageSearchRequest, List<MandrillMessageTimeSeries>>("messages/search-time-series.json",
                new MandrillMessageSearchRequest
                {
                    DateFrom = dateFrom != null ? dateFrom.Value.ToString(SearchDateFormat) : null,
                    DateTo = dateTo != null ? dateTo.Value.ToString(SearchDateFormat) : null,
                    Query = query,
                    Tags = tags,
                    Senders = senders
                });
        }

        public async Task<MandrillMessageInfo> InfoAync(string id)
        {
            return await MandrillApi.PostAsync<MandrillMessageInfoRequest, MandrillMessageInfo>("messages/info.json",
                new MandrillMessageInfoRequest
                {
                    Id = id
                });
        }

        public async Task<MandrillMessageContent> ContentAsync(string id)
        {
            return await MandrillApi.PostAsync<MandrillMessageInfoRequest, MandrillMessageContent>("messages/content.json",
                new MandrillMessageInfoRequest
                {
                    Id = id
                });
        }

        public async Task<MandrillMessage> ParseAsync(string rawMessage)
        {
            if (rawMessage == null) throw new ArgumentNullException("rawMessage");

            return await MandrillApi.PostAsync<MandrillSendRawMessageRequest, MandrillMessage>("messages/parse.json",
                new MandrillSendRawMessageRequest
                {
                    RawMessage = rawMessage
                });
        }

        public async Task<IList<MandrillMessageScheduleInfo>> ListScheduledAsync(string to = null)
        {
            return await MandrillApi.PostAsync<MandrillScheduleRequest, IList<MandrillMessageScheduleInfo>>("messages/list-scheduled.json",
                new MandrillScheduleRequest
                {
                    To = to
                });
        }

        public async Task<MandrillMessageScheduleInfo> RescheduleAsync(string id, DateTime sendAtUtc)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (sendAtUtc.Kind != DateTimeKind.Utc) throw new ArgumentException("date must be in utc", "sendAtUtc");

            return await MandrillApi.PostAsync<MandrillScheduleRequest, MandrillMessageScheduleInfo>("messages/reschedule.json",
                new MandrillScheduleRequest
                {
                    Id = id,
                    SendAt = sendAtUtc.ToString(SendAtDateFormat)
                });
        }

        public async Task<MandrillMessageScheduleInfo> CancelScheduledAsync(string id)
        {
            if (id == null) throw new ArgumentNullException("id");

            return await MandrillApi.PostAsync<MandrillScheduleRequest, MandrillMessageScheduleInfo>("messages/cancel-scheduled.json",
                new MandrillScheduleRequest
                {
                    Id = id,
                });
        }
    }
}