using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandrill
{
    public interface IMandrillTagsApi
    {
        Task<IList<MandrillTagInfo>> ListAsync();
        Task<MandrillTagInfo> InfoAsync(string tag);
        Task<MandrillTagInfo> DeleteAsync(string tag);
        Task<IList<MandrillTagTimeSeries>> TimeSeriesAsync(string tag);
        Task<IList<MandrillTagTimeSeries>> AllTimeSeriesAsync();
    }

    public static class MandrillTagApiSynchronousExtensions
    {
        public static IList<MandrillTagInfo> List(this IMandrillTagsApi api)
        {
            return api.ListAsync().Result;
        }

        public static MandrillTagInfo Info(this IMandrillTagsApi api, string tag)
        {
            return api.InfoAsync(tag).Result;
        }

        public static MandrillTagInfo Delete(this IMandrillTagsApi api, string tag)
        {
            return api.DeleteAsync(tag).Result;
        }

        public static IList<MandrillTagTimeSeries> TimeSeries(this IMandrillTagsApi api, string tag)
        {
            return api.TimeSeriesAsync(tag).Result;
        }

        public static IList<MandrillTagTimeSeries> AllTimeSeries(this IMandrillTagsApi api)
        {
            return api.AllTimeSeriesAsync().Result;
        }
    }
}
