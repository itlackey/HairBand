using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotLiquid;
using Microsoft.AspNet.Hosting;
using DotLiquid.FileSystems;

namespace HairBand
{
    public class Riff : DotLiquid.Tag
    {
        private string _name;



        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _name = markup.Trim();
        }

        protected override void Parse(List<string> tokens)
        {
            base.Parse(tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            base.Render(context, result);

            var host = new HttpContextAccessor().HttpContext.ApplicationServices.GetService
                  (typeof(IHostingEnvironment)) as IHostingEnvironment;

            var riffFolder = host.WebRootPath + "/app_data/_riffs/";

            var fileName = Path.Combine(riffFolder, String.Format("{0}.html", _name));
            
            var riffContents = File.ReadAllText(fileName);

            var riffContext = new Context(context.Environments, context.Scopes.FirstOrDefault(), context.Registers, true);

            Template.FileSystem = new HairBandFileSystem(riffFolder);

            var template = Template.Parse(riffContents);

            var output = template.Render(new RenderParameters() { Context = riffContext });

            result.Write(output);

        }

    }
}
