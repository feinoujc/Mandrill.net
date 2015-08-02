using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mandrill.Model
{
    public class MandrillMessageContent
    {
        private List<string> _tags;
        private Dictionary<string, object> _headers;
        private List<MandrillAttachment> _attachments;

        public DateTime Ts { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string Subject { get; set; }

        public MandrillMailAddress To { get; set; }

        public List<string> Tags
        {
            get { return _tags ?? (_tags = new List<string>()); }
            set { _tags = value; }
        }

        public Dictionary<string, object> Headers
        {
            get { return _headers ?? (_headers = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)); }
            set { _headers = value; }
        }

        public string Text { get; set; }

        public string Html { get; set; }

        public List<MandrillAttachment> Attachments
        {
            get { return _attachments ?? (_attachments = new List<MandrillAttachment>()); }
            set { _attachments = value; }
        }
    }
}