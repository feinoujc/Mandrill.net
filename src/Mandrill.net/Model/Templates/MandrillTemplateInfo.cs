using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mandrill.Model
{
    public class MandrillTemplateInfo
    {
        private List<string> _labels;
        public string Slug { get; set; }

        public string Name { get; set; }

        public List<string> Labels
        {
            get { return _labels ?? (_labels = new List<string>()); }
            set { _labels = value; }
        }

        public string Code { get; set; }

        public string Subject { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string Text { get; set; }

        public string PublishName { get; set; }

        public string PublishCode { get; set; }

        public string PublishSubject { get; set; }

        public string PublishFromEmail { get; set; }

        public string PublishFromName { get; set; }

        public string PublishText { get; set; }

        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime? PublishedAt { get; set; }

        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime? CreatedAt { get; set; }

        [JsonConverter(typeof (IsoDateTimeConverter))]
        public DateTime? UpdatedAt { get; set; }
    }
}