using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillTemplateInfo
    {
        private IList<string> _labels;
        public string Slug { get; set; }

        public string Name { get; set; }

        public IList<string> Labels
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

        public string PublishedAt { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }
    }
}