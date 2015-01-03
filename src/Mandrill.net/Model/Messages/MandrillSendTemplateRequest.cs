using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mandrill.Model
{
    class MandrillSendTemplateRequest:MandrillSendMessageRequest
    {
        private IList<MandrillTemplateContent> _templateContent;

        [Required]
        public string TemplateName { get; set; }

        [Required]
        public IList<MandrillTemplateContent> TemplateContent
        {
            get { return _templateContent ?? (_templateContent = new List<MandrillTemplateContent>()); }
            set { _templateContent = value; }
        }
    }
}