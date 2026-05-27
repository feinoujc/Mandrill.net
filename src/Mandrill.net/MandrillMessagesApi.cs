#nullable enable
#pragma warning disable CS8618
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill;

internal partial class MandrillMessagesApi
{
    public Task<IList<MandrillSendMessageResponse>> SendAsync(MandrillMessage message, bool? async = default, string? ipPool = default, string? sendAt = default, CancellationToken cancellationToken = default)
    {
        return MandrillApi.PostAsync<MessagesSendRequest, IList<MandrillSendMessageResponse>>("messages/send.json", new MessagesSendRequest
        {
            Message = message,
            Async = async,
            IpPool = ipPool,
            SendAt = sendAt,
        }, cancellationToken);
    }

    public Task<IList<MandrillSendMessageResponse>> SendTemplateAsync(string templateName, IList<MandrillTemplateContent> templateContent, MandrillMessage message, bool? async = default, string? ipPool = default, string? sendAt = default, CancellationToken cancellationToken = default)
    {
        return MandrillApi.PostAsync<MessagesSendTemplateRequest, IList<MandrillSendMessageResponse>>("messages/send-template.json", new MessagesSendTemplateRequest
        {
            TemplateName = templateName,
            TemplateContent = templateContent,
            Message = message,
            Async = async,
            IpPool = ipPool,
            SendAt = sendAt,
        }, cancellationToken);
    }

    public Task<IList<MandrillMessageInfo>> SearchAsync(string? query = default, string? dateFrom = default, string? dateTo = default, IList<string>? tags = default, IList<string>? senders = default, IList<string>? apiKeys = default, int? limit = default, CancellationToken cancellationToken = default)
    {
        return MandrillApi.PostAsync<MessagesSearchRequest, IList<MandrillMessageInfo>>("messages/search.json", new MessagesSearchRequest
        {
            Query = query,
            DateFrom = dateFrom,
            DateTo = dateTo,
            Tags = tags,
            Senders = senders,
            ApiKeys = apiKeys,
            Limit = limit,
        }, cancellationToken);
    }

    public Task<IList<MandrillMessageTimeSeries>> SearchTimeSeriesAsync(string? query = default, string? dateFrom = default, string? dateTo = default, IList<string>? tags = default, IList<string>? senders = default, CancellationToken cancellationToken = default)
    {
        return MandrillApi.PostAsync<MessagesSearchTimeSeriesRequest, IList<MandrillMessageTimeSeries>>("messages/search-time-series.json", new MessagesSearchTimeSeriesRequest
        {
            Query = query,
            DateFrom = dateFrom,
            DateTo = dateTo,
            Tags = tags,
            Senders = senders,
        }, cancellationToken);
    }

    public Task<MandrillMessageInfo> ParseAsync(string rawMessage, CancellationToken cancellationToken = default)
    {
        return MandrillApi.PostAsync<MessagesParseRequest, MandrillMessageInfo>("messages/parse.json", new MessagesParseRequest
        {
            RawMessage = rawMessage,
        }, cancellationToken);
    }

    public Task<IList<MandrillSendMessageResponse>> SendRawAsync(string rawMessage, string? fromEmail = default, string? fromName = default, IList<string>? to = default, bool? async = default, string? ipPool = default, string? sendAt = default, string? returnPathDomain = default, CancellationToken cancellationToken = default)
    {
        return MandrillApi.PostAsync<MessagesSendRawRequest, IList<MandrillSendMessageResponse>>("messages/send-raw.json", new MessagesSendRawRequest
        {
            RawMessage = rawMessage,
            FromEmail = fromEmail,
            FromName = fromName,
            To = to,
            Async = async,
            IpPool = ipPool,
            SendAt = sendAt,
            ReturnPathDomain = returnPathDomain,
        }, cancellationToken);
    }

    public Task<MandrillMessageScheduleInfo> RescheduleAsync(string id, string sendAt, CancellationToken cancellationToken = default)
    {
        return MandrillApi.PostAsync<MessagesRescheduleRequest, MandrillMessageScheduleInfo>("messages/reschedule.json", new MessagesRescheduleRequest
        {
            Id = id,
            SendAt = sendAt,
        }, cancellationToken);
    }

    internal class MessagesSendRequest : MandrillRequestBase
    {
        [JsonPropertyName("message")]
        public MandrillMessage Message { get; set; }

        [JsonPropertyName("async")]
        public bool? Async { get; set; }

        [JsonPropertyName("ip_pool")]
        public string? IpPool { get; set; }

        [JsonPropertyName("send_at")]
        public string? SendAt { get; set; }
    }

    internal class MessagesSendTemplateRequest : MandrillRequestBase
    {
        [JsonPropertyName("template_name")]
        public string TemplateName { get; set; }

        [JsonPropertyName("template_content")]
        public IList<MandrillTemplateContent> TemplateContent { get; set; } = new List<MandrillTemplateContent>();

        [JsonPropertyName("message")]
        public MandrillMessage Message { get; set; }

        [JsonPropertyName("async")]
        public bool? Async { get; set; }

        [JsonPropertyName("ip_pool")]
        public string? IpPool { get; set; }

        [JsonPropertyName("send_at")]
        public string? SendAt { get; set; }

        public bool ShouldSerializeTemplateContent() => true;
    }

    internal class MessagesSearchRequest : MandrillRequestBase
    {
        [JsonPropertyName("query")]
        public string? Query { get; set; }

        [JsonPropertyName("date_from")]
        public string? DateFrom { get; set; }

        [JsonPropertyName("date_to")]
        public string? DateTo { get; set; }

        [JsonPropertyName("tags")]
        public IList<string>? Tags { get; set; }

        [JsonPropertyName("senders")]
        public IList<string>? Senders { get; set; }

        [JsonPropertyName("api_keys")]
        public IList<string>? ApiKeys { get; set; }

        [JsonPropertyName("limit")]
        public int? Limit { get; set; }
    }

    internal class MessagesSearchTimeSeriesRequest : MandrillRequestBase
    {
        [JsonPropertyName("query")]
        public string? Query { get; set; }

        [JsonPropertyName("date_from")]
        public string? DateFrom { get; set; }

        [JsonPropertyName("date_to")]
        public string? DateTo { get; set; }

        [JsonPropertyName("tags")]
        public IList<string>? Tags { get; set; }

        [JsonPropertyName("senders")]
        public IList<string>? Senders { get; set; }
    }

    internal class MessagesParseRequest : MandrillRequestBase
    {
        [JsonPropertyName("raw_message")]
        public string RawMessage { get; set; }
    }

    internal class MessagesSendRawRequest : MandrillRequestBase
    {
        [JsonPropertyName("raw_message")]
        public string RawMessage { get; set; }

        [JsonPropertyName("from_email")]
        public string? FromEmail { get; set; }

        [JsonPropertyName("from_name")]
        public string? FromName { get; set; }

        [JsonPropertyName("to")]
        public IList<string>? To { get; set; }

        [JsonPropertyName("async")]
        public bool? Async { get; set; }

        [JsonPropertyName("ip_pool")]
        public string? IpPool { get; set; }

        [JsonPropertyName("send_at")]
        public string? SendAt { get; set; }

        [JsonPropertyName("return_path_domain")]
        public string? ReturnPathDomain { get; set; }
    }

    internal class MessagesRescheduleRequest : MandrillRequestBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("send_at")]
        public string SendAt { get; set; }
    }
}
