using System.Collections.Generic;

namespace Mandrill.Model
{
    class MandrillSendTemplateRequest:MandrillSendMessageRequest
    {
        private List<MandrillTemplateContent> _templateContent;

        public string TemplateName { get; set; }

        public List<MandrillTemplateContent> TemplateContent
        {
            get { return _templateContent ?? (_templateContent = new List<MandrillTemplateContent>()); }
            set { _templateContent = value; }
        }

        public bool ShouldSerializeTemplateContent() => true;
    }
}
