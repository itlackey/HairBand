using System.Collections.Generic;
using System.Threading.Tasks;

namespace HairBand
{
    public interface IPageDataProvider
    {
        //Task<PageData> GetPageData(string url);

        Task<IDictionary<string, object>> GetData(string url);

        Task<IEnumerable<IDictionary<string, object>>> GetPages();
    }
}