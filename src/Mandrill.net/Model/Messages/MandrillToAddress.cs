namespace Mandrill.Model
{
    public class MandrillToAddress
    {
        public MandrillToAddress()
        {
        }

        public MandrillToAddress(string email)
        {
            Email = email;
        }

        public MandrillToAddress(string email, string name)
        {
            Email = email;
            Name = name;
        }


        public string Email { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}