using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
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

        public async Task<string> PingAsync(CancellationToken cancellationToken = default)
        {
            return (await MandrillApi.PostAsync<MandrillUsersRequest, JsonElement>("users/ping2",
                new MandrillUsersRequest(), cancellationToken).ConfigureAwait(false)).GetProperty("PING").GetString()!;
        }

        public Task<MandrillPing2Response> Ping2Async(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillUsersRequest, MandrillPing2Response>("users/ping2",
                new MandrillUsersRequest(), cancellationToken);
        }

        public Task<IList<MandrillSenderDemographics>> SendersAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillUsersRequest, IList<MandrillSenderDemographics>>("users/senders", new MandrillUsersRequest(), cancellationToken);
        }

        public Task<MandrillUserInfo> InfoAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillUsersRequest, MandrillUserInfo>("users/info", new MandrillUsersRequest(), cancellationToken);
        }
    }
}
