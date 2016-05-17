using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    public partial interface IMandrillUsersApi
    {
        Task<string> PingAsync();
        Task<IList<MandrillSenderDemographics>> SendersAsync();
        Task<MandrillUserInfo> InfoAsync();
    }
#if !NETSTANDARD13

    public partial interface IMandrillUsersApi
    {
        string Ping();
        IList<MandrillSenderDemographics> Senders();
        MandrillUserInfo Info();
    }
#endif
}