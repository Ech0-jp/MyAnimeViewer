using MyAnimeViewer.Utility;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace MyAnimeViewer.MyAnimeList.API
{
    /// <summary>
    /// Validates user's login information using http://myanimelist.net/api/
    /// </summary>
    class MAL_Authenticator
    {
#region Properties
        protected MAL_LoginResult m_loginResult;        // The result of a login attempt.
        protected string m_ID;                          // The user's MAL ID.
        protected string m_username;                    // The user's MAL username.
#endregion Properties

        /// <summary>
        /// Attempt to load the user's stored credentails.
        /// </summary>
        /// <returns>True if success.</returns>
        protected bool LoadCredentials()
        {
            if (File.Exists(Config.Instance.MyAnimeList_FilePath))
            {
                try
                {
                    Logger.WriteLine("Loading stored credentials...", "MyAnimeListAPI");
                    using (var reader = new StreamReader(Config.Instance.MyAnimeList_FilePath))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);
                        m_ID = doc.SelectSingleNode("user/id").InnerText;
                        m_username = doc.SelectSingleNode("user/username").InnerText;
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error loading credentials\n" + e, "MyAnimeListAPI");
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Verify the user's login credentials with "http://myanimelist.net/api/account/verify_crednetials.xml".
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        protected async void Login(string username, string password)
        {
            m_loginResult = await LoginAsync(username, password);
        }

        /// <summary>
        /// Creates a new Task.Factory for the login request to be processed asynchronously.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <returns></returns>
        private Task<MAL_LoginResult> LoginAsync(string username, string password)
        {
            return Task.Factory.StartNew(() => MAL_RequestLogin(username, password));
        }

        /// <summary>
        /// Establish a connection with "http://myanimelist.net/api/account/verify_crednetials.xml" and verify the user's
        /// login credentials are correct.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <returns></returns>
        private MAL_LoginResult MAL_RequestLogin(string username, string password)
        {
            try
            {
                var url = Config.Instance.MyAnimeList_BaseUrl + "/account/verify_credentials.xml";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Credentials = new NetworkCredential(username, password);
                request.UserAgent = "curl/7.37.0";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(response.GetResponseStream());
                    m_ID = doc.SelectSingleNode("user/id").InnerText;
                    m_username = doc.SelectSingleNode("user/username").InnerText;
                    if (Config.Instance.MyAnimeListRememberLogin)
                    {
                        using (TextWriter writer = new StreamWriter(Config.Instance.MyAnimeList_FilePath))
                        {
                            doc.Save(writer);
                        }
                        return new MAL_LoginResult(true);
                    }
                }
                return new MAL_LoginResult(false, response.ToString());
            }
            catch (Exception e)
            {
                Logger.WriteLine("Error logging in...", "MAL_Authenticator");
                return new MAL_LoginResult(false, e.Message);
            }
        }
    }
}
