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

        private readonly IPageDataProvider _pageProvider;
        private ISiteDataProvider _siteProvider;
        private IUserStore<BandMember> _userStore;
        private IPostDataProvider _postProvider;

        public PagesController(
            IUserStore<BandMember> userStore,
            IPageDataProvider pageProvider,
            IPostDataProvider postProvider,
            ISiteDataProvider siteProvider)
        {
            this._pageProvider = pageProvider;

            this._postProvider = postProvider;

            this._siteProvider = siteProvider;

            this._userStore = userStore;

        }

        //// GET: /<controller>/
        //public async Task<IActionResult> Index()
        //{
        //    var pages = await this._pageProvider.GetPages();


        //    return View(pages);
        //}


        public async Task<IActionResult> Page(string page)
        {

            if (page.StartsWith("_") || page.StartsWith("app_data"))
                return HttpNotFound();

            await PopulateViewContext(page);

            var model = await this._pageProvider.GetPageAsync(page);
            ViewBag.Page = model;

            return View();
        }


        public async Task<IActionResult> Post(string page)
        {

            if (page.StartsWith("_") || page.StartsWith("app_data"))
                return HttpNotFound();

            await PopulateViewContext(page);

            var model = await this._postProvider.GetPostAsync(page);
            ViewBag.Page = model;

            return View();
        }


        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }



        private async Task PopulateViewContext(string page)
        {
            BandMember user = null;

            if (User.Identity.IsAuthenticated)
                user = await _userStore.FindByNameAsync(User.Identity.Name, CancellationToken.None);


            var site = await this._siteProvider.GetSiteDataAsync();

            ViewBag.User = user;
            ViewBag.Site = site;
        }
    }
}
