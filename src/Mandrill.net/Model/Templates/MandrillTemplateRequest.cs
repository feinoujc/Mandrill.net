using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillTemplateRequest : MandrillRequestBase
    {
        public string Name { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string Subject { get; set; }

        public string Code { get; set; }

        public string Text { get; set; }

        public bool Publish { get; set; }

        public IList<string> Labels { get; set; }
    }
}