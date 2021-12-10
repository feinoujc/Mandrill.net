using Mandrill.Model;
using System.Collections.Generic;
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

        public Task<IList<MandrillAllowlistInfo>> ListAsync(string email)
        {
            return MandrillApi.PostAsync<MandrillAllowlistRequest, IList<MandrillAllowlistInfo>>("allowlists/list.json",
                new MandrillAllowlistRequest
                {
                    Email = email
                });
        }

        public Task<MandrillAllowlistInfo> AddAsync(string email)
        {
            return MandrillApi.PostAsync<MandrillAllowlistRequest, MandrillAllowlistInfo>("allowlists/add.json",
                new MandrillAllowlistRequest
                {
                    Email = email
                });
        }

        public Task<MandrillAllowlistInfo> DeleteAsync(string email)
        {
            return MandrillApi.PostAsync<MandrillAllowlistRequest, MandrillAllowlistInfo>("allowlists/delete.json",
                new MandrillAllowlistRequest
                {
                    Email = email
                });
        }
    }
}
