namespace Mandrill.Model
{
    internal class MandrillMetadataAddRequest : MandrillRequestBase
    {
        public string Name { get; set; }
        public string ViewTemplate { get; set; }
    }
}
