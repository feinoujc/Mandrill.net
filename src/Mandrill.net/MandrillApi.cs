using System;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public class MandrillApi
    {
        private readonly MandrillRequest _request;

        private MandrillExportsApi _exports;
        private MandrillInboundApi _inbound;
        private MandrillMessagesApi _messages;
        private MandrillRejectsApi _rejects;
        private MandrillSendersApi _senders;
        private MandrillSubaccountsApi _subaccounts;
        private MandrillTagsApi _tags;
        private MandrillTemplatesApi _templates;
        private MandrillUsersApi _users;
        private MandrillWebHooksApi _webhooks;
        private MandrillWhitelistsApi _whitelists;

        public MandrillApi(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            _request = new SystemWebMandrillRequest(apiKey);
            ApiKey = apiKey;
        }

        public string ApiKey { get; private set; }
        public IMandrillMessagesApi Messages => _messages ?? (_messages = new MandrillMessagesApi(this));

        public IMandrillTagsApi Tags => _tags ?? (_tags = new MandrillTagsApi(this));

        public IMandrillTemplatesApi Templates => _templates ?? (_templates = new MandrillTemplatesApi(this));

        public IMandrillRejectsApi Rejects => _rejects ?? (_rejects = new MandrillRejectsApi(this));

        public IMandrillUsersApi Users => _users ?? (_users = new MandrillUsersApi(this));

        public IMandrillSendersApi Senders => _senders ?? (_senders = new MandrillSendersApi(this));

        public IMandrillWhitelistsApi Whitelists => _whitelists ?? (_whitelists = new MandrillWhitelistsApi(this));

        public IMandrillSubaccountsApi Subaccounts => _subaccounts ?? (_subaccounts = new MandrillSubaccountsApi(this));

        public IMandrillInboundApi Inbound => _inbound ?? (_inbound = new MandrillInboundApi(this));

        public IMandrillWebHooksApi WebHooks => _webhooks ?? (_webhooks = new MandrillWebHooksApi(this));

        public IMandrillExportsApi Exports => _exports ?? (_exports = new MandrillExportsApi(this));

        internal Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest value)
            where TRequest : MandrillRequestBase
        {
            return _request.PostAsync<TRequest, TResponse>(requestUri, value);
        }

#if !NETSTANDARD13
        internal TResponse Post<TRequest, TResponse>(string requestUri, TRequest value)
            where TRequest : MandrillRequestBase
        {
            return _request.Post<TRequest, TResponse>(requestUri, value);
        }
#endif
    }
}
