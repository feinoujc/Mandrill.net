using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace Mandrill.Model
{
    public class MandrillMessage
    {
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

        public List<MandrillMailAddress> To { get; set; } = new List<MandrillMailAddress>();

        public Dictionary<string, object> Headers { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

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

        public List<MandrillMergeVar> GlobalMergeVars { get; set; } = new List<MandrillMergeVar>();

        public List<MandrillRcptMergeVar> MergeVars { get; set; } = new List<MandrillRcptMergeVar>();

        public List<string> Tags  { get; set; } = new List<string>();

        public string Subaccount { get; set; }

        public List<string> GoogleAnalyticsDomains { get; set; } = new List<string>();

        public string GoogleAnalyticsCampaign { get; set; }

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public List<MandrillRcptMetadata> RecipientMetadata { get; set; } = new List<MandrillRcptMetadata>();

        public List<MandrillAttachment> Attachments { get; set; } = new List<MandrillAttachment>();

        public List<MandrillImage> Images { get; set; } = new List<MandrillImage>();

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
