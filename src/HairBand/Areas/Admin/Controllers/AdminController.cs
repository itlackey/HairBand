using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HairBand.Controllers
{
    [Area("Admin")]
    [Route("_admin/[action]")]
    public class AdminController : Controller
    {
        private ISiteDataProvider _siteDataProvider;

        public AdminController(ISiteDataProvider siteDataProvider)
        {
            this._siteDataProvider = siteDataProvider;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Install()
        {
            return View();
        }

        
        [HttpPost(Name = "Install")]
        public async Task<IActionResult> Install(SiteData model)
        {
            model.InstallCompleted = true;

            await _siteDataProvider.UpdateSiteDataAsync(model);

            return Redirect("~/");
        }

        public async Task<IActionResult> Settings()
        {
            return View();
        }

        [HttpPost(Name ="Settings")]
        public async Task<IActionResult> Settings(SiteData model)
        {
            model.InstallCompleted = true;

            await _siteDataProvider.UpdateSiteDataAsync(model);

            return Redirect("~/");
        }


    }
}
