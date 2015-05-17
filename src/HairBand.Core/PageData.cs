using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class PageData
    {
        public string Url { get; set; }

        public IDictionary<string, string> Settings { get; set; }

        public string Content { get; set; }
    }
}
