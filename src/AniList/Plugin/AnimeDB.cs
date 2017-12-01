using AniList.Enums;
using MyAnimeViewerInterfaces.AnimeDB;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AniList.Plugin
{
    public class AnimeDB : IAnimeDB
    {
        public IAnimeDBList animeList { get; private set; }

        public IAnimeDBLogin login { get; private set; }

        public Image loginLogo => Properties.Resources.AniList_logo;
        
        public IAnimeDBUser user { get; private set; }
        
        internal async Task GetAuthenticatedUser()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = await (login as AnimeDBLogin).AuthenticationHeader();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue((login as AnimeDBLogin).ContentTypeHeader));
                    var response = await client.GetAsync(Settings.Instance.BaseUrl + "user");
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);
                    var result = new AnimeDBUser((int)json.id, (string)json.display_name, (string)json.image_url_med);
                    string title = json.title_language;
                    result.TitleLanguage = (TitleLanguage)Enum.Parse(typeof(TitleLanguage), title.Replace(" ", ""), true);
                    result.AdultContent = json.adult_content;
                    user = result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IAnimeDBSeriesModel> GetSeriesModel(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = await (login as AnimeDBLogin).AuthenticationHeader();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue((login as AnimeDBLogin).ContentTypeHeader));
                    var response = await client.GetAsync(Settings.Instance.BaseUrl + $"anime/{id}/page");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        dynamic json = JsonConvert.DeserializeObject(responseString);
                        return new AnimeDBSeriesModel(json);
                    }
                    else
                    {
                        throw new Exception($"Unable to retrive information of anime by id: {id}.");
                    }
                }
            }
            catch (Exception e)
            {

                throw e; 
            }
        }

        public void Load()
        {
            login = new AnimeDBLogin();
            animeList = new AnimeDBList();
        }

        public void Unload()
        {
            login.Logout();
            login = null;
            animeList = null;
        }
    }
}
