using Microsoft.Framework.ConfigurationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class SiteConfiguraionProvider : ISiteConfigurationProvider
    {
        private IConfiguration _config;

        public SiteConfiguraionProvider(IConfiguration config)
        {
            this._config = config;

            //config.GetSubKey("AppSettings")
        }

        public Task<DynamicDictionaryObject> GetConfigurationAsync()
        {
            throw new NotImplementedException();

        
        }

        public Task UpdateConfigurationAsync(string key, string value)
        {
            _config.Set(key, value);
        }
    }
}
