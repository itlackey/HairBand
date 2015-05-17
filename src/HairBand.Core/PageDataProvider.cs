using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using System.IO;
using CommonMark;
using System.Dynamic;

namespace HairBand
{
    public class PageDataProvider
    {
        private IHostingEnvironment _host;

        public PageDataProvider(IHostingEnvironment host)
        {
            this._host = host;
        }


        public async Task<IEnumerable<PageData>> GetPages()
        {
            return await Task.Run(async () =>
            {
                var urls = Directory.GetFiles(this._host.WebRootPath + "/app_data/pages/", "*.md")
                               .Select(p => Path.GetFileNameWithoutExtension(p).Replace('-', '/'));

                var pages = new List<PageData>();

                foreach (var item in urls)
                {
                    pages.Add(await GetPageData(item));
                }

                return pages;
            });

        }

        public async Task<PageData> GetPageData(string url)
        {
            return await Task.Run(() =>
            {
                var path = this._host.WebRootPath + "/app_data/pages/" + url.Replace('/', '-') + ".md";

                if (File.Exists(path))
                {
                    var md = File.ReadAllText(path);

                    var headerString = md.Substring(md.IndexOf("---\r\n"), md.LastIndexOf("---") - 2);

                    var settings = new Dictionary<string, string>();

                    var settingLines = headerString.Split('\r', '\n');

                    foreach (var line in settingLines)
                    {
                        if (line.Contains(":"))
                        {
                            var data = line.Split(':');

                            settings.Add(data.First(), data.Last());
                        }
                    }

                    var body = md.Substring(md.LastIndexOf("---") + 5);

                    var html = CommonMarkConverter.Convert(body);

                    return new PageData() { Url = url.Replace('-', '/'), Content = html, Settings = settings };
                }
                else
                    throw new FileNotFoundException("This page does not exist");

            });

        }

    }
}
