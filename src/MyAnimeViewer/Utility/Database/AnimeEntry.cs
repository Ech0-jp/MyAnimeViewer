using System;
using System.Data;

namespace MyAnimeViewer.Utility.Database
{
    public class AnimeEntry
    {
        public int id { get; }
        public string title { get; }
        public string imageURL { get; }
        public string mediaType { get; }
        public int score { get; }
        public int episodesWatched { get; }
        public int totalEpisodes { get; }

        public AnimeEntry(DataRow entry)
        {
            id = Convert.ToInt32(entry["id"]);
            title = Convert.ToString(entry["title"]);
            imageURL = Convert.ToString(entry["image_url"]);
            mediaType = Convert.ToString(entry["media_type"]);
            score = Convert.ToInt32(entry["score"]);
            episodesWatched = Convert.ToInt32(entry["episodes_watched"]);
            totalEpisodes = Convert.ToInt32(entry["total_episodes"]);
        }
    }
}
