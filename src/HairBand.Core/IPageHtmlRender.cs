using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public interface IPageHtmlRender
    {
        Task<string> GetHtmlAsync(string url);
    }
}
