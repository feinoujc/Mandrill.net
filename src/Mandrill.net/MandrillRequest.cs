using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Mandrill.Model;
using Mandrill.Serialization;
using Newtonsoft.Json;

namespace Mandrill
{
    internal abstract class MandrillRequest
    {
        protected static readonly Uri BaseUrl = new Uri("https://mandrillapp.com/api/1.0/");

        protected MandrillRequest(string apiKey)
        {
            ApiKey = apiKey;
        }

        protected string ApiKey { get; set; }

#if NET45
        public abstract TResponse Post<TRequest, TResponse>(string requestUri, TRequest value)
            where TRequest : MandrillRequestBase;
#endif

        public abstract Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest value)
            where TRequest : MandrillRequestBase;
    }


    internal class SystemWebMandrillRequest : MandrillRequest
    {
        private static readonly string UserAgent =
            $"Mandrill.net/{typeof (MandrillApi).GetTypeInfo().Assembly.GetName().Version.ToString(3)}";

        public SystemWebMandrillRequest(string apiKey) : base(apiKey)
        {
        }
#if NET45
        public override TResponse Post<TRequest, TResponse>(string requestUri, TRequest value)
        {
            value.Key = ApiKey;

            var request = CreateHttpWebRequest(requestUri);
            using (var inputStream = new MemoryStream())
            using (var jsonWriter = new JsonTextWriter(new StreamWriter(inputStream)))
            {
                MandrillSerializer<TRequest>.Serialize(jsonWriter, value);
                jsonWriter.Flush();
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
                using (var jsonReader = new JsonTextReader(new StreamReader(responseStream)))
                {
                    return MandrillSerializer<TResponse>.Deserialize(jsonReader);
                }
            }
            catch (WebException webException)
            {
                throw ExtractMandrillErrorResponse(requestUri, webException);
            }
        }
#endif
        public override async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest value)
        {
            value.Key = ApiKey;

            var request = CreateHttpWebRequest(requestUri);
            using (var inputStream = new MemoryStream())
            using (var jsonWriter = new JsonTextWriter(new StreamWriter(inputStream)))
            {
                MandrillSerializer<TRequest>.Serialize(jsonWriter, value);
                jsonWriter.Flush();
                inputStream.Seek(0, SeekOrigin.Begin);
                using (var requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
                {
                    await inputStream.CopyToAsync(requestStream).ConfigureAwait(false);
                }
            }

            try
            {
                using (var response = (HttpWebResponse) await request.GetResponseAsync().ConfigureAwait(false))
                using (var responseStream = response.GetResponseStream())
                using(var responseReader = new StreamReader(responseStream))
                using (var jsonReader = new JsonTextReader(responseReader))
                {
                    return MandrillSerializer<TResponse>.Deserialize(jsonReader);
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
                    using (var responseReader = new StreamReader(responseStream))
                    using (var jsonReader = new JsonTextReader(responseReader))
                    {
                        error = MandrillSerializer<MandrillErrorResponse>.Deserialize(jsonReader);
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
#if NET45
            request.UserAgent = UserAgent;
#endif
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "POST";
            return request;
        }
    }

}