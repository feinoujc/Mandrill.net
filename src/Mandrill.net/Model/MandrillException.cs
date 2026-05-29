using System;

namespace Mandrill.Model
{
    public class MandrillException : Exception
    {
        public MandrillException()
        {
        }

        public MandrillException(string message) : base(message)
        {
        }

        public MandrillException(string message, Exception inner) : base(message, inner)
        {
        }

        internal MandrillException(MandrillErrorResponse errorResponse, Exception inner)
            : base($"status: {errorResponse.Status}, code: {errorResponse.Code}, name: {errorResponse.Name}, message: {errorResponse.Message}", inner)
        {
            Status = errorResponse.Status;
            Code = errorResponse.Code;
            Name = errorResponse.Name;
        }

        public string Status { get; private set; }
        public int? Code { get; private set; }
        public string Name { get; private set; }
    }
}
