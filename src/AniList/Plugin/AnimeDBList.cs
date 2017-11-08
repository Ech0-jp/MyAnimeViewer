using MyAnimeViewerInterfaces.AnimeDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AniList.Plugin
{
    public class AnimeDBList : IAnimeDBList
    {
        public async Task<List<IAnimeDBUserEntry>> GetUsersList()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = await (Core.PluginController.AnimeDB.login as AnimeDBLogin).AuthenticationHeader();
                    var baseurl = Settings.Instance.BaseUrl;
                    var user = Core.PluginController.AnimeDB.user;
                    var response = await client.GetAsync(Settings.Instance.BaseUrl + $"user/{Core.PluginController.AnimeDB.user.ID}/animelist");
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    List<IAnimeDBUserEntry> result = new List<IAnimeDBUserEntry>();
                    foreach (var list in json.lists)
                    foreach (var item in list)
                    foreach (var property in item)
                    {
                        result.Add(new AnimeDBUserEntry((string)list.Name, property));
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
