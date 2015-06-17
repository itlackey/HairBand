using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.OptionsModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class SiteConfigurationProvider : ISiteConfigurationProvider
    {
        private IConfiguration _config;
        private IHostingEnvironment _host;
        private IOptions<AppSettings> _options;

        public SiteConfigurationProvider(IHostingEnvironment host, IOptions<AppSettings> options, IConfiguration config)
        {
            this._config = config;
            this._options = options;
            this._host = host;
        }

        public AppSettings GetConfiguration()
        {
            return _options.Options;
        }

        public void UpdateConfiguration(AppSettings config)
        {
            config.Test = Guid.NewGuid().ToString();

            var json = JsonConvert.SerializeObject(config);

            File.WriteAllText(_host.WebRootPath + "/app_data/settings.json", "{ \r\n \"AppSettings\":" + json + "\r\n}");

            _config.Set("AppSettings", json);
        }
    }
}
