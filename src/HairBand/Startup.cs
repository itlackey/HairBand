using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Routing;
using Microsoft.Data.Entity;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using Microsoft.Framework.Runtime;
using HairBand.Models;
using HairBand.Web;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.StaticFiles;
using System.Reflection;

namespace HairBand
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            var configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                configuration.AddUserSecrets();
            }
            configuration.AddEnvironmentVariables();

            Configuration = configuration;

            //ToDo --- how to load these in coreclr?
            _bootstrappers.Add(new CoreBootstrapper());
            _bootstrappers.Add(new AdminBootstrapper());

            //List in config file?
           // Activator.CreateInstance(Type.GetType(""));

            foreach (var item in _bootstrappers)
            {
                item.Initialize(Configuration);
            }


        }

        private List<IBootstrapper> _bootstrappers = new List<IBootstrapper>();


        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Application settings to the services container.
            services.Configure<AppSettings>(Configuration.GetSubKey("AppSettings"));

       



            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();
      
            foreach (var item in _bootstrappers)
                item.ConfigureServices(services);



        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            
            foreach (var item in _bootstrappers)
                item.Configure(app, env, loggerfactory);

        }
    }
}
