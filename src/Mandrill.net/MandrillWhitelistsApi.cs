using Mandrill.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Mandrill
{
    internal partial class MandrillWhitelistsApi : IMandrillWhitelistsApi
    {
        public MandrillWhitelistsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }

        public Task<IList<MandrillWhitelistInfo>> ListAsync(string email)
        {
            return MandrillApi.PostAsync<MandrillWhitelistRequest, IList<MandrillWhitelistInfo>>("whitelists/list.json",
                new MandrillWhitelistRequest
                {
                    Email = email
                });
        }

        public Task<MandrillWhitelistInfo> AddAsync(string email)
        {
            return MandrillApi.PostAsync<MandrillWhitelistRequest, MandrillWhitelistInfo>("whitelists/add.json",
                new MandrillWhitelistRequest
                {
                    Email = email
                });
        }

        public Task<MandrillWhitelistInfo> DeleteAsync(string email)
        {
            return MandrillApi.PostAsync<MandrillWhitelistRequest, MandrillWhitelistInfo>("whitelists/delete.json",
                new MandrillWhitelistRequest
                {
                    Email = email
                });
        }
    }

#if NET45
    internal partial class MandrillWhitelistsApi
    {
        public IList<MandrillWhitelistInfo> List(string email)
        {
            return MandrillApi.Post<MandrillWhitelistRequest, IList<MandrillWhitelistInfo>>("whitelists/list.json",
                new MandrillWhitelistRequest
                {
                    Email = email
                });
        }

        public MandrillWhitelistInfo Add(string email)
        {
            return MandrillApi.Post<MandrillWhitelistRequest, MandrillWhitelistInfo>("whitelists/add.json",
                new MandrillWhitelistRequest
                {
                    Email = email
                });
        }

        public MandrillWhitelistInfo Delete(string email)
        {
            return MandrillApi.Post<MandrillWhitelistRequest, MandrillWhitelistInfo>("whitelists/delete.json",
                new MandrillWhitelistRequest
                {
                    Email = email
                });
        }
    }
#endif
}
