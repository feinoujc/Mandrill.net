using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Mandrill.Serialization;
using Newtonsoft.Json;

namespace Mandrill.Http
{
    internal class MandrillJsonContent : HttpContent
    {
        protected MemoryStream Stream { get; private set; }

        public MandrillJsonContent(object value)
        {
            Stream = new MemoryStream();
            var jw = new JsonTextWriter(new StreamWriter(Stream));
            MandrillSerializer.Instance.Serialize(jw, value);
            jw.Flush();
            Stream.Seek(0, SeekOrigin.Begin);
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await Stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = Stream.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (Stream)
                {
                }
            }
            base.Dispose(disposing);
        }
    }
}