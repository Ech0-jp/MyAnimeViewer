using MyAnimeViewer.Utility;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyAnimeViewer.AniList.API
{
    public class AL_User
    {
        private int m_id;
        private string m_displayName;
        private int m_animeTime;
        private int m_mangaChap;
        private string m_imageUrlMedium;
        private string m_titleLanguage;

        public int getID { get { return m_id; } }

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
                    m_imageUrlMedium = json.image_url_med;
                    m_titleLanguage = json.title_language;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error retrieving user data. \n" + e.Message, "AL_User");
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
