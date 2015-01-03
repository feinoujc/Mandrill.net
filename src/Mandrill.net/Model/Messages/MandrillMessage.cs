using System.Collections.Generic;

namespace Mandrill.Model
{
    public class MandrillMessage
    {
        private IList<string> _googleAnalyticsDomains;
        private IDictionary<string, string> _metadata;
        private IList<MandrillRecipientMetadata> _recipientMetadata;
        private IList<MandrillAttachment> _attachments;
        private IList<MandrillImage> _images;
        private IDictionary<string, string> _headers;
        private IList<string> _tags;
        private IList<MandrillRcptMergeVar> _mergeVars;
        private IList<MandrillMergeVar> _globalMergeVars;
        private IList<MandrillToAddress> _to;

        public string Html { get; set; }

        public string Text { get; set; }

        public string Subject { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public IList<MandrillToAddress> To
        {
            get { return _to ?? (_to = new List<MandrillToAddress>()); }
            set { _to = value; }
        }

        public IDictionary<string, string> Headers
        {
            get { return _headers ?? (_headers = new Dictionary<string, string>()); }
            set { _headers = value; }
        }

        public bool? Important { get; set; }

        public bool? TrackOpens { get; set; }

        public bool? TrackClicks { get; set; }

        public bool? AutoText { get; set; }

        public bool? AutoHtml { get; set; }

        public bool? InlineCss { get; set; }

        public bool? UrlStripQs { get; set; }

        public bool? PreserveRecipients { get; set; }

        public bool? ViewContentLink { get; set; }

        public string BccAddress { get; set; }

        public string TrackingDomain { get; set; }

        public string SigningDomain { get; set; }

        public string ReturnPathDomain { get; set; }

        public bool? Merge { get; set; }

        public string MergeLanguage { get; set; }

        public IList<MandrillMergeVar> GlobalMergeVars
        {
            get { return _globalMergeVars ?? (_globalMergeVars = new List<MandrillMergeVar>()); }
            set { _globalMergeVars = value; }
        }

        public IList<MandrillRcptMergeVar> MergeVars
        {
            get { return _mergeVars ?? (_mergeVars = new List<MandrillRcptMergeVar>()); }
            set { _mergeVars = value; }
        }

        public IList<string> Tags
        {
            get { return _tags ?? (_tags = new List<string>()); }
            set { _tags = value; }
        }

        public string Subaccount { get; set; }

        public IList<string> GoogleAnalyticsDomains
        {
            get { return _googleAnalyticsDomains ?? (_googleAnalyticsDomains = new List<string>()); }
            set { _googleAnalyticsDomains = value; }
        }

        public string GoogleAnalyticsCampaign { get; set; }

        public IDictionary<string, string> Metadata
        {
            get { return _metadata ?? (_metadata = new Dictionary<string, string>()); }
            set { _metadata = value; }
        }

        public IList<MandrillRecipientMetadata> RecipientMetadata
        {
            get { return _recipientMetadata ?? (_recipientMetadata = new List<MandrillRecipientMetadata>()); }
            set { _recipientMetadata = value; }
        }

        public IList<MandrillAttachment> Attachments
        {
            get { return _attachments ?? (_attachments = new List<MandrillAttachment>()); }
            set { _attachments = value; }
        }

        public IList<MandrillImage> Images
        {
            get { return _images ?? (_images = new List<MandrillImage>()); }
            set { _images = value; }
        }
    }
}