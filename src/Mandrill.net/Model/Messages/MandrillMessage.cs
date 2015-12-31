using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace Mandrill.Model
{
    public class MandrillMessage
    {
        private List<MandrillAttachment> _attachments;
        private List<MandrillMergeVar> _globalMergeVars;
        private List<string> _googleAnalyticsDomains;
        private Dictionary<string, object> _headers;
        private List<MandrillImage> _images;
        private List<MandrillRcptMergeVar> _mergeVars;
        private Dictionary<string, string> _metadata;
        private List<MandrillRcptMetadata> _recipientMetadata;
        private List<string> _tags;
        private List<MandrillMailAddress> _to;

        public MandrillMessage()
        {
        }

        public MandrillMessage(MandrillMailAddress from, MandrillMailAddress to)
        {
            FromEmail = from.Email;
            FromName = from.Name;
            To.Add(to);
        }

        public MandrillMessage(string from, string to) : this(new MandrillMailAddress(from), new MandrillMailAddress(to))
        {
        }

        public MandrillMessage(string from, string to, string subject, string body) : this(from, to)
        {
            Subject = subject;
            if (body != null)
            {
                if (body.StartsWith("<"))
                {
                    Html = body;
                }
                else
                {
                    Text = body;
                }
            }
        }

        public string Html { get; set; }

        public string Text { get; set; }

        public string Subject { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public List<MandrillMailAddress> To
        {
            get { return _to ?? (_to = new List<MandrillMailAddress>()); }
            set { _to = value; }
        }

        public Dictionary<string, object> Headers
        {
            get { return _headers ?? (_headers = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)); }
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

        public MandrillMessageMergeLanguage? MergeLanguage { get; set; }

        public List<MandrillMergeVar> GlobalMergeVars
        {
            get { return _globalMergeVars ?? (_globalMergeVars = new List<MandrillMergeVar>()); }
            set { _globalMergeVars = value; }
        }

        public List<MandrillRcptMergeVar> MergeVars
        {
            get { return _mergeVars ?? (_mergeVars = new List<MandrillRcptMergeVar>()); }
            set { _mergeVars = value; }
        }

        public List<string> Tags
        {
            get { return _tags ?? (_tags = new List<string>()); }
            set { _tags = value; }
        }

        public string Subaccount { get; set; }

        public List<string> GoogleAnalyticsDomains
        {
            get { return _googleAnalyticsDomains ?? (_googleAnalyticsDomains = new List<string>()); }
            set { _googleAnalyticsDomains = value; }
        }

        public string GoogleAnalyticsCampaign { get; set; }

        public Dictionary<string, string> Metadata
        {
            get { return _metadata ?? (_metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)); }
            set { _metadata = value; }
        }

        public List<MandrillRcptMetadata> RecipientMetadata
        {
            get { return _recipientMetadata ?? (_recipientMetadata = new List<MandrillRcptMetadata>()); }
            set { _recipientMetadata = value; }
        }

        public List<MandrillAttachment> Attachments
        {
            get { return _attachments ?? (_attachments = new List<MandrillAttachment>()); }
            set { _attachments = value; }
        }

        public List<MandrillImage> Images
        {
            get { return _images ?? (_images = new List<MandrillImage>()); }
            set { _images = value; }
        }

        [JsonIgnore]
        public string ReplyTo
        {
            get
            {
                object value;
                if (Headers.TryGetValue("Reply-To", out value))
                {
                    return value as string;
                }
                return null;
            }
            set { Headers["Reply-To"] = value; }
        }

        public void AddTo(string email)
        {
            AddTo(email, null, null);
        }

        public void AddTo(string email, string name)
        {
            AddTo(email, name, null);
        }

        public void AddTo(string email, string name, MandrillMailAddressType? type)
        {
            To.Add(new MandrillMailAddress(email, name) {Type = type});
        }

        public void AddGlobalMergeVars(string name, string content)
        {
            GlobalMergeVars.Add(new MandrillMergeVar {Name = name, Content = content});
        }

        public void AddGlobalMergeVars(string name, dynamic content)
        {
            GlobalMergeVars.Add(new MandrillMergeVar
            {
                Name = name,
                Content = content
            });
        }
        
        public void AddRcptMergeVars(string rcptEmail, string name, string content)
        {
            var mergeVar = MergeVars.FirstOrDefault(x => x.Rcpt == rcptEmail);
            if (mergeVar == null)
            {
                MergeVars.Add(mergeVar = new MandrillRcptMergeVar {Rcpt = rcptEmail});
            }
            mergeVar.Vars.Add(new MandrillMergeVar
            {
                Name = name,
                Content = content
            });
        }

        public void AddRcptMergeVars(string rcptEmail, string name, dynamic content)
        {
            var mergeVar = MergeVars.FirstOrDefault(x => x.Rcpt == rcptEmail);
            if (mergeVar == null)
            {
                MergeVars.Add(mergeVar = new MandrillRcptMergeVar {Rcpt = rcptEmail});
            }
            mergeVar.Vars.Add(new MandrillMergeVar
            {
                Name = name,
                Content = content
            });
        }

        public void AddMetadata(string key, string value)
        {
            Metadata[key] = value;
        }

        public void AddRecipientMetadata(string rcptEmail, string key, string value)
        {
            var metadata = RecipientMetadata.FirstOrDefault(x => x.Rcpt == rcptEmail);
            if (metadata == null)
            {
                RecipientMetadata.Add(metadata = new MandrillRcptMetadata {Rcpt = rcptEmail});
            }
            metadata.Values[key] = value;
        }

        public void AddHeader(string key, string value)
        {
            Headers[key] = value;
        }

        public void AddHeader(string key, IEnumerable<string> values)
        {
            Headers[key] = values.ToArray();
        }
    }
}