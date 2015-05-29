using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class Site 
    {
        public string Name { get; set; }

        public DateTime Time { get; set; }

        public IEnumerable<PageData> Pages { get; set; }

        public IEnumerable<PostData> Posts { get; set; }
        
        public IEnumerable<PostData> RelatedPosts { get; set; }

        public IEnumerable<FileData> StaticFiles { get; set; }

        public IEnumerable<string> HtmlPages { get; set; }

        public IEnumerable<IEnumerable> Collections { get; set; }

        public IEnumerable<object> Data { get; set; }

        public IEnumerable<object> Documents { get; set; }

        public IDictionary<string, PostData> Categories { get; set; }

        public IDictionary<string, PostData> Tags { get; set; }




    }
}
