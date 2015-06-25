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
using Microsoft.AspNet.FileProviders;

namespace HairBand
{
    public class PageDataProvider : IPageDataProvider, IPostDataProvider
    {
        private IHostingEnvironment _host;

        private string _rootDataDirectory = "/app_data";
        private string _pageDirectory = "/_pages";
        private string _postDirectory = "/_posts";
        private string _adminDirectory = "/_admin";

        public PageDataProvider(IHostingEnvironment host)
        {
            this._host = host;

        }


        public async Task<IEnumerable<PageData>> GetPagesAsync()
        {
            //ToDo does this move to site?

            var pages = new List<PageData>();

            var files = _host.WebRootFileProvider.GetDirectoryContents(_rootDataDirectory + _pageDirectory);

            //var urls = Directory.GetFiles(GetDirectoryPath("page"), "*.md")
            //               .Select(p => Path.GetFileNameWithoutExtension(p).Replace('-', '/'));

            foreach (var item in files)
            {

                var filePath = Path.GetFileNameWithoutExtension(item.Name).Replace('-', '/');

                var page = await GetPageAsync(filePath);

                page.Date = item.LastModified.Date;

                page.Path = item.PhysicalPath;

                SetPageUrls(page);


                pages.Add(page);
            }


            return pages.OrderBy(p => p.Order).ThenBy(p => p.Title);


        }

        private void SetPageUrls(PageData page)
        {
            var defaultUrl = Path.GetFileNameWithoutExtension(page.Path)
                //.Replace('_', '/')
                .Replace("-", "/");

            if (page.Url == null || !page.Url.Contains(defaultUrl))
            {
                var urls = new List<string>();

                //if(!String.IsNullOrEmpty(page.Parent))
                //{
                //    var parent =await GetPageAsync(page.Parent);
                //    defaultUrl = parent.Url.FirstOrDefault() + "/" + defaultUrl;
                //}

                urls.Add(defaultUrl);

                if (page.Url != null)
                    urls.AddRange(page.Url);

                page.Url = urls;
            }
        }


        public async Task<PageData> GetPageAsync(string url)
        {

            var fileName = GetFilename(url);

            var path = String.Format("{0}/{1}/{2}", this._rootDataDirectory, this._pageDirectory, fileName);


            var file = _host.WebRootFileProvider.GetFileInfo(path);

            if (file.Exists) // File.Exists(path))
            {

                string markdown = string.Empty;
                using (var reader = new StreamReader(file.CreateReadStream()))
                {

                    markdown = await reader.ReadToEndAsync();

                }

                if (!markdown.Contains("---\r\n"))
                    throw new ArgumentException("This is not a valid page. Page's must conain metadata.");


                var settings = new PageData();

                await PopulateData(file, settings);

                ////var markdown = File.ReadAllText(file.PhysicalPath);

                //var headerString = markdown.Substring(markdown.IndexOf("---\r\n"), markdown.LastIndexOf("---") - 2);

                //var des = new Deserializer(
                //    new DefaultObjectFactory(),
                //    new UnderscoredNamingConvention(),
                //    false);

                //var s = des.Deserialize(new StringReader(headerString));


                //var settings = new PageData();

                //var settingLines = headerString.Split('\r', '\n');


                //foreach (var line in settingLines)
                //{
                //    if (line.Contains(":"))
                //    {
                //        var data = line.Split(':');

                //        settings[data.First()] = data.Last();

                //    }
                //}


                //var body = markdown.Substring(markdown.LastIndexOf("---") + 5);

                //var html = CommonMarkConverter.Convert(body);

                //settings["content"] = html;

                ////SetRequiredProperties(url, settings);

                return settings;
            }
            else
                throw new FileNotFoundException("This page does not exist: " + file.PhysicalPath);


        }

        public async Task<PostData> GetPostAsync(string url)
        {
            var fileName = GetFilename(url);

            var path = String.Format("{0}/{1}/{2}", this._rootDataDirectory, this._postDirectory, fileName);

            var file = _host.WebRootFileProvider.GetFileInfo(path);

            if (file.Exists) // File.Exists(path))
            {

                var settings = new PostData();

                await PopulateData(file, settings);

                return settings;
            }
            else
                throw new FileNotFoundException("This post does not exist: " + file.PhysicalPath);

        }

        private static async Task PopulateData(IFileInfo file, DynamicDictionaryObject settings)
        {
            string markdown = string.Empty;
            using (var reader = new StreamReader(file.CreateReadStream()))
            {

                markdown = await reader.ReadToEndAsync();

            }

            var headerString = markdown.Substring(markdown.IndexOf("---\r\n"), markdown.LastIndexOf("---") - 2);
            try
            {

                var des = new Deserializer(
                    new DefaultObjectFactory(),
                    new UnderscoredNamingConvention(),
                    false);

                var s = des.Deserialize<Dictionary<object, object>>(new StringReader(headerString));

                if (s == null)
                    throw new InvalidDataException("Page does not contain valid meta data: " + file.Name);

                foreach (var item in s)
                {
                    settings[item.Key.ToString()] = item.Value;
                }
            }
            catch  (InvalidDataException dataEx)
            {
                throw dataEx;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Page does not contain valid meta data: " + file.Name, ex);
            }


            //var settingLines = headerString.Split('\r', '\n');


            //foreach (var line in settingLines)
            //{
            //    if (line.Contains(":"))
            //    {
            //        var data = line.Split(':');

            //        settings[data.First()] = data.Last();

            //    }
            //}


            var body = markdown.Substring(markdown.LastIndexOf("---") + 5);

            var html = CommonMarkConverter.Convert(body);

            settings["content"] = html;
        }

        private static string GetFilename(string url)
        {
            var fileName = url.TrimEnd('/');

            if (!url.StartsWith("_"))
                fileName = fileName.Replace('/', '-');

            if (!Path.HasExtension(fileName))
                fileName += ".md";
            else if (Path.HasExtension(fileName) && Path.GetExtension(fileName) == ".html")
                fileName = Path.ChangeExtension(fileName, ".md");

            return fileName;
        }

        public async Task<IEnumerable<PostData>> GetPostsAsync()
        {
            //ToDo does this move to site?

            var posts = new List<PostData>();

            var files = _host.WebRootFileProvider.GetDirectoryContents(_rootDataDirectory + _postDirectory);

            //var urls = Directory.GetFiles(GetDirectoryPath("page"), "*.md")
            //               .Select(p => Path.GetFileNameWithoutExtension(p).Replace('-', '/'));

            foreach (var item in files)
            {

                var post = await GetPostAsync(Path.GetFileNameWithoutExtension(item.Name).Replace('-', '/'));

                post.Date = item.LastModified.Date;
                post.Path = item.PhysicalPath;

                posts.Add(post);
            }

            return posts;
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
