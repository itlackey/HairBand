using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public interface ISiteDataProvider
    {
        Task<SiteData> GetSiteDataAsync();

        Task UpdateSiteDataAsync(SiteData data);

    }
}
