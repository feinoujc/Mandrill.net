

namespace Mandrill.Model
{
    public class MandrillMailAddress
    {
        public MandrillMailAddress()
        {
        }

        public MandrillMailAddress(string address)
        {
            Email = address;

        }

        public MandrillMailAddress(string address, string name)
        {

            Email = address;
            Name = name;
        }


        public string Email { get; set; }

        public string Name { get; set; }

        public MandrillMailAddressType? Type { get; set; }
    }
}
