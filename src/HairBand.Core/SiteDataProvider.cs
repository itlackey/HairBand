using Microsoft.AspNet.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.ObjectFactories;

namespace HairBand
{
    public class SiteDataProvider : ISiteDataProvider
    {
        private IHostingEnvironment _host;
        private IPageDataProvider _pageProvider;

        public SiteDataProvider(IHostingEnvironment host, IPageDataProvider pageProvider)
        {
            this._host = host;
            this._pageProvider = pageProvider;
        }

        public async Task<SiteData> GetSiteDataAsync()
        {

            //Todo caching, static_files, pages, posts, related_posts, html_page, data, documents, categories, tags
           
            var path = this._host.WebRootPath + "/app_data/_config.yml";

            var des = new Deserializer(new DefaultObjectFactory(), new PascalCaseNamingConvention(), true);

            var s = des.Deserialize(new StringReader(File.ReadAllText(path))) as Dictionary<object, object>;

            var data = new SiteData();

            foreach (var item in s)
                data[item.Key.ToString()] = item.Value;

            if (String.IsNullOrEmpty(data.Name))
                throw new ArgumentNullException("Site name must be set.");

           
            data.RootPath = this._host.WebRootPath;

            var pages = await _pageProvider.GetPagesAsync();

            data.Pages = pages.ToList();
            
            return data;

        }
    }
}
