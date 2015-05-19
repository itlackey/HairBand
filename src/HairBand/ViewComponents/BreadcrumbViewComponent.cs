using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand.ViewComponents
{
    public class BreadcrumbViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();

        }
    }
}
