using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.AspNet.Mvc;
using HairBand.Web;

namespace HairBand
{
    public class CoreBootstrapper : IBootstrapper
    {
        public IConfiguration Configuration { get; set; }

        public void Initialize(IConfiguration config)
        {
            Configuration = config;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.

            // Add the console logger.
            loggerfactory.AddConsole(minLevel: LogLevel.Warning);
          
            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
                // app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseErrorHandler("/Pages/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();
            //    .MapWhen(ctx => !ctx.Request.Path.HasValue || !ctx.Request.Path.Value.EndsWith(".html"), config =>
            //{

            //});




            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {

   

                routes.MapRoute(
                   name: "error",
                   template: "_error",
                   defaults: new { controller = "Pages", action = "Error" });

        
                routes.MapRoute(
                  name: "Post",
                  template: "blog/{post}",
                  defaults: new { controller = "Pages", action = "Post" });

                //routes.MapRoute(
                //   name: "HtmlPages",
                //   template: "{page}.html",
                //   defaults: new { controller = "Pages", action = "Page", page = "home" });

                routes.MapRoute(
                     name: "Pages",
                     template: "{*page}",
                     defaults: new { controller = "Pages", action = "Page", page = "index" });



                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });

        }

        public void ConfigureServices(IServiceCollection services)
        {

            // Add MVC services to the services container.
            services.AddMvc()
                .Configure<MvcOptions>(options =>
                {
                    //options.ViewEngines.Clear();
                    options.ViewEngines.Add(typeof(HairBandViewEngine));

                });

            services.Add(new ServiceDescriptor(typeof(IPageDataProvider), typeof(PageDataProvider), ServiceLifetime.Singleton));

            services.Add(new ServiceDescriptor(typeof(IPostDataProvider), typeof(PageDataProvider), ServiceLifetime.Singleton));

            services.Add(new ServiceDescriptor(typeof(ISiteDataProvider), typeof(SiteDataProvider), ServiceLifetime.Singleton));

        }


    }
}
