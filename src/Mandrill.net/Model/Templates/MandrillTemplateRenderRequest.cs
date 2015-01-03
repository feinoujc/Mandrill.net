using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mandrill.Model;

namespace Mandrill.Model
{
    internal class MandrillTemplateRenderRequest : MandrillRequestBase
    {
        private IList<MandrillMergeVar> _mergeVars;
        private IList<MandrillTemplateContent> _templateContent;

        [Required]
        public string TemplateName { get; set; }

        public IList<MandrillTemplateContent> TemplateContent
        {
            get { return _templateContent ?? (_templateContent = new List<MandrillTemplateContent>()); }
            set { _templateContent = value; }
        }

        public IList<MandrillMergeVar> MergeVars
        {
            get { return _mergeVars ?? (_mergeVars = new List<MandrillMergeVar>()); }
            set { _mergeVars = value; }
        }
    }
}