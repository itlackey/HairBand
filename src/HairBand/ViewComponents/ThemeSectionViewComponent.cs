using DotLiquid;
using DotLiquid.FileSystems;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand.ViewComponents
{
    public class ThemeSectionViewComponent : ViewComponent
    {
        public ThemeSectionViewComponent(IOptions<AppSettings> appSettings, IHostingEnvironment host)
        {
            this.AppSettings = appSettings;
            this.Host = host;
        }

        public IOptions<AppSettings> AppSettings { get; private set; }
        public IHostingEnvironment Host { get; private set; }

        public string ThemePath { get; private set; }

        public string GetHtml(string file)
        {


            if (!file.EndsWith(".html"))
                file += ".html";

            this.ThemePath = this.Host.WebRootPath + "/themes/" + this.AppSettings.Options.Theme;

            var templateHtml = System.IO.File.ReadAllText(ThemePath + "/" + file);

            return ReplaceTokens(templateHtml);
        }


        public IViewComponentResult Invoke(string file)
        {
            var html = this.GetHtml(file);

            return View("ThemeSection", html);

        }

        private string ReplaceTokens(string templateHtml)
        {

            Template.RegisterSafeType(typeof(AppSettings), new string[] { "Title", "Theme" });

            Template.FileSystem = new LocalFileSystem(this.ThemePath);
       
            //var context = new DotLiquid.Context();

            //var html = Template.FileSystem.ReadTemplateFile(context, "app_data/themes/happy/default.html");



            var template = Template.Parse(templateHtml);

            var hash = Hash.FromAnonymousObject(new
            {
                page = (ViewBag.PageData as PageData)._settings,
                site = this.AppSettings.Options,
                theme_folder = "/themes/" + this.AppSettings.Options.Theme,
                current_date = DateTime.Now
            });


            var themeFiles = Directory.GetFiles(ThemePath);

            foreach (var file in themeFiles)
            {
                hash.Add(Path.GetFileNameWithoutExtension(file), file);
            }

            var output = template.Render(hash);

            return output;



            //return templateHtml
            //                  .Replace("{{PageTitle}}", ViewBag.Title)
            //                  .Replace("{{SiteTitle}}", this.AppSettings.Options.SiteTitle)
            //                  .Replace("{{ThemeFolder}}", "/themes/" + this.AppSettings.Options.Theme)
            //                  .Replace("{{Year}}", DateTime.Now.Year.ToString());
        }

    }
}
