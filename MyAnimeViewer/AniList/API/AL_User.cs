using MyAnimeViewer.Enums.AniList;
using MyAnimeViewer.Utility;
using MyAnimeViewer.Utility.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyAnimeViewer.AniList.API
{
    /// <summary>
    /// This stores the users data. Things such as user ID, display name, etc.
    /// 
    /// @author Robert Andrew Gray
    /// @date 1/26/2017
    /// </summary>
    public class AL_User
    {
#region PrivateVars
        private int m_id;                           // The users ID
        private string m_displayName;               // The users display name
        private int m_animeTime;                    // The amount of time watched in anime.
        private int m_mangaChap;                    // The amount of chapters read in manga.
        private bool m_adultContent;                // If the user is able to see adult content or no.
        private string m_imageUrlMedium;            // The users profile image.
        private AL_TitleLanguage m_titleLanguage;   // The users preferred language for titles.
#endregion

#region Getters
        public int ID { get { return m_id; } }
        public string DisplayName { get { return m_displayName; } }
        public int AnimeTime { get { return m_animeTime; } }
        public int MangaChap { get { return m_mangaChap; } }
        public bool AdultContent { get { return m_adultContent; } }
        public string ImageUrlMedium { get { return m_imageUrlMedium; } }
        public AL_TitleLanguage TitleLanguage { get { return m_titleLanguage; } }
#endregion

        public AL_User()
        {
            
        }

        /// <summary>
        /// Retrieves the currently authenticated user model from AniList.co
        /// </summary>
        public async Task<bool> GetAuthenticatedUser()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);
                    var response = await client.GetAsync(Config.Instance.AniList_BaseUrl + "user");
                    var responseString = await response.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    m_id = json.id;
                    m_displayName = json.display_name;
                    m_animeTime = json.anime_time;
                    m_mangaChap = json.manga_chap;
                    m_adultContent = json.adult_content;
                    m_imageUrlMedium = json.image_url_med;
                    string title = json.title_language;
                    m_titleLanguage = (AL_TitleLanguage)Enum.Parse(typeof(AL_TitleLanguage), title.Replace(" ", ""), true);
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Error retrieving user date.\n{e}");
                return false;
            }
        }

        //****WILL NEED TO BE REFORMATED TO RETURN A LIST<ANIME> OF THE CURRENTLY AIRING ANIME!****

        /// <summary>
        /// Returns anime list entry with small model anime, where the anime is currently airing and being currently watched by the user.
        /// </summary>
        public async void GetAiring()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);
                    var response = await client.GetAsync(Config.Instance.AniList_BaseUrl + "user/airing");
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Format the response and return to user...
                    // Will probably have to build this as a task to return type List<Anime>
                }
            }
            catch (Exception e)
            {
                // Add exception
                throw;
            }
        }
    }
}
