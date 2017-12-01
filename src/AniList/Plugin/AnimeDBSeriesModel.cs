using MyAnimeViewerInterfaces.AnimeDB;
using System;
using MyAnimeViewerInterfaces.AnimeDB.DataTypes;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace AniList.Plugin
{
    public class AnimeDBSeriesModel : IAnimeDBSeriesModel
    {
        public double average_score { get; private set; }

        public Character[] characters { get; private set; }

        public string description { get; private set; }

        public int duration { get; private set; }

        public string[] genres { get; private set; }

        public int id { get; private set; }

        public string image_url { get; private set; }

        public string main_studio { get; private set; }

        public string media_type { get; private set; }

        public int popularity { get; private set; }

        public string preview_url { get; private set; }

        public Relation[] relations { get; private set; }

        public AiringSeason season { get; private set; }

        public SeriesStatus series_status { get; private set; }

        public DateTime start_date { get; private set; }

        public string title { get; private set; }

        public AnimeDBSeriesModel(JObject entry)
        {
            average_score = (double)entry.Property("average_score").Value;

            characters = new Character[entry.Property("characters").Count];
            foreach (var item in entry.Property("characters").Value.Children().Select((value, i) => new { i, value }))
            {
                var props = item.value.Children<JProperty>();
                Character character = new Character();
                character.id = (int)props.FirstOrDefault(x => x.Name == "id").Value;
                character.name = (string)props.FirstOrDefault(x => x.Name == "name_last").Value + 
                                 (string)props.FirstOrDefault(x => x.Name == "name_first").Value;
                var actor = props.FirstOrDefault(x => x.Name == "actor").Value.First.Children<JProperty>();
                character.actor_name = (string)actor.FirstOrDefault(x => x.Name == "name_last").Value +
                                       (string)actor.FirstOrDefault(x => x.Name == "name_first").Value;
                character.image_url = (string)props.FirstOrDefault(x => x.Name == "image_url_med").Value;
                characters[item.i] = character;
            }

            description = (string)entry.Property("description").Value;
            genres = entry.Property("genres").Value.ToObject<string[]>();
            id = (int)entry.Property("id").Value;
            image_url = (string)entry.Property("image_url_med").Value;
            main_studio = (string)entry.Property("studio").Value;
            media_type = (string)entry.Property("type").Value;
            popularity = (int)entry.Property("popularity").Value;
            preview_url = (string)entry.Property("youtube_id");

            relations = new Relation[entry.Property("relations").Count];
            foreach (var item in entry.Property("relations").Value.Children().Select((value, i) => new { i, value }))
            {
                var props = item.value.Children<JProperty>();
                Relation relation = new Relation();
                relation.id = (int)props.FirstOrDefault(x => x.Name == "id").Value;
                relation.title = (string)props.FirstOrDefault(x => x.Name == "title_romaji").Value;
                relation.image_url = (string)props.FirstOrDefault(x => x.Name == "image_url_med").Value;
                relation.relation_type = (string)props.FirstOrDefault(x => x.Name == "relation_type").Value;
                relations[item.i] = relation;
            }

            string season_raw = (string)entry.Property("season").Value;
            if (!string.IsNullOrEmpty(season_raw))
            {
                int y = Convert.ToInt32($"20{season_raw[1]}{season_raw[2]}");
                if (y > DateTime.Now.Year)
                    y = Convert.ToInt32($"19{season_raw[1]}{season_raw[2]}");
                season = new AiringSeason
                {
                    season = (AiringSeason.Season)Convert.ToInt32(season_raw[0]),
                    year = y
                };
            }

            series_status = (SeriesStatus)Enum.Parse(typeof(SeriesStatus), ((string)entry.Property("airing_status")).Replace(" ", ""), true);
            start_date = Convert.ToDateTime((string)entry.Property("start_date").Value);
            title = (string)entry.Property("title_romaji").Value;
        }
    }
}
