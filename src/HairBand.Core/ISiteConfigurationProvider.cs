using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public interface ISiteConfigurationProvider
    {
        Task<DynamicDictionaryObject> GetConfigurationAsync();

        Task UpdateConfigurationAsync(string key, string value);

    }
}
