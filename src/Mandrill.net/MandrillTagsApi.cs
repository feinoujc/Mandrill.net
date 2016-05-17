﻿using Mandrill.Model;
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

        public Task<IList<MandrillTagInfo>> ListAsync()
        {
            return MandrillApi.PostAsync<MandrillTagRequest, IList<MandrillTagInfo>>("tags/list.json",
                new MandrillTagRequest());
        }

        public Task<MandrillTagInfo> InfoAsync(string tag)
        {
            return MandrillApi.PostAsync<MandrillTagRequest, MandrillTagInfo>("tags/info.json",
                new MandrillTagRequest
                {
                    Tag = tag
                });
        }

        public Task<MandrillTagInfo> DeleteAsync(string tag)
        {
            return MandrillApi.PostAsync<MandrillTagRequest, MandrillTagInfo>("tags/delete.json",
                new MandrillTagRequest
                {
                    Tag = tag
                });
        }

        public Task<IList<MandrillTagTimeSeries>> TimeSeriesAsync(string tag)
        {
            return MandrillApi.PostAsync<MandrillTagRequest, IList<MandrillTagTimeSeries>>("tags/time-series.json",
                new MandrillTagRequest
                {
                    Tag = tag
                });
        }

        public Task<IList<MandrillTagTimeSeries>> AllTimeSeriesAsync()
        {
            return MandrillApi.PostAsync<MandrillTagRequest, IList<MandrillTagTimeSeries>>("tags/all-time-series.json",
                new MandrillTagRequest());
        }
    }

#if NETFX
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

#endif
}
