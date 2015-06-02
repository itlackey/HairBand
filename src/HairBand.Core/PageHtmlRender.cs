using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLiquid;
using DotLiquid.FileSystems;
using System.IO;
using System.Threading;

namespace HairBand
{
    public class PageHtmlRender : IPageHtmlRender
    {
        private IPageDataProvider _pageDataProvider;
        private ISiteDataProvider _siteDataProvider;

        public PageHtmlRender(IPageDataProvider pageProvider, ISiteDataProvider siteProvider)
        {
            this._pageDataProvider = pageProvider;
            this._siteDataProvider = siteProvider;
        }

        public async Task<string> GetHtmlAsync(string url)
        {
            return await GetHtmlAsync(url, null);
        }

        public async Task<string> GetHtmlAsync(string url, BandMember currentUser)
        {
            var pageData = await this._pageDataProvider.GetData(url);

            var siteData = await this._siteDataProvider.GetSiteDataAsync();

            var themePath = siteData.RootPath + "\\themes\\" + siteData["theme"];

            var templateHtml = File.ReadAllText(themePath + "/default.html");

            Template.FileSystem = new LocalFileSystem(themePath);

            var template = Template.Parse(templateHtml);
            Template.RegisterSafeType(typeof(BandMember), new string[] { "UserName", "Name", "Email" });
            Hash userHash = null;

            if (currentUser != null)
               userHash =  Hash.FromAnonymousObject(new { name = currentUser.UserName });

            var hash = Hash.FromAnonymousObject(new
            {
                page = pageData,
                site = siteData,
                theme_folder = "/themes/" + siteData["theme"],
                current_date = DateTime.Now,
                content = pageData.Content,
                user = userHash

            });


            var output = template.Render(hash);

            return output;
        }
    }
}
