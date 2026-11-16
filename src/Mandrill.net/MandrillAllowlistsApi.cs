using Mandrill.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Mandrill
{
    internal partial class MandrillAllowlistsApi : IMandrillAllowlistsApi
    {
        public MandrillAllowlistsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillAllowlistInfo>> ListAsync(string email, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillAllowlistRequest, IList<MandrillAllowlistInfo>>("allowlists/list",
                new MandrillAllowlistRequest
                {
                    Email = email
                }, cancellationToken);
        }

        public Task<MandrillAllowlistAddResponse> AddAsync(string email, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillAllowlistRequest, MandrillAllowlistAddResponse>("allowlists/add",
                new MandrillAllowlistRequest
                {
                    Email = email
                }, cancellationToken);
        }

        public Task<MandrillAllowlistDeleteResponse> DeleteAsync(string email, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillAllowlistRequest, MandrillAllowlistDeleteResponse>("allowlists/delete",
                new MandrillAllowlistRequest
                {
                    Email = email
                }, cancellationToken);
        }
    }
}
