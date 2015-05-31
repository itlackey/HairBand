using Microsoft.AspNet.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class SiteDataProvider : ISiteDataProvider
    {
        private IHostingEnvironment _host;

        public SiteDataProvider(IHostingEnvironment host)
        {
            this._host = host;
        }

        public Task<SiteData> GetSiteDataAsync()
        {
            var path = this._host.WebRootPath + "/app_data/_config.yml";

            var des = new YamlDotNet.Serialization.Deserializer(
                new YamlDotNet.Serialization.ObjectFactories.DefaultObjectFactory(),
                new YamlDotNet.Serialization.NamingConventions.PascalCaseNamingConvention(), true);
            
            var s = des.Deserialize(new StringReader(File.ReadAllText(path))) as Dictionary<object, object>;


            var result = new Dictionary<string, object>();

            foreach (var item in s)
            {
                result.Add(item.Key.ToString(), item.Value);
            }

            var data = new SiteData(result);

            return Task.FromResult(data);

        }
    }
}
