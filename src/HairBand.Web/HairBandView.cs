using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Internal;
using System.IO;
using DotLiquid;
using DotLiquid.FileSystems;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.Text;

namespace HairBand.Web
{
    internal class HairBandView : IView
    {
        public HairBandView()
        {
            this.Path = "default";
        }

        public HairBandView(string path)
        {
            this.Path = path;
        }


        public string Path { get; private set; }

        public async Task RenderAsync(ViewContext context)
        {

            if (context == null)
                throw new ArgumentNullException("viewContext");

            var renderParams = GetTemplateParameters(context);

            var themePath = System.IO.Path.GetDirectoryName(Path); 

            Template.FileSystem = new LocalThemeFileSystem(themePath);


            string fileContents = GetTemplateConents(this.Path);

            var template = Template.Parse(fileContents);

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

            if (siteData != null)
                localVars.Add("lib_folder", "/lib");

            localVars.Add("images_folder", "/images");


            if (pageData != null)
                localVars.Add("content", GetContent(pageData, localVars));

            var renderParams = new RenderParameters
            {
                LocalVariables = Hash.FromDictionary(localVars)
            };

            return renderParams;
        }

        private static string GetContent(PageData pageData, Hash localVars)
        {
            var contentTemplate = Template.Parse(pageData.Content);
            var html = contentTemplate.Render(localVars);
            return html; // pageData.Content;
        }

        private string GetTemplateConents(string templatePath)
        {

            var templateContent = new StringBuilder();

            var fileContents = File.ReadAllText(Path);


            if (fileContents.Contains("---\r\n"))
            {

                var headerString = fileContents.Substring(0, fileContents.LastIndexOf("---") - 2);

                var des = new Deserializer(
                    new DefaultObjectFactory(),
                    new UnderscoredNamingConvention(),
                    false);

                var meta = des.Deserialize(new StringReader(headerString));

                var d = meta as Dictionary<object, object>;

                if (d.ContainsKey("layout"))
                {
                    Console.WriteLine("has layout");
                }

                ///Merge with parent layout... (recursive)

            }
            else
            {
                templateContent.Append(fileContents);
            }

            return templateContent.ToString();
        }
 }
}