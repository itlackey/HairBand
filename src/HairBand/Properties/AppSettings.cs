using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class AppSettings
    {

        public AppSettings()
        {
            this.Theme = "default";
            this.Title = "Hair Band (Default)";
        }
        public string Title { get; set; }

        public string Theme { get; set; }
    }
}
