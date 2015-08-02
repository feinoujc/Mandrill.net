using Mandrill.Model;
using System;
using System.Collections.Generic;
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

        public async Task<IList<MandrillTagTimeSeries>> TimeSeriesAsync(string tag)
        {
            return await MandrillApi.PostAsync<MandrillTagRequest, IList<MandrillTagTimeSeries>>("tags/time-series.json",
                new MandrillTagRequest
                {
                    Tag = tag
                });
        }

        public async Task<IList<MandrillTagTimeSeries>> AllTimeSeriesAsync()
        {
            return await MandrillApi.PostAsync<MandrillTagRequest, IList<MandrillTagTimeSeries>>("tags/all-time-series.json",
                new MandrillTagRequest());
        }
    }

    internal partial class MandrillTagsApi
    {
        public IList<MandrillTagInfo> List()
        {
            return MandrillApi.Post<MandrillTagRequest, IList<MandrillTagInfo>>("tags/list.json",
                new MandrillTagRequest());
        }

        public MandrillTagInfo Info(string tag)
        {
            return MandrillApi.Post<MandrillTagRequest, MandrillTagInfo>("tags/info.json",
                new MandrillTagRequest
                {
                    Tag = tag
                });
        }

        public MandrillTagInfo Delete(string tag)
        {
            return MandrillApi.Post<MandrillTagRequest, MandrillTagInfo>("tags/delete.json",
                new MandrillTagRequest
                {
                    Tag = tag
                });
        }

        public IList<MandrillTagTimeSeries> TimeSeries(string tag)
        {
            return MandrillApi.Post<MandrillTagRequest, IList<MandrillTagTimeSeries>>("tags/time-series.json",
                new MandrillTagRequest
                {
                    Tag = tag
                });
        }

        public IList<MandrillTagTimeSeries> AllTimeSeries()
        {
            return MandrillApi.Post<MandrillTagRequest, IList<MandrillTagTimeSeries>>("tags/all-time-series.json",
                new MandrillTagRequest());
        }
    }
}
