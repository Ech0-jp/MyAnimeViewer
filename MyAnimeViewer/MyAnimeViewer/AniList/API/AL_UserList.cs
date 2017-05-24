using MyAnimeViewer.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeViewer.AniList.API
{
    class AL_UserList
    {
        private List<AL_AnimeList> m_animeList;

        public AL_UserList(int userID)
        {
            GenerateUserList(userID);
        }

        private async void GenerateUserList(int userID)
        {
            bool result = await AsyncGenerateUserList(userID);
        }

        private async Task<bool> AsyncGenerateUserList(int userID)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);
                    var response = await client.GetAsync(Config.Instance.AniList_BaseUrl + string.Format("user/{0}/animelist", userID));
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    m_animeList = new List<AL_AnimeList>();

                    foreach (var list in json.lists)
                    {
                        m_animeList.Add(new AL_AnimeList(list.Name));
                        foreach (var array in list)
                        {
                            foreach (var item in array)
                            {
                                m_animeList.Last().AddEntry(new AL_AnimeListModel(item));
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
                return false;
            }
        }
    }
}
