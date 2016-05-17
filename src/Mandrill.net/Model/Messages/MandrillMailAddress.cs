#if !NETSTANDARD13
using System.Net.Mail;
#endif

namespace Mandrill.Model
{
    public class MandrillMailAddress
    {
        public MandrillMailAddress()
        {
        }

        public MandrillMailAddress(string address)
        {

#if !NETSTANDARD13
            var parsed = new MailAddress(address);
            Email = parsed.Address;
            if (!string.IsNullOrEmpty(parsed.DisplayName))
            {
                Name = parsed.DisplayName;
            }
#else
            Email = address;
#endif

        }

        public MandrillMailAddress(string address, string name)
        {
#if !NETSTANDARD13
            var parsed = new MailAddress(address, name);
            Email = parsed.Address;
            Name = parsed.DisplayName;
#else
            Email = address;
            Name = name;
#endif
        }


        public string Email { get; set; }

        public string Name { get; set; }

        public MandrillMailAddressType? Type { get; set; }
    }
}