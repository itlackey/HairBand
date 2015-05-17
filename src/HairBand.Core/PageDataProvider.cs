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


        public async Task<IEnumerable<string>> GetPageNames()
        {
            var pages = Directory.GetFiles(this._host.WebRootPath + "/app_data/pages/", "*.md")
                .Select(p => Path.GetFileNameWithoutExtension(p) + ".page");


            return pages;
        }

        public async Task<PageData> GetPageData(string url)
        {

            var path = this._host.WebRootPath + "/app_data/pages/" + url + ".md";

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

                return new PageData() { Content = html, Settings = settings };
            }
            else
                throw new FileNotFoundException("This page does not exist");



        }

    }
}
