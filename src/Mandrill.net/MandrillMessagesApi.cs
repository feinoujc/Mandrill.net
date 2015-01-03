using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;
using Mandrill.Model;

namespace Mandrill
{
    public class MandrillMessagesApi
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

        public async Task<IList<MandrillMessageInfo>> SearchAsync(string query, DateTime? dateFrom = null, DateTime? dateTo = null, IList<string> tags = null, IList<string> senders = null,
            IList<string> apiKeys = null, int? limit = null)
        {
            return await MandrillApi.PostAsync<MandrillMessageSearchRequest, List<MandrillMessageInfo>>("messages/search.json",
                new MandrillMessageSearchRequest()
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

        public async Task<MandrillMessageInfo> Info(Guid id)
        {
            return await MandrillApi.PostAsync<MandrillMessageInfoRequest, MandrillMessageInfo>("messages/info.json",
                new MandrillMessageInfoRequest()
                {
                    Id = id
                });
        }

        public async Task<MandrillMessageContent> Content(Guid id)
        {
            return await MandrillApi.PostAsync<MandrillMessageInfoRequest, MandrillMessageContent>("messages/content.json",
                new MandrillMessageInfoRequest()
                {
                    Id = id
                });
        }
    }
}