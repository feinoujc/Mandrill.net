using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillTagsApi
    {
        Task<IList<MandrillTagInfo>> ListAsync();
        Task<MandrillTagInfo> InfoAsync(string tag);
        Task<MandrillTagInfo> DeleteAsync(string tag);
        Task<IList<MandrillTagTimeSeries>> TimeSeriesAsync(string tag);
        Task<IList<MandrillTagTimeSeries>> AllTimeSeriesAsync();
    }

    public partial interface IMandrillTagsApi
    {
        IList<MandrillTagInfo> List();
        MandrillTagInfo Info(string tag);
        MandrillTagInfo Delete(string tag);
        IList<MandrillTagTimeSeries> TimeSeries(string tag);
        IList<MandrillTagTimeSeries> AllTimeSeries();
    }
}
