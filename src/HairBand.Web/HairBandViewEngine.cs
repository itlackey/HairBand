using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace HairBand.Web
{
    public class HairBandViewEngine : IViewEngine //, RazorViewEngine
    {
        public HairBandViewEngine()
        {

        }

        public ViewEngineResult FindPartialView(ActionContext context, string partialViewName)
        {
            return ViewEngineResult.Found(partialViewName, new HairBandView(partialViewName));
        }

        public ViewEngineResult FindView(ActionContext context, string viewName)
        {
            return ViewEngineResult.Found(viewName, new HairBandView(viewName));
        }
    }
}
