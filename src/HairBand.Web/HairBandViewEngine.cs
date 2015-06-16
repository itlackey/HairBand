using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading;
using System.IO;

namespace HairBand.Web
{
    public class HairBandViewEngine : IViewEngine //, RazorViewEngine
    {
        private ISiteDataProvider _siteDataProvider;

        public HairBandViewEngine(ISiteDataProvider siteProvider)
        {
            this._siteDataProvider = siteProvider;

        }

        public ViewEngineResult FindPartialView(ActionContext context, string partialViewName)
        {
            var view = this.CreateView(context, partialViewName);

            return ViewEngineResult.Found(partialViewName, view);
        }

        public ViewEngineResult FindView(ActionContext context, string viewName)
        {
            var view = this.CreateView(context, viewName);

            return ViewEngineResult.Found(viewName, view);
        }

        private HairBandView CreateView(ActionContext context, string viewName)
        {

            var siteData = _siteDataProvider.GetSiteData();
            var themePath = String.Format("{0}/themes/{1}/", siteData.RootPath, siteData.Theme ?? "Default");
            var path = string.Empty;


            if (File.Exists(Path.Combine(themePath, viewName)))
                path = Path.Combine(themePath, viewName);

            else if (File.Exists(Path.Combine(themePath, viewName + ".html")))
                path = Path.Combine(themePath, viewName + ".html");

            else if (File.Exists(Path.Combine(themePath, "_" + viewName)))
                path = Path.Combine(themePath, "_" + viewName);

            else if (File.Exists(Path.Combine(themePath, "_" + viewName + ".liquid")))
                path = Path.Combine(themePath, "_" + viewName + ".liquid");

            else
                throw new FileNotFoundException("View cannot be located.");

            return new HairBandView(path);
        }



    }
}
