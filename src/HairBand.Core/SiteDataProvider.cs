using Microsoft.AspNet.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            {
                if (item.Key.ToString() == "groups")
                {
                    var groups = new List<Group>();

                    foreach (var gr in item.Value as IEnumerable<object>)
                    {
                        var group = new Group();

                        group.Merge(gr as IDictionary<object, object>);
                    
                        groups.Add(group);

                    }
                    data.Groups = groups;

                }
                else
                {
                    data[item.Key.ToString()] = item.Value;

                }

            }

            if (String.IsNullOrEmpty(data.Name))
                throw new ArgumentNullException("Site name must be set.");


            data.RootPath = this._host.WebRootPath;

            var pages = await _pageProvider.GetPagesAsync();

            data.Pages = pages.OrderBy(p => p.Order).ThenBy(p => p.Title).ToList();

            if (data.Groups != null)
            {
                data.Groups = data.Groups.OrderBy(g => g.Order).ThenBy(g => g.Name).ToList();
                foreach (var item in data.Groups)
                {
                    item.Pages = data.Pages.Where(p => p.Group == item.Name).ToList();
                }

            }
            //pages.Select(p => p.Group).Where(g => !String.IsNullOrEmpty(g)).Distinct().ToList();

            return data;

        }

        public async Task UpdateSiteDataAsync(SiteData data)
        {

            var currentData = await GetSiteDataAsync();

            foreach (var item in currentData)
            {
                if (!data.ContainsKey(item.Key) || data[item.Key] == null || String.IsNullOrEmpty(data[item.Key].ToString()))
                    data[item.Key] = item.Value;
            }

            var serializer = new Serializer(SerializationOptions.None, new UnderscoredNamingConvention());

            var path = this._host.WebRootPath + "/app_data/_config.yml";
            var builder = new StringBuilder();

            serializer.Serialize(new StringWriter(builder), data);
            File.WriteAllText(path, builder.ToString());

        }
    }
}
