using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    class MandrillRejectsApi : IMandrillRejectsApi
    {
        public MandrillRejectsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; set; }

        public async Task<MandrillRejectAddResponse> AddAsync(string email, string comment = null, string subaccount = null)
        {
            if (email == null) throw new ArgumentNullException("email");
            return await MandrillApi.PostAsync<MandrillRejectRequest, MandrillRejectAddResponse>("rejects/add.json",
                new MandrillRejectRequest
                {
                    Email = email,
                    Comment = comment,
                    Subaccount = subaccount
                });
        }


        public async Task<MandrillRejectDeleteResponse> DeleteAsync(string email, string subaccount = null)
        {
            if (email == null) throw new ArgumentNullException("email");
            return await MandrillApi.PostAsync<MandrillRejectRequest, MandrillRejectDeleteResponse>("rejects/delete.json",
                new MandrillRejectRequest
                {
                    Email = email,
                    Subaccount = subaccount
                });
        }

        public async Task<IList<MandrillRejectInfo>> ListAsync(string email = null, bool? includeExpired = null, string subaccount = null)
        {
            return await MandrillApi.PostAsync<MandrillRejectRequest, IList<MandrillRejectInfo>>("rejects/list.json",
                new MandrillRejectRequest
                {
                    Email = email,
                    IncludeExpired = includeExpired,
                    Subaccount = subaccount
                });
        }
    }
}