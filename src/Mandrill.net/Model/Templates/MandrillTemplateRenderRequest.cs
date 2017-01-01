using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mandrill.Model
{
    internal class MandrillTemplateRenderRequest : MandrillRequestBase
    {
        [Required]
        public string TemplateName { get; set; }

        public List<MandrillTemplateContent> TemplateContent { get; set; } = new List<MandrillTemplateContent>();

        public List<MandrillMergeVar> MergeVars { get; set; } = new List<MandrillMergeVar>();
    }
}