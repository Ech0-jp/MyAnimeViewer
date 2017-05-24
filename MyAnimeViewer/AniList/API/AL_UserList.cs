using MyAnimeViewer.Errors;
using MyAnimeViewer.Utility.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyAnimeViewer.AniList.API
{
    /// <summary>
    /// This stores all of the sub-lists in the users list (Eg; Completed, Watching, etc.)
    /// 
    /// @author Robert Andrew Gray
    /// @date 1/26/2017
    /// </summary>
    public class AL_UserList
    {
        private List<AL_AnimeList> m_animeList; // A list containing all of the sub-lists in the users list.
        public List<AL_AnimeList> AnimeList { get { return m_animeList; } }

        /// <summary>
        /// Find a list by name.
        /// </summary>
        /// <param name="listName">The name of the list</param>
        /// <returns>AL_AnimeList</returns>
        public AL_AnimeList FindList(string listName)
        {
            return m_animeList.FirstOrDefault(o => o.ListName == listName);
        }

        /// <summary>
        /// Find a series entry from any list.
        /// </summary>
        /// <param name="name">Title of the series.</param>
        /// <returns>AL_AnimeListModel</returns>
        public AL_AnimeListModel FindAnime(string name)
        {
            foreach (var item in m_animeList)
            {
                var result = item.FindAnime(name);
                if (result != null && result.Title == name)
                    return result;
            }
            return null;
        }

        public AL_AnimeListModel FindAnime(int id)
        {
            foreach (var item in m_animeList)
            {
                var result = item.FindAnime(id);
                if (result != null && result.ID == id)
                    return result;
            }
            return null;
        }

        public AL_UserList()
        {
            m_animeList = new List<AL_AnimeList>();
        }

        /// <summary>
        /// The task to generate user's list asyncronously.
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>True if success</returns>
        public async Task<bool> GenerateUserList(int userID)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);
                    var response = await client.GetAsync(Config.Instance.AniList_BaseUrl + string.Format("user/{0}/animelist", userID));
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    foreach (var list in json.lists)
                    {
                        m_animeList.Add(new AL_AnimeList(list.Name));
                        foreach (var item in list)
                        {
                            foreach (var property in item)
                            {
                                m_animeList.Last().AddEntry(new AL_AnimeListModel(property, list.Name));
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Error generating list.", $"An unexpected error occured when generating the user's list. {e}", true);
                return false;
            }
        }

        /// <summary>
        /// Create a new anime entry and add it to the user's list and AniList database.
        /// </summary>
        /// <returns>True if success</returns>
        public async Task<bool> CreateAnimeEntry(AL_AnimeModel anime, string status, int episodesWatched, int score, int rewatched, string notes, DateTime startDate, DateTime finishDate)
        {
            try
            {
                AL_AnimeListModel entry = new AL_AnimeListModel(anime.ID, anime.TitleRomaji, anime.MediumImageURL, anime.Type, anime.TotalEpisodes);
                entry.EditInfo(status, score, episodesWatched, rewatched, notes, startDate, finishDate);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);
                    var values = new Dictionary<string, string>
                    {
                        { "id", entry.ID.ToString() },
                        { "list_status", status.ToLower() },
                        { "score", score.ToString() },
                        { "score_raw", (score * 10).ToString() },
                        { "episodes_watched", episodesWatched.ToString() },
                        { "rewatched", entry.Rewatched.ToString() },
                        { "notes", entry.Notes },
                        { "started_on", startDate.ToString("yyyy/MM/dd") },
                        { "finished_on", finishDate.ToString("yyyy/MM/dd") },
                        { "advanced_rating_scores", "" },
                        { "custom_lists", "" },
                        { "hidden_default", "0" }
                    };
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(Config.Instance.AniList_BaseUrl + "animelist", content);

                    if (response.IsSuccessStatusCode)
                    {
                        foreach (var list in m_animeList)
                        {
                            if (list.ListName == status)
                                list.AddEntry(entry);
                        }
                        return true;
                    }
                    else
                    {
                        ErrorManager.AddError("Error adding entry.", $"An error occured while attempting to add your entry. {response.StatusCode}: {response.ReasonPhrase}", true);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Error adding entry.", $"An error occured while attempting to add your entry. {e}", true);
                throw;
            }
        }

        /// <summary>
        /// Edit an existing entry in the user's list and update the information to AniList's database.
        /// </summary>
        /// <returns>True if success</returns>
        public async Task<bool> EditAnimeEntry(int id, string status, int episodesWatched, int score, int rewatched, string notes, DateTime startDate, DateTime finishDate)
        {
            try
            {
                AL_AnimeListModel entry = FindAnime(id);
                bool newStatus = !(status == entry.ListStatus);
                string oldStatus = entry.ListStatus;
                entry.EditInfo(status, score, episodesWatched, rewatched, notes, startDate, finishDate);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);

                    // Need to check if the status is "on_hold" or "plan_to_watch" and change them to "on-hold" or "plan to watch" due to inconsistencies in AniList.co API.
                    // AniList.co sends us "on_hold" and "plan_to_watch" requires "on-hold" or "plan to watch" when sending data or else results in error.
                    string _status;
                    switch (status)
                    {
                        case "on_hold":
                            _status = "on-hold";
                            break;
                        case "plan_to_watch":
                            _status = "plan to watch";
                            break;
                        default:
                            _status = status;
                            break;
                    }
                    
                    var values = new Dictionary<string, string>
                    {
                        { "id", entry.ID.ToString() },
                        { "list_status", _status.ToLower() },
                        { "score", score.ToString() },
                        { "score_raw", (score * 10).ToString() },
                        { "episodes_watched", episodesWatched.ToString() },
                        { "rewatched", entry.Rewatched.ToString() },
                        { "notes", entry.Notes },
                        { "advanced_rating_scores", "" },
                        { "custom_lists", "" },
                        { "hidden_default", "0" }
                    };

                    DateTime dt = new DateTime();
                    if (startDate != dt)
                        values.Add("started_on", startDate.ToString("yyyy/MM/dd"));
                    if (finishDate != dt)
                        values.Add("finished_on", finishDate.ToString("yyyy/MM/dd"));

                    Log.Debug($"<{string.Join(", ", values.Select(kv => kv.Key + "=" + kv.Value).ToArray())}");
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PutAsync(Config.Instance.AniList_BaseUrl + "animelist", content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (newStatus)
                        {
                            FindList(status).AddEntry(entry);
                            FindList(oldStatus).RemoveEntry(entry);
                        }
                        return true;
                    }
                    else
                    {
                        ErrorManager.AddError("Error editing entry.", $"An error occured while attempting to edit your entry. {response.StatusCode}: {response.ReasonPhrase}", true);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Error editing entry.", $"A fatal error occured while attempting to edit your entry. {e}", true);
                throw;
            }
        }
    }
}
