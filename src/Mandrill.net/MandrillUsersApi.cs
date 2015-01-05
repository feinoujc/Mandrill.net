using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;
using Newtonsoft.Json.Linq;

namespace Mandrill
{
    class MandrillUsersApi : IMandrillUsersApi
    {
        public MandrillUsersApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; set; }

        public async Task<string> PingAsync()
        {
            return (await MandrillApi.PostAsync<MandrillUsersRequest, JObject>("users/ping2.json",
                new MandrillUsersRequest()))["PING"].Value<string>();
        }

        public async Task<IList<MandrillSenderDemographics>> SendersAsync()
        {
            return await MandrillApi.PostAsync<MandrillUsersRequest, List<MandrillSenderDemographics>>("users/senders.json", new MandrillUsersRequest());
        }

        public async Task<MandrillUserInfo> InfoAsync()
        {
            return await MandrillApi.PostAsync<MandrillUsersRequest, MandrillUserInfo>("users/info.json", new MandrillUsersRequest());
        }
    }
}