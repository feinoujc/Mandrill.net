namespace Mandrill.Model
{
    class MandrillErrorResponse
    {
        public string Status { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
