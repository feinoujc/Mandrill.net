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
}
