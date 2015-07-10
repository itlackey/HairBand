using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using DotLiquid;
using Microsoft.AspNet.Identity;
using System.Threading;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HairBand.Controllers
{
    [ProtectedFolders]
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
        

        public async Task<IActionResult> Page(string page)
        {
            await PopulateViewContext(page);

            //ToDo refactor this into action filter...
            var site = ViewBag.Site as SiteData;

            if (!site.InstallCompleted)
                return RedirectToAction("install", new { controller = "admin", area = "admin" });

            var model = await this._pageProvider.GetPageAsync(page);
            ViewBag.Page = model;

            return View();
        }


        public async Task<IActionResult> Post(string page)
        {
            
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

            ViewBag.User = Hash.FromAnonymousObject(user);
            ViewBag.Site = site;
        }
    }
}
