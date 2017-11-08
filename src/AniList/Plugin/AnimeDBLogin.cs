using MyAnimeViewerInterfaces.AnimeDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AniList.Plugin
{
    public class AnimeDBLogin : IAnimeDBLogin
    {
        public LoginType loginType => LoginType.OAuth;

        public async Task<bool> Login(LoginInformation loginInfo)
        {
            if (string.IsNullOrEmpty(loginInfo.OAuth.authorization_pin) && string.IsNullOrEmpty(loginInfo.OAuth.refresh_token))
            {
                RequestAuthorizationPin();
                return false;
            }
            else if (!string.IsNullOrEmpty(loginInfo.OAuth.refresh_token))
            {
                Settings.Instance.LoginInformation = loginInfo;
                return await RefreshAccessToken(loginInfo);
            }
            return await RequestAccessToken(loginInfo);
        }

        public void Logout()
        {
            _accessToken = null;
            _tokenType = null;
            Settings.Instance.LoginInformation = null;
        }

        private string _accessToken;
        private string _tokenType;
        private DateTime _expiresBy;

        public string ContentTypeHeader { get { return "application/x-www-form-urlencoded"; } }
        public async Task<AuthenticationHeaderValue> AuthenticationHeader()
        {
            if (DateTime.Now > _expiresBy)
                await RefreshAccessToken(Settings.Instance.LoginInformation);
            return new AuthenticationHeaderValue(_tokenType, _accessToken);
        }

        private void RequestAuthorizationPin()
        {
            try
            {
                string header = "auth/authorize";
                string grant_type = "authorization_pin";
                string client_id = Resources.Secrets.ClientID;
                string response_type = "pin";

                string url = Settings.Instance.BaseUrl + string.Format("{0}?grant_type={1}&client_id={2}&response_type={3}", header, grant_type, client_id, response_type);
                Process.Start(url);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<bool> RequestAccessToken(LoginInformation loginInfo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "grant_type", "authorization_pin" },
                        { "client_id", Resources.Secrets.ClientID },
                        { "client_secret", Resources.Secrets.ClientSecret },
                        { "code", loginInfo.OAuth.authorization_pin }
                    };
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(Settings.Instance.BaseUrl + "auth/access_token", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    _accessToken = json.access_token;
                    _tokenType = json.token_type;
                    _expiresBy = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double)json.expires).ToLocalTime();
                    var OAuth = new LoginInformation();
                    OAuth.OAuth = new LoginInformation.OAUTH();
                    OAuth.OAuth.refresh_token = json.refresh_token;
                    Settings.Instance.LoginInformation = OAuth;

                    if (Core.PluginController.AnimeDB.user == null)
                        await (Core.PluginController.AnimeDB as AnimeDB).GetAuthenticatedUser();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<bool> RefreshAccessToken(LoginInformation loginInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(loginInfo.OAuth.refresh_token))
                {
                    throw new Exception("Missing refresh token!");
                }

                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "grant_type", "refresh_token" },
                        { "client_id", Resources.Secrets.ClientID },
                        { "client_secret", Resources.Secrets.ClientSecret },
                        { "refresh_token", loginInfo.OAuth.refresh_token }
                    };
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(Settings.Instance.BaseUrl + "auth/access_token", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    _accessToken = json.access_token;
                    _tokenType = json.token_type;
                    _expiresBy = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double)json.expires).ToLocalTime();

                    if (Core.PluginController.AnimeDB.user == null)
                        await (Core.PluginController.AnimeDB as AnimeDB).GetAuthenticatedUser();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
