using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Hosting;
using HairBand.Web;

namespace HairBand.Controllers
{
    public class HomeController : Controller
    {
        private DefaultUserStore _store;

        public HomeController(IHostingEnvironment host)
        {
            _store = new DefaultUserStore(host);

        }
        public IActionResult Index()
        {
            var users = _store.Users;

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
