using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using HairBand.Web;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Facebook;

namespace HairBand
{
    public class AdminBootstrapper : IBootstrapper
    {
        public IConfiguration Configuration { get; set; }

        public void Initialize(IConfiguration config)
        {
            Configuration = config;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Add cookie-based authentication to the request pipeline.
            app.UseIdentity();


            // Add authentication middleware to the request pipeline. You can configure options such as Id and Secret in the ConfigureServices method.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            // app.UseFacebookAuthentication();
            // app.UseGoogleAuthentication();
            // app.UseMicrosoftAccountAuthentication();
            // app.UseTwitterAuthentication();



            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {

                routes.MapRoute(
                           name: "manage",
                           template: "_account/manage/{action}/{id?}",
                           defaults: new { controller = "Manage", action = "Index", area = "Admin" });


                routes.MapRoute(
                      name: "logoff",
                      template: "_account/logoff",
                      defaults: new { controller = "Account", action = "LogOff", area = "Admin" });



                routes.MapRoute(
                    name: "account",
                    template: "_account/{action}/{id?}",
                    defaults: new { controller = "Account", action = "Login", area = "Admin" });



                routes.MapRoute(
                    name: "admin",
                    template: "_admin/{action}/{id?}",
                    defaults: new { controller = "Admin", action = "Index", area = "Admin" });

            });


        }

        public void ConfigureServices(IServiceCollection services)
        {
            //// Add EF services to the services container.
            //services.AddEntityFramework()
            //    .AddSqlServer()
            //    .AddDbContext<ApplicationDbContext>(options =>
            //        options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            //// Add Identity services to the services container.
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddIdentity<BandMember, Role>(options =>
            {

            })
            .AddUserStore<DefaultUserStore>()
            .AddRoleStore<DefaultRoleStore>();


            // Configure the options for the authentication middleware.
            // You can add options for Google, Twitter and other middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            services.Configure<FacebookAuthenticationOptions>(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            services.Configure<MicrosoftAccountAuthenticationOptions>(options =>
            {
                options.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
                options.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
            });
        }


    }
}
