using MyAnimeViewer.Enums.AniList;
using MyAnimeViewer.Errors;
using MyAnimeViewer.Utility.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyAnimeViewer.AniList.API
{
    /// <summary>
    /// This class is a storage container used to store the users data for each list entry in the users list.
    /// 
    /// @author Robert Andrew Gray
    /// @date 1/26/2017
    /// </summary>
    public class AL_AnimeListModel
    {
#region PrivateVars
        private string m_listStatus;    // The anime sub-list this entry belongs to.
        private int m_id;               // The series ID.
        private string m_title;         // The series title.
        private string m_imageURL;      // The series image URL.
        private AL_MediaType m_type;    // The type of series.
        private AL_AnimeStatus m_seriesStatus;
        private int m_score;            // The user's score given to the anime.
        private DateTime m_startedOn;   // When the user started the anime.
        private DateTime m_finishedOn;  // When the user finished the anime.
        private int m_episodesWatched;  // How many episodes the user has watched.
        private int m_totalEpisodes;    // The total amount of episodes for the series.
        private int m_rewatched;        // How many times the user has rewatched the anime.
        private string m_notes;         // Any notes the user has added to the anime.
#endregion

#region Getters
        public string ListStatus { get { return m_listStatus; } }
        public string ListStatusFormatted
        {
            get
            {
                string temp = m_listStatus.Replace("_", " ");
                return Regex.Replace(temp, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
            }
        }
        public int ID { get { return m_id; } }
        public string Title { get { return m_title; } }
        public string ImageURL { get { return m_imageURL; } }
        public AL_MediaType Type { get { return m_type; } }
        public AL_AnimeStatus SeriesStatus { get { return m_seriesStatus; } }
        public int Score { get { return m_score; } }
        public DateTime StartedOn { get { return m_startedOn; } }
        public DateTime FinishedOn { get { return m_finishedOn; } }
        public int EpisodesWatched { get { return m_episodesWatched; } }
        public int TotalEpisodes { get { return m_totalEpisodes; } }
        public int Rewatched { get { return m_rewatched; } }
        public string Notes { get { return m_notes; } }
#endregion

        public AL_AnimeListModel(int id, string title, string imageURL, AL_MediaType type, int totalEpisodes)
        {
            m_id = id;
            m_title = title;
            m_imageURL = imageURL;
            m_type = type;
            m_totalEpisodes = totalEpisodes;
        }

        public AL_AnimeListModel(JObject animeListModel, string listName)
        {
            m_listStatus = listName;
            JObject anime = (JObject)animeListModel.Property("anime").Value;
            m_id = (int)anime.Property("id").Value;
            switch (Core.MainWindow.AniListUser.TitleLanguage)
            { 
                case AL_TitleLanguage.English:
                    m_title = (string)anime.Property("title_english").Value;
                    break;
                case AL_TitleLanguage.Romaji:
                    m_title = (string)anime.Property("title_romaji").Value;
                    break;
                case AL_TitleLanguage.Japanese:
                    m_title = (string)anime.Property("title_japanese").Value;
                    break;
            }
            m_imageURL = (string)anime.Property("image_url_med").Value;
            string type = (string)anime.Property("type").Value;
            m_type = (AL_MediaType)Enum.Parse(typeof(AL_MediaType), type.Replace(" ", ""), true);
            string status = (string)anime.Property("airing_status");
            m_seriesStatus = (AL_AnimeStatus)Enum.Parse(typeof(AL_AnimeStatus), status.Replace(" ", ""), true);
            m_score = (int)animeListModel.Property("score").Value;
            string startedOn = (string)animeListModel.Property("started_on").Value;
            m_startedOn = Convert.ToDateTime(startedOn);
            string finishedOn = (string)animeListModel.Property("finished_on").Value;
            m_finishedOn = Convert.ToDateTime(finishedOn);
            m_episodesWatched = (int)animeListModel.Property("episodes_watched").Value;
            m_totalEpisodes = (int)anime.Property("total_episodes").Value;
            m_rewatched = (int)animeListModel.Property("rewatched").Value;
            m_notes = (string)animeListModel.Property("notes").Value;
        }

        /// <summary>
        /// Edit the user's info on the series.
        /// 
        /// NOTE: Leave NULL or -1 (for ints) if no changes want to be made.
        /// </summary>
        public void EditInfo(string status, int score, int episodesWatched, int rewatched, string notes, DateTime startDate, DateTime finishDate)
        {
            m_listStatus = status == null ? m_listStatus : status;
            m_score = score == -1 ? m_score : score;
            m_episodesWatched = episodesWatched == -1 ? m_episodesWatched : episodesWatched;
            m_rewatched = rewatched == -1 ? m_rewatched : rewatched;
            m_notes = notes == null ? m_notes : notes;
            m_startedOn = startDate == null ? m_startedOn : startDate;
            m_finishedOn = finishDate == null ? m_finishedOn : finishDate;
            Log.Info($"Updated information for {m_title}");
        }

        /// <summary>
        /// Retrieve the "Series Model" from AniLists database. (NOTE: the series data, not the users)
        /// </summary>
        /// <returns>AL_AnimeModel</returns>
        public async Task<AL_AnimeModel> GetAnimeInfo()
        {
            return await AL_AnimeModel.GetAnimeInfo(m_id);
        }

        /// <summary>
        /// Retrieves the "Series Model" from AniLists database including the following:
        /// Up to 9 small model characters (ordered by main role) with Japanese small model actors for anime.
        /// Up to 9 small model staff
        /// Up to 2 small model reviews with their users.
        /// Relations (small model)
        /// Anime/Manga relations (small model)
        /// Studios (anime)
        /// External links (anime)
        /// </summary>
        /// <returns>AL_AnimeModel</returns>
        public async Task<AL_AnimeModel> GetAnimePage()
        {
            return await AL_AnimeModel.GetAnimePage(m_id);
        }
    }
}
