using MyAnimeViewer.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MyAnimeViewer.Utility.Logging;
using MyAnimeViewer.Errors;

namespace MyAnimeViewer.AniList.API
{
    /// <summary>
    /// The authorization pin grant type allows the client access to view, add, edit and remove a resource 
    /// owner’s data on their behalf. To do this we require the permission from the resource owner 
    /// themselves, this will provide us with an authorization pin, which we can later exchange for the 
    /// required access token.
    /// 
    /// @author Robert Andrew Gray
    /// @date 1/26/2017
    /// </summary>
    public static class AL_Authentication
    {
        private static string m_accessToken;   // A token used to access protected resources.
        private static string m_tokenType;     // The type of token that was returned.
        private static DateTime m_expires;     // When the token expires.
        private static DateTime m_expiresBy;   // not sure?
        private static string m_refreshToken;  // A token used to refresh the access token when it expires.

        public static string AccessToken { get { return m_accessToken; } }
        public static string TokenType { get { return m_tokenType; } }

        /// <summary>
        /// This will direct the resource owner to a web page where they may choose to accept or deny the 
        /// client. If the resource owner is not currently logged in, they will be redirected to the standard AniList 
        /// login page, then redirected back to the client approval page once logged in.
        /// 
        /// If the resource owner accepts the client, the authorization pin will be displayed for the user to copy
        /// and paste into the client. The client can then use this pin to request an access token.
        /// </summary>
        public static void RequestAuthorizationPin()
        {
            try
            {
                Log.Info("Requesting authorization pin..");
                // URL Params.
                string header = "auth/authorize";
                string grant_type = "authorization_pin";
                string client_id = Config.Instance.AniList_ClientID;
                string response_type = "pin";

                string url = Config.Instance.AniList_BaseUrl + string.Format("{0}?grant_type={1}&client_id={2}&response_type={3}", header, grant_type, client_id, response_type);
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception e)
            {
                Log.Error($"Error requestion authorization pin..\n{e}");
                
            }
        }

        /// <summary>
        /// Request an access token from AniList.co so we can freely access the API with GET and POST messages.
        /// </summary>
        /// <param name="AuthorizationPin">The Authorization Pin provided by the user.</param>
        /// <returns>True if success</returns>
        public static async Task<bool> RequestAccessToken(string AuthorizationPin)
        {
            try
            {
                Log.Info("Requesting Access Token..");
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "grant_type", "authorization_pin" },
                        { "client_id", Config.Instance.AniList_ClientID },
                        { "client_secret", Config.Instance.AniList_ClientSecret },
                        { "code", AuthorizationPin }
                    };
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(Config.Instance.AniList_BaseUrl + "auth/access_token", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    m_accessToken = json.access_token;
                    m_tokenType = json.token_type;
                    int expires = json.expires;
                    int expires_in = json.expires_in;
                    m_expires = new DateTime(1970, 1, 1).AddSeconds(expires);
                    m_expiresBy = new DateTime().AddSeconds(expires_in);
                    m_refreshToken = json.refresh_token;
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Error requesting access token..\n{e}");
                return false;
            }
        }

        /// <summary>
        /// Request a new access token using the "refresh token". This function only works if an access token previously existed.
        /// </summary>
        /// <returns>True if success.</returns>
        public static async Task<bool> RefreshAccessToken()
        {
            try
            {
                Log.Info("Refreshing access token..");
                if (string.IsNullOrEmpty(m_refreshToken))
                {
                    Log.Error("Refresh token is null or empty.. Cannot refresh access token.");
                    ErrorManager.AddError("Error refreshing access token.", "Refresh token does not exist. Cannot refresh the access token.");
                    return false;
                }

                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "grant_type", "refresh_token" },
                        { "client_id", Config.Instance.AniList_ClientID },
                        { "client_secret", Config.Instance.AniList_ClientSecret },
                        { "refresh_token", m_refreshToken }
                    };
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(Config.Instance.AniList_BaseUrl + "auth/access_token", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    m_accessToken = json.access_token;
                    m_tokenType = json.token_type;
                    int expires = json.expires;
                    int expires_in = json.expires_in;
                    m_expires = new DateTime(1970, 1, 1).AddSeconds(expires);
                    m_expiresBy = new DateTime().AddSeconds(expires_in);
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"Error refreshing access token.\n{e}");
                return false;
            }
        }

        /// <summary>
        /// Request a new access token using the "refresh token". This function only works if an access token previously existed.
        /// </summary>
        /// <param name="refreshToken">An existing refresh token.</param>
        /// <returns>True if success.</returns>
        public static async Task<bool> RefreshAccessToken(string refreshToken)
        {
            m_refreshToken = refreshToken;
            bool result = await RefreshAccessToken();
            return result;
        }

        /// <summary>
        /// Saves the "refresh token" for future use.
        /// Used so the application can refresh the access token without the user needing to sign back into the app.
        /// </summary>
        public static void SaveRefreshToken()
        {
            Properties.Settings.Default.AL_RefreshToken = m_refreshToken;
            Properties.Settings.Default.Save();
        }

        public static void RemoveRefreshToken()
        {
            Properties.Settings.Default.AL_RefreshToken = null;
            Properties.Settings.Default.Save();
        }
    }
}
