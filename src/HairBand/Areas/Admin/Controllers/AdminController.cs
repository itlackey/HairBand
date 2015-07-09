using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HairBand.Controllers
{
    [Area("Admin")]
    //[Route("_admin/[action]")]
    public class AdminController : Controller
    {
        private ISiteDataProvider _siteDataProvider;
        private IHostingEnvironment _host;

        public AdminController(ISiteDataProvider siteDataProvider, IHostingEnvironment host)
        {
            this._siteDataProvider = siteDataProvider;
            this._host = host;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        #region Files
        public ActionResult Files()
        {
            return View();
        } 

        [Route("_admin/api/files/")]
        public IActionResult GetFileSystemItems()
        {
            var items = _host.WebRootFileProvider.GetDirectoryContents("App_data");
         
            return new ObjectResult(items);
        }
        #endregion

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
