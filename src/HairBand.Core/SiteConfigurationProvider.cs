using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.OptionsModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class SiteConfigurationProvider : ISiteConfigurationProvider
    {
        private IConfiguration _config;
        private IOptions<AppSettings> _options;

        public SiteConfigurationProvider(IOptions<AppSettings> options, IConfiguration config)
        {
            this._config = config;
            this._options = options;                 
        }

        public AppSettings GetConfiguration()
        {
            return _options.Options;
        }

        public void UpdateConfiguration(AppSettings config)
        {
            _config.Set("AppSettings:Test", "updated");

            _config.Set("AppSettings", JsonConvert.SerializeObject(config));
        }
    }
}
