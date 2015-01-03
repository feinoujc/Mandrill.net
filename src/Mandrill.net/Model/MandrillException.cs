using System;
using System.Runtime.Serialization;

namespace Mandrill.Model
{
    [Serializable]
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
            : base(string.Format("status: {0}, code: {1}, name: {2}, message: {3}",
                errorResponse.Status, errorResponse.Code, errorResponse.Name, errorResponse.Message), inner)
        {
        }


        protected MandrillException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}