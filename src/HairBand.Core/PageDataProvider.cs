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
using System.Text;

namespace HairBand
{
    public class PageDataProvider : IPageDataProvider, IPostDataProvider
    {
        private IHostingEnvironment _host;

        // private string _rootDataDirectory = "/app_data";
        private string _pageDirectory = "/app_data/_pages";
        private string _postDirectory = "/app_data/_posts";
        private string _adminDirectory = "/app_data/_admin";

        public PageDataProvider(IHostingEnvironment host)
        {
            this._host = host;

        }

        private async Task<IEnumerable<PageData>> GetPagesByFolder(string path)
        {
            var items = _host.WebRootFileProvider.GetDirectoryContents(path);
            
            var pages = new List<PageData>();

            foreach (var item in items)
            {

                if (item.IsDirectory)
                {
                    pages.AddRange(await GetPagesByFolder(path + "/" + item.Name));
                }
                else
                {
                    var page = await GetPageAsync(path + "/" + item.Name);

                    pages.Add(page);
                }

            }


            return pages.OrderBy(p => p.Order).ThenBy(p => p.Title);
        }



        public async Task<IEnumerable<PageData>> GetPagesAsync()
        {
            //ToDo does this move to site?
            var pages = await GetPagesByFolder(_pageDirectory);

            return pages;

        }

        public async Task<PageData> GetPageAsync(string url)
        {

            var path = url;

            if (!path.StartsWith(_pageDirectory))
                path = _pageDirectory + "/" + path;


            if (path.EndsWith("/"))
                path += "index.md";

            if (!Path.HasExtension(path))
                path += ".md";

            var file = _host.WebRootFileProvider.GetFileInfo(path);

            if (file.Exists)
            {

                string markdown = string.Empty;
                using (var reader = new StreamReader(file.CreateReadStream()))
                {

                    markdown = await reader.ReadToEndAsync();

                }

                if (!markdown.Contains("---\r\n"))
                    throw new ArgumentException("This is not a valid page. Page's must contain metadata.");


                var page = new PageData();


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
                        page[item.Key.ToString()] = item.Value;
                    }
                }
                catch (InvalidDataException dataEx)
                {
                    throw dataEx;
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException("Page does not contain valid meta data: " + file.Name, ex);
                }

                var body = markdown.Substring(markdown.LastIndexOf("---") + 5);

                var html = CommonMarkConverter.Convert(body);

                page["content"] = html;

                page.Date = file.LastModified.Date;

                page.Path = file.PhysicalPath;


                var urlBuilder = new StringBuilder();
                urlBuilder.Append(path); // + "/" + file.Name);

                urlBuilder.Replace(".md", string.Empty);
                urlBuilder.Replace(".html", string.Empty);

                if (urlBuilder.ToString().EndsWith("index"))
                    urlBuilder.Remove(urlBuilder.ToString().LastIndexOf("index"), 5);

                if (urlBuilder.ToString().Contains(_pageDirectory))
                    urlBuilder.Replace(_pageDirectory, "");

                if (!urlBuilder.ToString().StartsWith("/"))
                    urlBuilder.Insert(0, "/");


                page.Url = urlBuilder.ToString();
                

                return page;
            }
            else
                throw new FileNotFoundException("This page does not exist: " + file.PhysicalPath);


        }

        public async Task<PostData> GetPostAsync(string url)
        {
            var fileName = GetRelativePath(url);

            var path = String.Format("{0}/{1}", this._postDirectory, fileName);

            var file = _host.WebRootFileProvider.GetFileInfo(path);

            if (file.Exists)
            {

                var settings = new PostData();

                await PopulateData(file, settings);

                return settings;
            }
            else
                throw new FileNotFoundException("This post does not exist: " + file.PhysicalPath);

        }

        private async Task PopulateData(IFileInfo file, PageData page)
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
                    page[item.Key.ToString()] = item.Value;
                }
            }
            catch (InvalidDataException dataEx)
            {
                throw dataEx;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Page does not contain valid meta data: " + file.Name, ex);
            }

            var body = markdown.Substring(markdown.LastIndexOf("---") + 5);

            var html = CommonMarkConverter.Convert(body);

            page["content"] = html;

            page.Date = file.LastModified.Date;

            page.Path = file.PhysicalPath;


            //var urlBuilder = new StringBuilder();
            //urlBuilder.Append(path + "/" + file.Name);

            //urlBuilder.Replace(".md", string.Empty);
            //urlBuilder.Replace(".html", string.Empty);

            //if (urlBuilder.ToString().EndsWith("index"))
            //    urlBuilder.Remove(urlBuilder.ToString().LastIndexOf("index"), 5);

            //if (urlBuilder.ToString().Contains(_pageDirectory))
            //    urlBuilder.Replace(_pageDirectory, "");

            //if (!urlBuilder.ToString().StartsWith("/"))
            //    urlBuilder.Insert(0, "/");


            //page.Url = urlBuilder.ToString();

        }

        private static string GetRelativePath(string url)
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

            var files = _host.WebRootFileProvider.GetDirectoryContents(_postDirectory);

            foreach (var item in files)
            {

                var post = await GetPostAsync(Path.GetFileNameWithoutExtension(item.Name).Replace('-', '/'));

                post.Date = item.LastModified.Date;
                post.Path = item.PhysicalPath;

                posts.Add(post);
            }

            return posts;
        }
    }
}
