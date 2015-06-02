using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using System.IO;
using CommonMark;
using System.Dynamic;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.NamingConventions;

namespace HairBand
{
    public class PageDataProvider : IPageDataProvider
    {
        private IHostingEnvironment _host;

        private string _rootDataDirectory = "/app_data/";
        private string _pageDirectory = "/_pages/";
        private string _postDirectory = "/_posts/";
        private string _adminDirectory = "/_admin/";

        public PageDataProvider(IHostingEnvironment host)
        {
            this._host = host;

        }


        public async Task<IEnumerable<PageData>> GetPages()
        {
            //ToDo does this move to site?
            return await Task.Run(async () =>
            {
                var urls = Directory.GetFiles(GetDirectoryPath("page"), "*.md")
                               .Select(p => Path.GetFileNameWithoutExtension(p).Replace('-', '/'));

                var pages = new List<PageData>();

                foreach (var item in urls)
                {
                    pages.Add(await GetData(item));
                }

                return pages;
            });

        }

        private string GetDirectoryPath(string pageType)
        {
            return this._host.WebRootPath + "/app_data/_pages/";
        }

        public async Task<PageData> GetData(string url)
        {
            return await Task.Run(() =>
            {
                var path = GetFilePath(url);

                if (File.Exists(path))
                {
                    var md = File.ReadAllText(path);

                    var headerString = md.Substring(md.IndexOf("---\r\n"), md.LastIndexOf("---") - 2);

                    var des = new Deserializer(
                        new DefaultObjectFactory(),
                        new UnderscoredNamingConvention(),
                        false);

                    var s = des.Deserialize(new StringReader(headerString));


                    var settings = new PageData();

                    var settingLines = headerString.Split('\r', '\n');


                    foreach (var line in settingLines)
                    {
                        if (line.Contains(":"))
                        {
                            var data = line.Split(':');

                            settings[data.First()] = data.Last();

                        }
                    }


                    var body = md.Substring(md.LastIndexOf("---") + 5);

                    var html = CommonMarkConverter.Convert(body);

                    settings["content"] = html;

                    //SetRequiredProperties(url, settings);

                    return settings;
                }
                else
                    throw new FileNotFoundException("This page does not exist");

            });

        }

        private string GetFilePath(string url)
        {
            var pageDirectory = "_pages/";

            if (url.StartsWith("_admin"))
                pageDirectory = string.Empty;

            var path = this._host.WebRootPath + "/app_data/" + pageDirectory + url.TrimEnd('/').Replace('/', '-') + ".md";
            return path;
        }

        //protected void SetRequiredProperties(string url, Dictionary<string, object> settings)
        //{
        //    SetDefaultValue(settings, "title", String.Empty);

        //    SetDefaultValue(settings, "excerpt", String.Empty);

        //    SetDefaultValue(settings, "url", url);

        //    SetDefaultValue(settings, "date", DateTime.Now);

        //    SetDefaultValue(settings, "id", url);

        //    SetDefaultValue(settings, "categories", new string[0]);

        //    SetDefaultValue(settings, "tags", new string[0]);

        //    SetDefaultValue(settings, "path", url);

        //    SetDefaultValue(settings, "next", null);

        //    SetDefaultValue(settings, "previous", null);
        //}

        //protected void SetDefaultValue(IDictionary<string, object> settings, string key, object defaultValue)
        //{
        //    if (!settings.ContainsKey(key))
        //        settings.Add(key, defaultValue);
        //}

        //public async Task<PageData> GetPageData(string url)
        //{
        //    return await Task.Run(() =>
        //    {

        //        var path = this._host.WebRootPath + "/app_data/pages/" + url.TrimEnd('/').Replace('/', '-') + ".md";

        //        if (File.Exists(path))
        //        {
        //            var md = File.ReadAllText(path);

        //            var headerString = md.Substring(md.IndexOf("---\r\n"), md.LastIndexOf("---") - 2);

        //            var settings = new Dictionary<string, object>();

        //            var settingLines = headerString.Split('\r', '\n');

        //            var metadata = new PageSettings();

        //            foreach (var line in settingLines)
        //            {
        //                if (line.Contains(":"))
        //                {
        //                    var data = line.Split(':');

        //                    settings.Add(data.First(), data.Last());

        //                    metadata.AddProperty(data.First(), data.Last());

        //                }
        //            }

        //            var body = md.Substring(md.LastIndexOf("---") + 5);

        //            var html = CommonMarkConverter.Convert(body);

        //            return new PageData() { Url = url.Replace('-', '/'), Content = html, Settings = settings, Metadata = metadata };
        //        }
        //        else
        //            throw new FileNotFoundException("This page does not exist");

        //    });

        //}

    }
}
