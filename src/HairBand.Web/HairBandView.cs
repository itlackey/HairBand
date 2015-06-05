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
        public HairBandView(string path)
        {
            this.Path = path;
        }
        

        public string Path { get; private set; }
       
        public async Task RenderAsync(ViewContext context)
        {

            if (context == null)
                throw new ArgumentNullException("viewContext");

            RenderParameters renderParams = GetTemplateParameters(context);

            var themePath = System.IO.Path.GetDirectoryName(Path); // siteData.RootPath + "\\themes\\" + siteData["theme"];

            Template.FileSystem = new LocalFileSystem(themePath);

            //// Render the template

            string fileContents = GetTemplateConents();

            var template = Template.Parse(fileContents);

            //template.Render(context.Writer, renderParams);
            var html = template.Render(renderParams);

            await context.Writer.WriteAsync(html);



        }

        private static RenderParameters GetTemplateParameters(ViewContext context)
        {
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

            localVars.Add("current_date", DateTime.Now);

            var siteData = context.ViewBag.Site as SiteData;
            var pageData = context.ViewBag.Page as PageData;

            if (siteData != null)
                localVars.Add("theme_folder", "/themes/" + siteData.Theme);


            if (pageData != null)
                localVars.Add("content", pageData.Content);

            var renderParams = new RenderParameters
            {
                LocalVariables = Hash.FromDictionary(localVars)
            };

            return renderParams;
        }

        private string GetTemplateConents()
        {
            var templateHtml = File.ReadAllText(Path); // themePath + "/default.html");

            var fileContents = templateHtml; // VirtualPathProviderHelper.Load(ViewPath);

            return fileContents;
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