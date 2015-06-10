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
using Microsoft.AspNet.Identity;
using System.Threading;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HairBand.Controllers
{
    public class PagesController : Controller
    {

        private readonly IPageDataProvider _provider;
       // private IPageHtmlRender _renderer;
        private ISiteDataProvider _siteProvider;
        private IUserStore<BandMember> _userStore;

        public PagesController(
            //IPageHtmlRender renderer,
            IUserStore<BandMember> userStore,
            IPageDataProvider provider,
            ISiteDataProvider siteProvider) //, 
                                            //IOptions<AppSettings> appSettings, 
                                            //IHostingEnvironment host)
        {
            this._provider = provider;
            this._siteProvider = siteProvider;

            //this.AppSettings = appSettings;
            //this.Host = host;

            //this._renderer = renderer;
            this._userStore = userStore;

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

            if (page.StartsWith("_") || page.StartsWith("app_data"))
                return HttpNotFound();

            BandMember user = null;

            if (User.Identity.IsAuthenticated)
                user = await _userStore.FindByNameAsync(User.Identity.Name, CancellationToken.None);

            var model = await this._provider.GetData(page);
            var site = this._siteProvider.GetSiteData();
          //  var html = await this._renderer.GetHtmlAsync(page, user);

            //ViewBag.Content = html;


            ViewBag.Page = model;
            ViewBag.User = user;
            ViewBag.Site = site;
            return View(); // model: html);
        }

      
        //[Authorize(Roles = "Administrators")]
        public async Task<IActionResult> Admin()
        {
            BandMember user = null;

            if (User.Identity.IsAuthenticated)
                user = await _userStore.FindByNameAsync(User.Identity.Name, CancellationToken.None);


            //var model = await this._provider.GetData(page);
            //var html = await this._renderer.GetHtmlAsync("_admin/home", user);

            //ViewBag.Content = html;

            return View(); // model: html);

        }

        //public string RenderRazorViewToString(string viewName, object model)
        //{
        //    ViewData.Model = model;
        //    using (var sw = new StringWriter())
        //    {
          
        //        var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

        //        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

        //        viewResult.View.Render(viewContext, sw);

        //        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

        //        return sw.GetStringBuilder().ToString();
        //    }
        //}
    }
}
