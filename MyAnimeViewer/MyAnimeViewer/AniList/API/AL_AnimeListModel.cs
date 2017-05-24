using Newtonsoft.Json.Linq;
using System;

namespace MyAnimeViewer.AniList.API
{
    public class AL_AnimeListModel
    {
        private AL_AnimeModel m_anime;  // The anime model.
        private int m_score;            // The user's score given to the anime.
        private DateTime m_startedOn;   // When the user started the anime.
        private DateTime m_finishedOn;  // When the user finished the anime.
        private int m_episodesWatched;  // How many episodes the user has watched.
        private int m_rewatched;        // How many times the user has rewatched the anime.
        private string m_notes;         // Any notes the user has added to the anime.

        public AL_AnimeListModel(AL_AnimeModel anime, int score, DateTime startedOn, DateTime finishedOn, int episodesWatched, int rewatched, string notes)
        {
            m_anime = anime;
            m_score = score;
            m_startedOn = startedOn;
            m_finishedOn = finishedOn;
            m_episodesWatched = episodesWatched;
            m_rewatched = rewatched;
            m_notes = notes;
        }

        public AL_AnimeListModel(JObject animeListModel)
        {
            m_anime = new AL_AnimeModel((JObject)animeListModel.Property("anime").Value);
            m_score = (int)animeListModel.Property("score").Value;
            m_episodesWatched = (int)animeListModel.Property("episodes_watched").Value;
            m_rewatched = (int)animeListModel.Property("rewatched").Value;
            m_notes = (string)animeListModel.Property("notes").Value;
        }
    }
}
