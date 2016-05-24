using System;
#if NET45
using System.Runtime.Serialization;
using System.Security.Permissions;
#endif
namespace Mandrill.Model
{
#if NET45
    [Serializable]
#endif
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


#if NET45

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected MandrillException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Status = info.GetString("Status");
            Code = (int?) info.GetValue("Code", typeof (int?));
            Name = info.GetString("Name");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("Status", Status);

            info.AddValue("Name", Name);
            info.AddValue("Code", Code, typeof (int?));

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }
#endif
    }
}