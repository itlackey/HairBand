using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public interface IPostDataProvider
    {
        Task<PostData> GetPostAsync(string url);

        Task<IEnumerable<PostData>> GetPostsAsync();
    }
}
