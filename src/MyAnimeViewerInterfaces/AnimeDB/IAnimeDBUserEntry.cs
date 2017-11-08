using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeViewerInterfaces.AnimeDB
{
    /// <summary>
    /// Defines an anime entry in the user's list.
    /// </summary>
    public interface IAnimeDBUserEntry
    {
        /// <summary>
        /// Anime ID of the list item.
        /// </summary>
        int id { get; }

        /// <summary>
        /// The title of the series.
        /// </summary>
        string title { get; }

        /// <summary>
        /// The image URL of the series.
        /// </summary>
        string image_url { get; }

        /// <summary>
        /// Watching || Completed || On-Hold || Dropped || Plan to Watch
        /// </summary>
        string list_status { get; set; }

        /// <summary>
        /// The type of media this entry is. EG; TV || OVA || Special || etc..
        /// </summary>
        string media_type { get; }

        /// <summary>
        /// The current status of the series. EG; Airing || Finished || Not Yet Aired || Cancelled
        /// </summary>
        string series_status { get; }

        /// <summary>
        /// The user's score of the entry. (1-10).
        /// </summary>
        int score { get; set; }

        /// <summary>
        /// How many episodes the user has watched in the series.
        /// </summary>
        int episodes_watched { get; set; }

        /// <summary>
        /// The total amount of episodes in the series.
        /// Return -1 if unknown.
        /// </summary>
        int total_episodes { get; }

        /// <summary>
        /// How many times the user has rewatched the series.
        /// </summary>
        int rewatched { get; set; }

        /// <summary>
        /// The note's the user has on the series.
        /// </summary>
        string notes { get; set; }
    }
}
