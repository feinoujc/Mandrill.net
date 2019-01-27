using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillTemplateRenderRequest : MandrillRequestBase
    {
        public string TemplateName { get; set; }

        public List<MandrillTemplateContent> TemplateContent { get; set; } = new List<MandrillTemplateContent>();

        public List<MandrillMergeVar> MergeVars { get; set; } = new List<MandrillMergeVar>();
    }
}
