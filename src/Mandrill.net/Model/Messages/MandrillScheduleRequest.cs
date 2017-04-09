namespace Mandrill.Model
{
    class MandrillScheduleRequest : MandrillRequestBase
    {
        public string To { get; set; }
        public string Id { get; set; }
        public string SendAt { get; set; }
    }
}
