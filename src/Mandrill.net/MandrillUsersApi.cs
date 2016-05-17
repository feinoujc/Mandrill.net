using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;
using Newtonsoft.Json.Linq;

namespace Mandrill
{
    internal partial class MandrillUsersApi : IMandrillUsersApi
    {
        public MandrillUsersApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public async Task<string> PingAsync()
        {
            return (await MandrillApi.PostAsync<MandrillUsersRequest, JObject>("users/ping2.json",
                new MandrillUsersRequest()).ConfigureAwait(false))["PING"].Value<string>();
        }

        public Task<IList<MandrillSenderDemographics>> SendersAsync()
        {
            return MandrillApi.PostAsync<MandrillUsersRequest, IList<MandrillSenderDemographics>>("users/senders.json", new MandrillUsersRequest());
        }

        public Task<MandrillUserInfo> InfoAsync()
        {
            return MandrillApi.PostAsync<MandrillUsersRequest, MandrillUserInfo>("users/info.json", new MandrillUsersRequest());
        }
    }

#if NETFX
    internal partial class MandrillUsersApi
    {
        public string Ping()
        {
            return (MandrillApi.Post<MandrillUsersRequest, JObject>("users/ping2.json",
                new MandrillUsersRequest()))["PING"].Value<string>();
        }

        public IList<MandrillSenderDemographics> Senders()
        {
            return MandrillApi.Post<MandrillUsersRequest, List<MandrillSenderDemographics>>("users/senders.json", new MandrillUsersRequest());
        }

        public MandrillUserInfo Info()
        {
            return MandrillApi.Post<MandrillUsersRequest, MandrillUserInfo>("users/info.json", new MandrillUsersRequest());
        }
    }
#endif
}