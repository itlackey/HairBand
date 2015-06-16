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
        private ISiteDataProvider _siteProvider;
        private IUserStore<BandMember> _userStore;

        public PagesController(
            IUserStore<BandMember> userStore,
            IPageDataProvider provider,
            ISiteDataProvider siteProvider)
        {
            this._provider = provider;
            this._siteProvider = siteProvider;
            this._userStore = userStore;

        }

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
            var site = await this._siteProvider.GetSiteDataAsync();

            ViewBag.Page = model;
            ViewBag.User = user;
            ViewBag.Site = site;

            return View();
        }

    }
}
