namespace Mandrill.Model
{
    internal class MandrillMetadataUpdateRequest : MandrillRequestBase
    {
        public string Name { get; set; }
        public string ViewTemplate { get; set; }
    }
}
