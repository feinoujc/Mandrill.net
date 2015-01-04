using System.Net.Mail;

namespace Mandrill.Model
{
    public class MandrillMailAddress
    {
        public MandrillMailAddress()
        {
        }

        public MandrillMailAddress(string address)
        {
            var parsed = new MailAddress(address);
            Email = parsed.Address;
            if (!string.IsNullOrEmpty(parsed.DisplayName))
            {
                Name = parsed.DisplayName;
            }
        }

        public MandrillMailAddress(string address, string name)
        {
            var parsed = new MailAddress(address, name);
            Email = parsed.Address;
            Name = parsed.DisplayName;
        }


        public string Email { get; set; }

        public string Name { get; set; }

        public MandrillMailAddressType? Type { get; set; }
    }
}