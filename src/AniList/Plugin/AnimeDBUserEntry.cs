using AniList.Enums;
using MyAnimeViewerInterfaces.AnimeDB;
using Newtonsoft.Json.Linq;
using System;

namespace AniList.Plugin
{
    public class AnimeDBUserEntry : IAnimeDBUserEntry
    {
        public int id { get; }

        public string title { get; }

        public string image_url { get; }

        public string media_type { get; }

        public string series_status { get; }

        public string list_name { get; }

        public string list_status { get; set; }

        public int score { get; set; }

        public int episodes_watched { get; set; }

        public int total_episodes { get; }

        public string notes { get; set; }

        public int rewatched { get; set; }

        public AnimeDBUserEntry(string ListName, JObject entry)
        {
            list_name = ListName;
            JObject anime = (JObject)entry.Property("anime").Value;
            id = Convert.ToInt32(anime.Property("id").Value);
            switch ((Core.PluginController.AnimeDB.user as AnimeDBUser).TitleLanguage)
            {
                case TitleLanguage.English:
                    title = (string)anime.Property("title_english").Value;
                    break;
                case TitleLanguage.Japanese:
                    title = (string)anime.Property("title_japanese").Value;
                    break;
                default:
                    title = (string)anime.Property("title_romaji").Value;
                    break;
            }
            list_status = (string)entry.Property("list_status").Value;
            image_url = (string)anime.Property("image_url_med").Value;
            media_type = (string)anime.Property("type").Value;
            series_status = (string)anime.Property("airing_status");
            score = (int)entry.Property("score").Value;
            episodes_watched = (int)entry.Property("episodes_watched").Value;
            total_episodes = (int)anime.Property("total_episodes").Value;
            rewatched = (int)entry.Property("rewatched").Value;
            notes = (string)entry.Property("notes").Value;
        }
    }
}
