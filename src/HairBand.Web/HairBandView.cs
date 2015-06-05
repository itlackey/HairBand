using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Internal;
using System.IO;
using DotLiquid;
using DotLiquid.FileSystems;

namespace HairBand.Web
{
    internal class HairBandView : IView
    {
        public HairBandView(string path, PageData page, SiteData site, BandMember user)
        {
            this.Page = page;
            this.Site = site;
            this.User = user;
            this.Path = path;
        }

        public PageData Page { get; private set; }
        public string Path { get; private set; }
        public SiteData Site { get; private set; }
        public BandMember User { get; private set; }

        public async Task RenderAsync(ViewContext context)
        {

           // await context.Writer.WriteAsync("Hair band!!");

            if (context == null)
                throw new ArgumentNullException("viewContext");

            // Copy data from the view context over to DotLiquid
            var localVars = new Hash();

            if (context.ViewData.Model != null)
            {
                var model = context.ViewData.Model;

                //if(model.GetType().Name.EndsWith("ViewModel"))
                //{
                //    // If it's view model, just copy all properties to the localVars collection
                //    localVars.Merge(Hash.FromAnonymousObject(model));
                //}
                //else
                //{
                // It it's not a view model, just add the model direct as a "model" variable
                localVars.Add("model", model);
                //}
            }

            foreach (var item in context.ViewData)
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);

            foreach (var item in context.TempData)
                localVars.Add(Template.NamingConvention.GetMemberName(item.Key), item.Value);

            var renderParams = new RenderParameters
            {
                LocalVariables = Hash.FromDictionary(localVars)
            };

            // Render the template
            var fileContents = ""; // VirtualPathProviderHelper.Load(ViewPath);
            var template = Template.Parse(fileContents);
            template.Render(context.Writer, renderParams);

        }

        //public async Task<string> GetHtmlAsync(string url, BandMember currentUser)
        //{
        //    //var pageData = await this._pageDataProvider.GetData(url);

        //    //var siteData = await this._siteDataProvider.GetSiteDataAsync();

        //    var themePath = siteData.RootPath + "\\themes\\" + siteData["theme"];

        //    var templateHtml = File.ReadAllText(themePath + "/default.html");

        //    Template.FileSystem = new LocalFileSystem(themePath);

        //    var template = Template.Parse(templateHtml);
        //    Template.RegisterSafeType(typeof(BandMember), new string[] { "UserName", "Name", "Email" });
        //    Hash userHash = null;

        //    if (currentUser != null)
        //        userHash = Hash.FromAnonymousObject(new { name = currentUser.UserName });

        //    var hash = Hash.FromAnonymousObject(new
        //    {
        //        page = pageData,
        //        site = siteData,
        //        theme_folder = "/themes/" + siteData["theme"],
        //        current_date = DateTime.Now,
        //        content = pageData.Content,
        //        user = userHash

        //    });


        //    var output = template.Render(hash);

        //    return output;
        //}
    }
}