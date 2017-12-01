using MyAnimeViewerInterfaces.AnimeDB.DataTypes;
using System;

namespace MyAnimeViewerInterfaces.AnimeDB
{
    /// <summary>
    /// The information for a given anime series.
    /// </summary>
    public interface IAnimeDBSeriesModel
    {
        /// <summary>
        /// ID for the anime.
        /// </summary>
        int id { get; }

        /// <summary>
        /// The title of the series.
        /// </summary>
        string title { get; }

        /// <summary>
        /// The description for the series.
        /// </summary>
        string description { get; }

        /// <summary>
        /// The image URL for the series.
        /// </summary>
        string image_url { get; }

        /// <summary>
        /// The type of media this entry is. EG; TV || OVA || Special || etc..
        /// </summary>
        string media_type { get; }

        /// <summary>
        /// The current status of the series.
        /// </summary>
        SeriesStatus series_status { get; }

        /// <summary>
        /// The date the series started.
        /// </summary>
        DateTime start_date { get; }

        /// <summary>
        /// The season the anime aired.
        /// </summary>
        AiringSeason season { get; }

        /// <summary>
        /// The average user given score.
        /// </summary>
        double average_score { get; }

        /// <summary>
        /// The popularity of the anime.
        /// </summary>
        int popularity { get; }

        /// <summary>
        /// The main studio for the series.
        /// </summary>
        string main_studio { get; }

        /// <summary>
        /// The duration of each episode.
        /// </summary>
        int duration { get; }

        /// <summary>
        /// The genres for the series.
        /// </summary>
        string[] genres { get; }

        /// <summary>
        /// The related series.
        /// </summary>
        Relation[] relations { get; }

        /// <summary>
        /// The characters for the series.
        /// </summary>
        Character[] characters { get; }

        /// <summary>
        /// The URL for the series Preview Video.
        /// </summary>
        string preview_url { get; }
    }

    public struct AiringSeason
    {
        public enum Season
        {
            Winter = 1,
            Spring = 2,
            Summer = 3,
            Fall = 4
        }
        public Season season;
        public int year;
    }

    public struct Relation
    {
        public int id;
        public string title;
        public string image_url;
        public string relation_type;
    }

    public struct Character
    {
        public int id;
        public string name;
        public string actor_name;
        public string image_url;
    }
}
