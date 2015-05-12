using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand.Core
{
    public class PageDataProvider
    {

        public async Task<dynamic> GetPageData()
        {

            dynamic yaml = new YamlDotNet.Dynamic.DynamicYaml("");

            var title = yaml.Title;
          
            return Task.FromResult(yaml);


        }

    }
}
