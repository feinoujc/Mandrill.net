using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Mandrill
{
    public class MandrillWhitelistsApi : IMandrillWhitelistsApi
    {
        public MandrillWhitelistsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; set; }

        public async Task<IList<MandrillWhitelistInfo>> ListAsync(string email)
        {
            return await MandrillApi.PostAsync<MandrillWhitelistRequest, IList<MandrillWhitelistInfo>>("whitelists/list.json",
                new MandrillWhitelistRequest
                {
                    Email = email
                });
        }

        public async Task<MandrillWhitelistInfo> AddAsync(string email)
        {
            return await MandrillApi.PostAsync<MandrillWhitelistRequest, MandrillWhitelistInfo>("whitelists/add.json",
                new MandrillWhitelistRequest
                {
                    Email = email
                });
        }

        public async Task<MandrillWhitelistInfo> DeleteAsync(string email)
        {
            return await MandrillApi.PostAsync<MandrillWhitelistRequest, MandrillWhitelistInfo>("whitelists/delete.json",
                new MandrillWhitelistRequest
                {
                    Email = email
                });
        }
    }
}
