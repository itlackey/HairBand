using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Internal;
using System.IO;
using DotLiquid;
using DotLiquid.FileSystems;

namespace HairBand.Web
{
    internal class HairBandView : IView
    {
        public HairBandView(string path, PageData page, SiteData site, BandMember user)
        {
            this.Page = page;
            this.Site = site;
            this.User = user;
            this.Path = path;
        }

        public PageData Page { get; private set; }
        public string Path { get; private set; }
        public SiteData Site { get; private set; }
        public BandMember User { get; private set; }

        public async Task RenderAsync(ViewContext context)
        {

            await context.Writer.WriteAsync("Hair band!!");
        }

        //public async Task<string> GetHtmlAsync(string url, BandMember currentUser)
        //{
        //    //var pageData = await this._pageDataProvider.GetData(url);

        //    //var siteData = await this._siteDataProvider.GetSiteDataAsync();

        //    var themePath = siteData.RootPath + "\\themes\\" + siteData["theme"];

        //    var templateHtml = File.ReadAllText(themePath + "/default.html");

        //    Template.FileSystem = new LocalFileSystem(themePath);

        //    var template = Template.Parse(templateHtml);
        //    Template.RegisterSafeType(typeof(BandMember), new string[] { "UserName", "Name", "Email" });
        //    Hash userHash = null;

        //    if (currentUser != null)
        //        userHash = Hash.FromAnonymousObject(new { name = currentUser.UserName });

        //    var hash = Hash.FromAnonymousObject(new
        //    {
        //        page = pageData,
        //        site = siteData,
        //        theme_folder = "/themes/" + siteData["theme"],
        //        current_date = DateTime.Now,
        //        content = pageData.Content,
        //        user = userHash

        //    });


        //    var output = template.Render(hash);

        //    return output;
        //}
    }
}