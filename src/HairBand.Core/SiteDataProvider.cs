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

        public SiteDataProvider(IHostingEnvironment host)
        {
            this._host = host;
        }

        public Task<SiteData> GetSiteDataAsync()
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


            //var urls = Directory.GetFiles(this._host.WebRootPath + "/app_data/pages/", "*.md")
            //                  .Select(p => Path.GetFileNameWithoutExtension(p).Replace('-', '/'));

            //var pages = new List<IDictionary<string, object>>();

            //foreach (var item in urls)
            //{
            //    pages.Add(await GetData(item));
            //}


            return Task.FromResult(data);

        }
    }
}
