using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Mandrill.Model;
using Mandrill.Serialization;
using Newtonsoft.Json;

namespace Mandrill
{

    public class MandrillApi
    {
        private static readonly Uri BaseUrl = new Uri("https://mandrillapp.com/api/1.0/");

        private static readonly string UserAgent =
            $"Mandrill.net/{typeof (MandrillApi).Assembly.GetName().Version.ToString(3)}";

      
        private MandrillInboundApi _inbound;
        private MandrillMessagesApi _messages;
        private MandrillRejectsApi _rejects;
        private MandrillSendersApi _senders;
        private MandrillSubaccountsApi _subaccounts;
        private MandrillTagsApi _tags;
        private MandrillTemplatesApi _templates;
        private MandrillUsersApi _users;
        private IMandrillWebHooksApi _webhooks;
        private MandrillWhitelistsApi _whitelists;

        public MandrillApi(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            ApiKey = apiKey;
        }

        public string ApiKey { get; }
        public IMandrillMessagesApi Messages
        {
            get { return _messages ?? (_messages = new MandrillMessagesApi(this)); }
        }

        public IMandrillTagsApi Tags
        {
            get { return _tags ?? (_tags = new MandrillTagsApi(this)); }
        }

        public IMandrillTemplatesApi Templates
        {
            get { return _templates ?? (_templates = new MandrillTemplatesApi(this)); }
        }

        public IMandrillRejectsApi Rejects
        {
            get { return _rejects ?? (_rejects = new MandrillRejectsApi(this)); }
        }

        public IMandrillUsersApi Users
        {
            get { return _users ?? (_users = new MandrillUsersApi(this)); }
        }

        public IMandrillSendersApi Senders
        {
            get { return _senders ?? (_senders = new MandrillSendersApi(this)); }
        }

        public IMandrillWhitelistsApi Whitelists
        {
            get { return _whitelists ?? (_whitelists = new MandrillWhitelistsApi(this)); }
        }

        public IMandrillSubaccountsApi Subaccounts
        {
            get { return _subaccounts ?? (_subaccounts = new MandrillSubaccountsApi(this)); }
        }

        public IMandrillInboundApi Inbound
        {
            get { return _inbound ?? (_inbound = new MandrillInboundApi(this)); }
        }

        public IMandrillWebHooksApi WebHooks
        {
            get { return _webhooks ?? (_webhooks = new MandrillWebHooksApi(this)); }
        }

        internal async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest value)
            where TRequest : MandrillRequestBase
        {
            value.Key = ApiKey;

            try
            {
                var request = CreateHttpWebRequest(requestUri);
                using (var inputStream = new MemoryStream())
                using (var jsonWriter = new JsonTextWriter(new StreamWriter(inputStream)))
                {
                    MandrillSerializer.Instance.Serialize(jsonWriter, value);
                    jsonWriter.Flush();
                    inputStream.Seek(0, SeekOrigin.Begin);
                    using (var requestStream = await request.GetRequestStreamAsync())
                    {
                        await inputStream.CopyToAsync(requestStream);
                    }
                }
                using (var response = (HttpWebResponse) await request.GetResponseAsync())
                using (var responseStream = response.GetResponseStream())
                using (var jsonReader = new JsonTextReader(new StreamReader(responseStream)))
                {
                    return MandrillSerializer.Instance.Deserialize<TResponse>(jsonReader);
                }
            }
            catch (WebException webException)
            {
                throw ExtractMandrillErrorResponse(requestUri, webException);
            }
        }

        internal TResponse Post<TRequest, TResponse>(string requestUri, TRequest value)
            where TRequest : MandrillRequestBase
        {
            value.Key = ApiKey;
            var request = CreateHttpWebRequest(requestUri);
            using (var inputStream = new MemoryStream())
            using (var jw = new JsonTextWriter(new StreamWriter(inputStream)))
            {
                MandrillSerializer.Instance.Serialize(jw, value);
                jw.Flush();
                inputStream.Seek(0, SeekOrigin.Begin);
                using (var requestStream = request.GetRequestStream())
                {
                    inputStream.CopyTo(requestStream);
                }
            }

            try
            {
                using (var response = (HttpWebResponse) request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var jr = new JsonTextReader(new StreamReader(responseStream)))
                {
                    return MandrillSerializer.Instance.Deserialize<TResponse>(jr);
                }
            }
            catch (WebException webException)
            {
                throw ExtractMandrillErrorResponse(requestUri, webException);
            }
        }

        private static MandrillException ExtractMandrillErrorResponse(string requestUri, WebException webException)
        {
            MandrillErrorResponse error = null;
            var webResponse = webException.Response as HttpWebResponse;
            if (webResponse != null)
            {
                try
                {
                    using (var response = webResponse)
                    using (var responseStream = response.GetResponseStream())
                    using (var jsonReader = new JsonTextReader(new StreamReader(responseStream)))
                    {
                        error = MandrillSerializer.Instance.Deserialize<MandrillErrorResponse>(jsonReader);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                if (error != null)
                {
                    return new MandrillException(error, webException);
                }
            }
            return new MandrillException($"{requestUri} failed", webException);
        }

        protected virtual HttpWebRequest CreateHttpWebRequest(string relativeUri)
        {
            var request = WebRequest.CreateHttp(new Uri(BaseUrl, relativeUri));
            request.UserAgent = UserAgent;
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "POST";
            return request;
        }
    }
}