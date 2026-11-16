using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mandrill
{
    internal partial class MandrillTagsApi : IMandrillTagsApi
    {
        public MandrillTagsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; set; }

        public Task<IList<MandrillTagInfo>> ListAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTagRequest, IList<MandrillTagInfo>>("tags/list",
                new MandrillTagRequest(), cancellationToken);
        }

        public Task<MandrillTagInfo> InfoAsync(string tag, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTagRequest, MandrillTagInfo>("tags/info",
                new MandrillTagRequest
                {
                    Tag = tag
                }, cancellationToken);
        }

        public Task<MandrillTagInfo> DeleteAsync(string tag, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTagRequest, MandrillTagInfo>("tags/delete",
                new MandrillTagRequest
                {
                    Tag = tag
                }, cancellationToken);
        }

        public Task<IList<MandrillTagTimeSeries>> TimeSeriesAsync(string tag, CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTagRequest, IList<MandrillTagTimeSeries>>("tags/time-series",
                new MandrillTagRequest
                {
                    Tag = tag
                }, cancellationToken);
        }

        public Task<IList<MandrillTagTimeSeries>> AllTimeSeriesAsync(CancellationToken cancellationToken = default)
        {
            return MandrillApi.PostAsync<MandrillTagRequest, IList<MandrillTagTimeSeries>>("tags/all-time-series",
                new MandrillTagRequest(), cancellationToken);
        }
    }
}
