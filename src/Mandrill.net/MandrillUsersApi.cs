using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Mandrill.Model;

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
            return (await MandrillApi.PostAsync<MandrillUsersRequest, JsonElement>("users/ping2",
                new MandrillUsersRequest()).ConfigureAwait(false)).GetProperty("PING").GetString()!;
        }

        public Task<MandrillPing2Response> Ping2Async()
        {
            return MandrillApi.PostAsync<MandrillUsersRequest, MandrillPing2Response>("users/ping2",
                new MandrillUsersRequest());
        }

        public Task<IList<MandrillSenderDemographics>> SendersAsync()
        {
            return MandrillApi.PostAsync<MandrillUsersRequest, IList<MandrillSenderDemographics>>("users/senders", new MandrillUsersRequest());
        }

        public Task<MandrillUserInfo> InfoAsync()
        {
            return MandrillApi.PostAsync<MandrillUsersRequest, MandrillUserInfo>("users/info", new MandrillUsersRequest());
        }
    }
}
