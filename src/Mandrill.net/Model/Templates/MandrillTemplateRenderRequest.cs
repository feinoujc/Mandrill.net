using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mandrill.Model
{
    internal class MandrillTemplateRenderRequest : MandrillRequestBase
    {
        private List<MandrillMergeVar> _mergeVars;
        private List<MandrillTemplateContent> _templateContent;

        [Required]
        public string TemplateName { get; set; }

        public List<MandrillTemplateContent> TemplateContent
        {
            get { return _templateContent ?? (_templateContent = new List<MandrillTemplateContent>()); }
            set { _templateContent = value; }
        }

        public List<MandrillMergeVar> MergeVars
        {
            get { return _mergeVars ?? (_mergeVars = new List<MandrillMergeVar>()); }
            set { _mergeVars = value; }
        }
    }
}