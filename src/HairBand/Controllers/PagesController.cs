using System;
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

            var des = new YamlDotNet.Serialization.Deserializer
                (new YamlDotNet.Serialization.ObjectFactories.DefaultObjectFactory(), 
                new YamlDotNet.Serialization.NamingConventions.UnderscoredNamingConvention(), 
                false);

            var s = des.Deserialize(new StringReader(""));


            ViewBag.Page = page;

            var model = await this._provider.GetData(page);

            ViewBag.PageData = model;


            ViewBag.Content =  this.GetHtml(model);

          
            return View(model);
        }

        private string GetHtml(IDictionary<string, object> pageData)
        {
            this.ThemePath = this.Host.WebRootPath + "\\themes\\" + this.AppSettings.Options.Theme;

            var templateHtml = System.IO.File.ReadAllText(ThemePath + "/default.html"); // + file);

       
            Template.RegisterSafeType(typeof(AppSettings), new string[] { "Title", "Theme" });

            
           Template.FileSystem = new LocalFileSystem(
                //"/themes/" + this.AppSettings.Options.Theme); 
                this.ThemePath);

            
            var template = Template.Parse(templateHtml);

            var hash = Hash.FromAnonymousObject(new
            {
                page = pageData,
                site =  this.AppSettings.Options,
                theme_folder = "/themes/" + this.AppSettings.Options.Theme,
                current_date = DateTime.Now,
                content = pageData["content"],
                user = this.User.Identity.Name
            });
            

            var output = template.Render(hash);

            return output;
            
        }

    }
}
