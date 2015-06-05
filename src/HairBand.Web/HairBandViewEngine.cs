using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading;

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
            //if (context.RouteData.Values["controller"].ToString() != "Pages")
            //    return ViewEngineResult.NotFound(partialViewName, new string[] { });

            var view = this.CreateView(context, partialViewName);

            return ViewEngineResult.Found(partialViewName, view);
        }

        public ViewEngineResult FindView(ActionContext context, string viewName)
        {

            //if (context.RouteData.Values["controller"].ToString() != "Pages")
            //    return ViewEngineResult.NotFound(viewName, new string[] { });

            var view = this.CreateView(context, viewName);

            return ViewEngineResult.Found(viewName, view);
        }

        private HairBandView CreateView(ActionContext context, string viewName)
        {

            var siteData = _siteDataProvider.GetSiteData();

            if (viewName != "default.html")
            {
                if (!viewName.StartsWith("_"))
                    viewName = String.Format("_{0}", viewName);

                if (!viewName.EndsWith(".liquid") || !viewName.EndsWith(".html"))
                    viewName += ".liquid";

            }

            var path = String.Format("{0}/themes/{1}/{2}", siteData.RootPath, siteData.Theme ?? "Default", viewName);

            var view = new HairBandView(path);

            return view;
        }



    }
}
