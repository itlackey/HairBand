using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public interface ISiteDataProvider
    {
        SiteData GetSiteData();

        Task<SiteData> GetSiteDataAsync();
    }
}
