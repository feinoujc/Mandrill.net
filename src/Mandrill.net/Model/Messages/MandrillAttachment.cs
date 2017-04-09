namespace Mandrill.Model
{
    public class MandrillAttachment
    {
        public MandrillAttachment()
        {
        }

        public MandrillAttachment(string type, string name, byte[] content)
        {
            Type = type;
            Name = name;
            Content = content;
        }

        public string Type { get; set; }

        public string Name { get; set; }

        public byte[] Content { get; set; }
    }
}
