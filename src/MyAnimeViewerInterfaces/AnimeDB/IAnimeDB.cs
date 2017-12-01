using System.Drawing;
using System.Threading.Tasks;

namespace MyAnimeViewerInterfaces.AnimeDB
{
    public interface IAnimeDB
    {
        /// <summary>
        /// Image to be displayed when logging in.
        /// Typically the logo for the website the Plugin is supporting.
        /// </summary>
        Image loginLogo { get; }

        /// <summary>
        /// The Instance of the Plugin's IAnimeDBLogin Class.
        /// </summary>
        IAnimeDBLogin login { get; }

        /// <summary>
        /// The Instance of the Plugin's IAnimeDBList Class.
        /// </summary>
        IAnimeDBList animeList { get; }

        /// <summary>
        /// The authenticated user's information.
        /// </summary>
        IAnimeDBUser user { get; }

        /// <summary>
        /// Get the series model for an anime by ID.
        /// </summary>
        /// <param name="id">The ID of the series.</param>
        /// <returns>The series model for the anime.</returns>
        Task<IAnimeDBSeriesModel> GetSeriesModel(int id);
    }
}
