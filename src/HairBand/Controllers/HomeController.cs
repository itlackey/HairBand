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
        private IHostingEnvironment _host;

        public HomeController(IHostingEnvironment host)
        {
            this._host = host;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Page(string page)
        {
            ViewBag.Page = page;

            var path = this._host.WebRootPath + "/app_data/" + page + ".md";

            var md = System.IO.File.ReadAllText(path);

            var html = CommonMark.CommonMarkConverter.Convert(md);

            ViewBag.PagePath = path;

            ViewBag.Html = html;

            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
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
