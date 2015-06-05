using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;

namespace HairBand.Web
{
    public class HairBandViewEngine : IViewEngine //, RazorViewEngine
    {
        //public HairBandViewEngine()
        //{

        //}

        private readonly IPageDataProvider _pageDataProvider;
        private IPageHtmlRender _renderer;
        private ISiteDataProvider _siteDataProvider;
        private IUserStore<BandMember> _userStore;

        public HairBandViewEngine(
            IPageHtmlRender renderer,
            IUserStore<BandMember> userStore,
            IPageDataProvider provider,
            ISiteDataProvider siteProvider) 
        {
            this._pageDataProvider = provider;
            this._siteDataProvider = siteProvider;

            this._renderer = renderer;
            this._userStore = userStore;

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
           new  TaskFactory().StartNew(async () =>
               {
                   var pageData = await this._pageDataProvider.GetData(viewName);

                   var siteData = await this._siteDataProvider.GetSiteDataAsync();


               }).ContinueWith(task =>
               {


               });

            throw new NotImplementedException();

            return new HairBandView("", null, null, null);

        }

    

    }
}
