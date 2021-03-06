﻿using System;
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
        private const string _riffDirectory = "/app_data/_riffs/";

        private string _name;

        private Dictionary<string, object> _args = new Dictionary<string, object>();



        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            if (markup.Contains(','))
            {
                var args = markup.Split(',');
                _name = args.FirstOrDefault().Trim();

                for (int i = 1; i < args.Count(); i++)
                {
                    var item = args.ElementAt(i);
                    if (item.Contains(':'))
                    {
                        var parts = item.Split(':');
                        _args.Add(parts.First().Trim(), parts.Last().Trim());
                    }
                    else
                    {
                        _args.Add(i.ToString(), item);
                    }
                }
            }
            else
                _name = markup.Trim();
        }

        public override void Render(Context context, TextWriter result)
        {
            base.Render(context, result);

            var host = new HttpContextAccessor().HttpContext.ApplicationServices.GetService
                  (typeof(IHostingEnvironment)) as IHostingEnvironment;

            var riffFolder = host.WebRootPath + _riffDirectory;

            var fileName = Path.Combine(riffFolder, String.Format("{0}.html", _name));

            var riffContents = File.ReadAllText(fileName);

            var riffContext = new Context(context.Environments, context.Scopes.FirstOrDefault(), context.Registers, true);

            context.Merge(Hash.FromDictionary(_args));

            Template.FileSystem = new HairBandFileSystem(riffFolder);

            var template = Template.Parse(riffContents);

            var output = template.Render(new RenderParameters() { Context = riffContext });

            result.Write(output);

        }

    }
}
