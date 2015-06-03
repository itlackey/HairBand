using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Internal;

namespace HairBand.Web
{
    internal class HairBandView : IView
    {
        public HairBandView(string path)
        {
            this.Path = path;
        }
        public string Path { get; private set; }

        public async Task RenderAsync(ViewContext context)
        {

            await context.Writer.WriteAsync("Hair band!!");
        }
    }
}