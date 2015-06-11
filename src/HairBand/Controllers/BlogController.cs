using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HairBand.Controllers
{
    public class BlogController : Controller
    {
        private readonly IPostDataProvider _provider;
        private ISiteDataProvider _siteProvider;
        private IUserStore<BandMember> _userStore;

        public BlogController(
            IUserStore<BandMember> userStore,
            IPostDataProvider provider,
            ISiteDataProvider siteProvider)
        {
            this._provider = provider;
            this._siteProvider = siteProvider;
            this._userStore = userStore;

        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Post(string post)
        {

            if (post.StartsWith("_") || post.StartsWith("app_data"))
                return HttpNotFound();

            BandMember user = null;

            if (User.Identity.IsAuthenticated)
                user = await _userStore.FindByNameAsync(User.Identity.Name, CancellationToken.None);

            var model = await this._provider.GetPost(post);
            var site = this._siteProvider.GetSiteData();

            ViewBag.Page = model;
            ViewBag.User = user;
            ViewBag.Site = site;
            return View();
        }
    }
}
