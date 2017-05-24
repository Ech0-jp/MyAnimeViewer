using MyAnimeViewer.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeViewer.Crunchyroll.API
{
    class CrunchyrollClient
    {
        private string m_sessionID;
        private string m_auth;
        private string m_expires;

        public CrunchyrollClient()
        {
            GenerateSessionID();
        }

        private void GenerateSessionID()
        {
            try
            {
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                HttpClient client = new HttpClient(handler);
                HttpResponseMessage response = client.GetAsync("http://www.crunchyroll.com/").Result;

                Dictionary<string, string> values = new Dictionary<string, string>();
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(new Uri("http://www.crunchyroll.com/")).Cast<Cookie>();
                foreach (var item in responseCookies)
                {
                    values.Add(item.Name, item.Value);
                }
                m_sessionID = values.ElementAt(0).Value;
            }
            catch (Exception e)
            {
                Log.Error($"A fatal error occured when generating the session ID. {e}");
            }
        }

        private string Url(string request)
        {
            return $"{Config.Instance.Crunchyroll_BaseUrl}{request}.{Config.Instance.Crunchyroll_ApiVersion}.json?&session_id={m_sessionID}";
        }

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Log.Info("Attempting Crunchyroll login..");

                    //var form = new Dictionary<string, string>
                    //{
                    //    { "formname", "RpcApiUser_Login" },
                    //    { "name", username },
                    //    { "password", password }
                    //};
                    //var values = new Dictionary<string, Dictionary<string, string>>()
                    //{
                    //    { "form", form }
                    //};
                    //var content = new FormUrlEncodedContent(form);

                    //var response = await client.PostAsync(@"https://www.crunchyroll.com/?a=formhandler", content);
                    //var responseString = await response.Content.ReadAsStringAsync();

                    //int debug = 0;



                    var temp = Url("login");
                    var values = new Dictionary<string, string>
                    {
                        { "account", username },
                        { "password", password },
                        { "duration", null }
                    };
                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync(@"https://api.crunchyroll.com/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Info("Successfully logged into Crunchyroll.");
                        var responseString = await response.Content.ReadAsStringAsync();

                        int debug = 0;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Log.Error($"A fatal error occured when attempting to log into Crunchyroll.. {e}");
                return false;
            }
        }
    }
}
