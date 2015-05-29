﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Hosting;
using DotLiquid;
using DotLiquid.FileSystems;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HairBand.Controllers
{
    public class PagesController : Controller
    {

        private readonly IPageDataProvider _provider;

        public PagesController(IPageDataProvider provider, IOptions<AppSettings> appSettings, IHostingEnvironment host)
        {
            this._provider = provider;

            this.AppSettings = appSettings;
            this.Host = host;

        }

        public IOptions<AppSettings> AppSettings { get; private set; }
        public IHostingEnvironment Host { get; private set; }

        public string ThemePath { get; private set; }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var pages = await this._provider.GetPages();


            return View(pages);
        }


        public async Task<IActionResult> Page(string page)
        {
            

            ViewBag.Page = page;

            var model = await this._provider.GetPageData(page);

            ViewBag.PageData = model;


            ViewBag.Content = this.GetHtml(model);


            return View(model);
        }

        private string GetHtml(PageData page)
        {
            //if (!file.EndsWith(".html"))
            //    file += ".html";

            this.ThemePath = this.Host.WebRootPath + "\\themes\\" + this.AppSettings.Options.Theme;

            var templateHtml = System.IO.File.ReadAllText(ThemePath + "/default.html"); // + file);
       
            Template.RegisterSafeType(typeof(AppSettings), new string[] { "Title", "Theme" });

            Template.FileSystem = new LocalFileSystem(
                //"/themes/" + this.AppSettings.Options.Theme); 
                this.ThemePath);

            //var context = new DotLiquid.Context();

            //var html = Template.FileSystem.ReadTemplateFile(context, "app_data/themes/happy/default.html");



            var template = Template.Parse(templateHtml);

            var hash = Hash.FromAnonymousObject(new
            {
                page = page.Settings,
                site = this.AppSettings.Options,
                theme_folder = "/themes/" + this.AppSettings.Options.Theme,
                current_date = DateTime.Now,
                content = page.Content
            });


            //var themeFiles = Directory.GetFiles(ThemePath);

            //foreach (var file in themeFiles)
            //{
            //    hash.Add(Path.GetFileNameWithoutExtension(file), file);
            //}

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