using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mandrill.Model
{
    class MandrillSendTemplateRequest:MandrillSendMessageRequest
    {
        private List<MandrillTemplateContent> _templateContent;

        [Required]
        public string TemplateName { get; set; }

        [Required]
        public List<MandrillTemplateContent> TemplateContent
        {
            get { return _templateContent ?? (_templateContent = new List<MandrillTemplateContent>()); }
            set { _templateContent = value; }
        }
    }
}