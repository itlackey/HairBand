using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public interface ISiteConfigurationProvider
    {
        AppSettings GetConfiguration();

        void UpdateConfiguration(AppSettings config);

    }
}
