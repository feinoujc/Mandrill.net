using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillMessageContent
    {
        private IList<string> _tags;
        private IDictionary<string, string> _headers;
        private IList<MandrillAttachment> _attachments;

        public DateTime Ts { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string Subject { get; set; }

        public MandrillToAddress To { get; set; }

        public IList<string> Tags
        {
            get { return _tags ?? (_tags = new List<string>()); }
            set { _tags = value; }
        }

        public IDictionary<string, string> Headers
        {
            get { return _headers ?? (_headers = new Dictionary<string, string>()); }
            set { _headers = value; }
        }

        public string Text { get; set; }

        public string Html { get; set; }

        public IList<MandrillAttachment> Attachments
        {
            get { return _attachments ?? (_attachments = new List<MandrillAttachment>()); }
            set { _attachments = value; }
        }
    }
}