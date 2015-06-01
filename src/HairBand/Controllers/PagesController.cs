using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Hosting;
using DotLiquid;
using DotLiquid.FileSystems;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HairBand.Controllers
{
    public class PagesController : Controller
    {

        private readonly IPageDataProvider _provider;
        private IPageHtmlRender _renderer;
        private ISiteDataProvider _siteProvider;

        public PagesController(
            IPageHtmlRender renderer,
            IPageDataProvider provider, 
            ISiteDataProvider siteProvider) //, 
            //IOptions<AppSettings> appSettings, 
            //IHostingEnvironment host)
        {
            this._provider = provider;
            this._siteProvider = siteProvider;

            //this.AppSettings = appSettings;
            //this.Host = host;

            this._renderer = renderer;

        }

        //public IOptions<AppSettings> AppSettings { get; private set; }
        //public IHostingEnvironment Host { get; private set; }

        //public string ThemePath { get; private set; }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var pages = await this._provider.GetPages();


            return View(pages);
        }


        public async Task<IActionResult> Page(string page)
        {

            var model = await this._provider.GetData(page);
            
            var html = await this._renderer.GetHtmlAsync(page);

            ViewBag.Content = html;

            return View(model);
        }

        //private string GetHtml(IDictionary<string, object> pageData, IDictionary<string, object> siteData)
        //{
        //    this.ThemePath = this.Host.WebRootPath + "\\themes\\" + siteData["theme"]; // this.AppSettings.Options.Theme;

        //    var templateHtml = System.IO.File.ReadAllText(ThemePath + "/default.html"); // + file);


        //    Template.RegisterSafeType(typeof(AppSettings), new string[] { "Title", "Theme" });

        //    Template.FileSystem = new LocalFileSystem(this.ThemePath);


        //    var template = Template.Parse(templateHtml);

        //    var hash = Hash.FromAnonymousObject(new
        //    {
        //        page = pageData,
        //        site = siteData,
        //        theme_folder = "/themes/" + this.AppSettings.Options.Theme,
        //        current_date = DateTime.Now,
        //        content = pageData["content"],
        //        user = this.User.Identity.Name
        //    });


        //    var output = template.Render(hash);

        //    return output;

        //}

    }
}
