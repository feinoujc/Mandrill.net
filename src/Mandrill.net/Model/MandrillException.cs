using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

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
            Status = errorResponse.Status;
            Code = errorResponse.Code;
            Name = errorResponse.Name;
        }


        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected MandrillException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Status = info.GetString("Status");
            Code = (int?) info.GetValue("Code", typeof (int?));
            Name = info.GetString("Name");
        }


        public string Status { get; private set; }
        public int? Code { get; private set; }
        public string Name { get; private set; }


        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("Status", Status);

            info.AddValue("Name", Name);
            info.AddValue("Code", Code, typeof (int?));

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }
    }
}