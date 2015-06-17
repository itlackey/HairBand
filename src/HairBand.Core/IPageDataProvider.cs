using System.Collections.Generic;
using System.Threading.Tasks;

namespace HairBand
{
    public interface IPageDataProvider
    {
        //Task<PageData> GetPageData(string url);

        Task<PageData> GetPageAsync(string url);

        Task<IEnumerable<PageData>> GetPagesAsync();
    }
}