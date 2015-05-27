using DotLiquid;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;
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

        public string GetHtml(string file)
        {

            if (!file.EndsWith(".html"))
                file += ".html";

            var themePath = this.Host.WebRootPath + "/themes/" + this.AppSettings.Options.Theme;

            var templateHtml = System.IO.File.ReadAllText(themePath + "/" + file);

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

            //ToDo replace with dotLiquid
            var template = Template.Parse(templateHtml);

            var hash = Hash.FromAnonymousObject(new
            {
                page = (ViewBag.PageData as PageData).Settings,
                site = this.AppSettings.Options,
                theme_folder = "/themes/" + this.AppSettings.Options.Theme,
                current_date = DateTime.Now
            });
           

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
