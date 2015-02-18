using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mandrill
{
    class MandrillTagsApi : IMandrillTagsApi
    {
        public MandrillTagsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; set; }

        public async Task<IList<MandrillTagInfo>> ListAsync()
        {
            return await MandrillApi.PostAsync<MandrillTagRequest, IList<MandrillTagInfo>>("tags/list.json",
                new MandrillTagRequest());
        }

        public async Task<MandrillTagInfo> InfoAsync(string tag)
        {
            return await MandrillApi.PostAsync<MandrillTagRequest, MandrillTagInfo>("tags/info.json",
                new MandrillTagRequest
                {
                    Tag = tag
                });
        }

        public async Task<MandrillTagInfo> DeleteAsync(string tag)
        {
            return await MandrillApi.PostAsync<MandrillTagRequest, MandrillTagInfo>("tags/delete.json",
                new MandrillTagRequest
                {
                    Tag = tag
                });
        }
    }
}
