using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Hosting;

namespace HairBand.Controllers
{
    public class HomeController : Controller
    {        
        private readonly PageDataProvider _provider;

        public HomeController(PageDataProvider provider)
        {
            this._provider = provider;
        }

        public async Task<IActionResult> Index()
        {
           
            return View();
        }


        public async Task<IActionResult> Page(string page)
        {
            ViewBag.Page = page;

            var model = await this._provider.GetPageData(page);

            return View(model);
        }

        public async Task<IActionResult> About()
        {
            var pages = await this._provider.GetPages();


            ViewBag.Message = "Your application description page.";

            return View(pages);
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
