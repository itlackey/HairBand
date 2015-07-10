using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace HairBand
{
    public interface IBootstrapper
    {
        void Initialize(IConfiguration config);

        void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory);

        void ConfigureServices(IServiceCollection services);
    }
}