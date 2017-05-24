using MyAnimeViewer.Enums.AniList;
using Newtonsoft.Json.Linq;
using System;

namespace MyAnimeViewer.AniList.API
{
    public class AL_AnimeModel
    {
        private int m_id;                       // The series ID.
        private bool m_adult;                   // True for adult series (Hentai). This does not include ecchi.
        private string m_titleEnglish;          // The English title for the series.
        private string m_titleRomaji;           // The Romaji title for the series.
        private string m_titleJapanese;         // The Japanese title for the series.
        private string[] m_synonyms;            // Alternative titles.
        private AL_AnimeStatus m_airingStatus;  // The Airing Status of the series.
        private int m_season;                   // First 2 numbers are the year (16 is 2016). Last number is the season starting 1 (3 is summer) <<AniList related only>>
        private int m_totalEpisodes;            // The total episodes for the series.
        private AL_MediaType m_type;            // The media type of the series.
        private string[] m_genres;              // The series' genres.
        private float m_averageScore;           // The average score given to the series.
        private string m_largeImageURL;         // The large image url.
        private string m_mediumImageURL;        // The medium image url.
        private string m_smallImageURL;         // the small image url.

        public AL_AnimeModel(int id, bool adult, string englishTitle, string romajiTitle, string japaneseTitle, string[] synonyms, AL_AnimeStatus airingStatus,
                             int season, int totalEpisodes, AL_MediaType type, string[] genres, float averageScore, string largeImageURL, string mediumImageURL, 
                             string smallImageURL)
        {
            m_id = id;
            m_adult = adult;
            m_titleEnglish = englishTitle;
            m_titleRomaji = romajiTitle;
            m_titleJapanese = japaneseTitle;
            m_synonyms = synonyms;
            m_airingStatus = airingStatus;
            m_season = season;
            m_totalEpisodes = totalEpisodes;
            m_type = type;
            m_genres = genres;
            m_averageScore = averageScore;
            m_largeImageURL = largeImageURL;
            m_mediumImageURL = mediumImageURL;
            m_smallImageURL = smallImageURL;
        }

        public AL_AnimeModel(JObject animeModel)
        {
            m_id = (int)animeModel.Property("id").Value;
            m_adult = (bool)animeModel.Property("adult").Value;
            m_titleEnglish = (string)animeModel.Property("title_english").Value;
            m_titleRomaji = (string)animeModel.Property("title_romaji").Value;
            m_titleJapanese = (string)animeModel.Property("title_japanese").Value;
            m_synonyms = animeModel.Property("synonyms").Value.ToObject<string[]>();
            string airingStatus = (string)animeModel.Property("airing_status").Value;
            m_airingStatus = (AL_AnimeStatus)Enum.Parse(typeof(AL_AnimeStatus), airingStatus.Replace(" ", ""), true);
            m_season = (int)animeModel.Property("season").Value;
            m_totalEpisodes = (int)animeModel.Property("total_episodes").Value;
            string type = (string)animeModel.Property("type").Value;
            m_type = (AL_MediaType)Enum.Parse(typeof(AL_MediaType), type.Replace(" ", ""), true);
            m_genres = animeModel.Property("genres").Value.ToObject<string[]>(); ;
            m_averageScore = (int)animeModel.Property("average_score").Value;
            m_largeImageURL = (string)animeModel.Property("image_url_lge").Value;
            m_mediumImageURL = (string)animeModel.Property("image_url_med").Value;
            m_smallImageURL = (string)animeModel.Property("image_url_sml").Value;
        }
    }
}
