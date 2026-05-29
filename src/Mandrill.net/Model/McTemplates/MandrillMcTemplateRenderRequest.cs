using System.Collections.Generic;

namespace Mandrill.Model
{
    internal class MandrillMcTemplateRenderRequest : MandrillRequestBase
    {
        public int McTemplateId { get; set; }
        public MandrillMcTemplateVersion? McTemplateVersion { get; set; }
        public List<MandrillMergeVar> MergeVars { get; set; }
    }
}
