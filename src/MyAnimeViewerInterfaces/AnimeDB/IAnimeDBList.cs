using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAnimeViewerInterfaces.AnimeDB
{
    public interface IAnimeDBList
    {
        /// <summary>
        /// Retrieve the user's list from supported platform.
        /// </summary>
        /// <returns>A List of IAnimeDBUserEntry (user's entries)</returns>
        Task<List<IAnimeDBUserEntry>> GetUsersList();
    }
}
