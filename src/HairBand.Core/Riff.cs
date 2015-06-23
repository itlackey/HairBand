using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotLiquid;
using Microsoft.AspNet.Hosting;

namespace HairBand
{
    public class Riff : DotLiquid.Tag
    {
        private string _name;

        

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _name = markup;
        }

        protected override void Parse(List<string> tokens)
        {
            base.Parse(tokens);
        }

        public override void Render(Context context, TextWriter result)
        {

            var host = new HttpContextAccessor().HttpContext.ApplicationServices.GetService
                  (typeof(IHostingEnvironment)) as IHostingEnvironment;

            var riffFolder = host.WebRootPath + "/app_data/_riffs/";

            var fileName = String.Format("{0}{1}.riff", riffFolder, _name);

        
            base.Render(context, result);
        }

    }
}
