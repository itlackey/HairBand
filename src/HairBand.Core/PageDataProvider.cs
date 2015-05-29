﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using System.IO;
using CommonMark;
using System.Dynamic;
using Newtonsoft.Json;

namespace HairBand
{
    public class PageDataProvider : IPageDataProvider
    {
        private IHostingEnvironment _host;

        public PageDataProvider(IHostingEnvironment host)
        {
            this._host = host;
          
        }


        public async Task<IEnumerable<IDictionary<string, object>>> GetPages()
        {
            return await Task.Run(async () =>
            {
                var urls = Directory.GetFiles(this._host.WebRootPath + "/app_data/pages/", "*.md")
                               .Select(p => Path.GetFileNameWithoutExtension(p).Replace('-', '/'));

                var pages = new List<IDictionary<string, object>>();

                foreach (var item in urls)
                {
                    pages.Add(await GetData(item));
                }

                return pages;
            });

        }

        public async Task<IDictionary<string, object>> GetData(string url)
        {
            return await Task.Run(() =>
            {

                var path = this._host.WebRootPath + "/app_data/pages/" + url.TrimEnd('/').Replace('/', '-') + ".md";

                if (File.Exists(path))
                {
                    var md = File.ReadAllText(path);

                    var headerString = md.Substring(md.IndexOf("---\r\n"), md.LastIndexOf("---") - 2);

                    var settings = new Dictionary<string, object>();

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

                    settings.Add("content", html);

                    if (!settings.ContainsKey("date"))
                        settings.Add("date", DateTime.Now);

                    return  settings;
                }
                else
                    throw new FileNotFoundException("This page does not exist");

            });

        }


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
