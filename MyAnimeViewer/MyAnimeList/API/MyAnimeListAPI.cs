using MyAnimeViewer.Enums;
using MyAnimeViewer.Enums.MyAnimeList;
using MyAnimeViewer.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace MyAnimeViewer.MyAnimeList.API
{
    public class MyAnimeListAPI
    {
        private static XmlDocument m_animeListXml;

        public static List<Anime> AnimeList_Watching { get; private set; }
        public static List<Anime> AnimeList_Completed { get; private set; }
        public static List<Anime> AnimeList_OnHold { get; private set; }
        public static List<Anime> AnimeList_Dropped { get; private set; }
        public static List<Anime> AnimeList_PlanToWatch { get; private set; }

        private static string m_baseUrl = "http://myanimelist.net/api/";

        private static string BaseUrl
        {
            get { return m_baseUrl; }
            set { m_baseUrl = value; }
        }

#region authentication
        public static string ID { get; private set; }

        public static bool IsLoggedIn
        {
            get { return !string.IsNullOrEmpty(ID); }
        }

        public static string LoggedInAs { get; private set; }

        public static bool Logout()
        {
            //>>TODO
            return true;
        }

        public static bool LoadCredentials()
        {
            if (File.Exists(Config.Instance.MyAnimeList_FilePath))
            {
                try
                {
                    //Logger.WriteLine("Loading stored credentials...", "MyAnimeListAPI");
                    using (var reader = new StreamReader(Config.Instance.MyAnimeList_FilePath))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(reader);
                        ID = doc.SelectSingleNode("user/id").InnerText;
                        LoggedInAs = doc.SelectSingleNode("user/username").InnerText;
                        return true;
                    }
                }
                catch (Exception e)
                {
                    //Logger.WriteLine("Error loading credentials\n" + e, "MyAnimeListAPI");
                    return false;
                }
            }
            return false;
        }

        public static async Task<MAL_LoginResult> LoginAsync(string email, string password)
        {
            try
            {
                //Logger.WriteLine("Logging in...", "MyAnimeListAPI");
                var url = BaseUrl + "account/verify_credentials.xml ";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Credentials = new NetworkCredential(email, password);
                request.UserAgent = "curl/7.37.0";
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Logger.WriteLine("Successfully logged in.", "MyAnimeListAPI");
                    XmlDocument doc = new XmlDocument();
                    doc.Load(response.GetResponseStream());
                    ID = doc.SelectSingleNode("user/id").InnerText;
                    LoggedInAs = doc.SelectSingleNode("user/username").InnerText;
                    if (Config.Instance.MyAnimeListRememberLogin)
                    {
                        using (TextWriter writer = new StreamWriter(Config.Instance.MyAnimeList_FilePath))
                            doc.Save(writer);
                    }
                    return new MAL_LoginResult(true);
                }
                //Logger.WriteLine("Error logging in...", "MyAnimeListAPI");
                return new MAL_LoginResult(false, response.ToString());
            }
            catch (Exception e)
            {
                //Logger.WriteLine("Error logging in...", "MyAnimeListAPI");
                return new MAL_LoginResult(false, e.Message);
            }
        }
#endregion authentication

#region Utilities
        /// <summary>
        /// Generates a list of the users anime for MAV to display.
        /// </summary>
        /// <returns>True if successful.</returns>
        public static async Task<bool> GenerateAnimeList()
        {
            try
            {
                if (await InitializeAnimeListAsync())
                {
                    foreach (XmlNode node in m_animeListXml.SelectNodes("myanimelist/anime"))
                    {
                        AddToAnimeList(node);
                    }
                    
                    AnimeList_Watching = AnimeList_Watching.OrderBy(o => o.Series_Title).ToList();
                    AnimeList_Completed = AnimeList_Completed.OrderBy(o => o.Series_Title).ToList();
                    AnimeList_OnHold = AnimeList_OnHold.OrderBy(o => o.Series_Title).ToList();
                    AnimeList_Dropped = AnimeList_Dropped.OrderBy(o => o.Series_Title).ToList();
                    AnimeList_PlanToWatch = AnimeList_PlanToWatch.OrderBy(o => o.Series_Title).ToList();

                    m_animeListXml.RemoveAll();
                    m_animeListXml = null;
                    //Logger.WriteLine("Generated user's anime list.", "MyAnimeListAPI");
                    return true;
                }
                //Logger.WriteLine("Error generating user's anime list.", "MyAnimeListAPI");
                return false;
            }
            catch (Exception e)
            {
                //Logger.WriteLine("Error generating user's anime list.\n" + e, "MyAnimeListAPI");
                return false;
            }
        }

        /// <summary>
        /// Retrieves the user's Anime List from MyAnimeList.net
        /// </summary>
        /// <returns>True if task successful.</returns>
        private static async Task<bool> InitializeAnimeListAsync()
        {
            if (IsLoggedIn)
            {
                try
                {
                    //Logger.WriteLine("Loading user's anime list...", "MyAnimeListAPI");
                    var url = "http://myanimelist.net/malappinfo.php?u=" + LoggedInAs + "&status=all&type=anime";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //Logger.WriteLine("Retrieved user's anime list.", "MyAnimeListAPI");
                        m_animeListXml = new XmlDocument();
                        m_animeListXml.Load(response.GetResponseStream());
                        return true;
                    }
                    //Logger.WriteLine("Error retrieving user's anime list.", "MyAnimeListAPI");
                    return false;
                }
                catch (Exception e)
                {
                    //Logger.WriteLine("Error retrieving user's anime list.\n" + e, "MyAnimeListAPI");
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Add to List<Anime> AnimeList from XmlNode
        /// </summary>
        /// <param name="node">The node to be added to AnimeList</param>
        private static void AddToAnimeList(XmlNode node)
        {
            if (AnimeList_Watching == null)
                AnimeList_Watching = new List<Anime>();
            if (AnimeList_Completed == null)
                AnimeList_Completed = new List<Anime>();
            if (AnimeList_OnHold == null)
                AnimeList_OnHold = new List<Anime>();
            if (AnimeList_Dropped == null)
                AnimeList_Dropped = new List<Anime>();
            if (AnimeList_PlanToWatch == null)
                AnimeList_PlanToWatch = new List<Anime>();

            Anime newAnime = new Anime();
            newAnime.Series_ID = Convert.ToInt32(node.SelectSingleNode("series_animedb_id").InnerText);
            newAnime.Series_Title = node.SelectSingleNode("series_title").InnerText;
            newAnime.Series_Synonyms = node.SelectSingleNode("series_synonyms").InnerText;
            newAnime.Series_Type = (MAL_AnimeType)Convert.ToInt32(node.SelectSingleNode("series_type").InnerText);
            newAnime.Series_Episodes = Convert.ToInt32(node.SelectSingleNode("series_episodes").InnerText);
            newAnime.Series_Status = (MAL_AnimeStatus)Convert.ToInt32(node.SelectSingleNode("series_status").InnerText);
           // DateTime.ParseExact(node.SelectSingleNode("series_start").InnerText, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
            string series_start = node.SelectSingleNode("series_start").InnerText;
            newAnime.Series_Start = node.SelectSingleNode("series_start").InnerText == "0000-00-00" ?
                                    new DateTime() : Convert.ToDateTime(node.SelectSingleNode("series_start").InnerText);
            newAnime.Series_End = node.SelectSingleNode("series_end").InnerText == "0000-00-00" ?
                                  new DateTime() : Convert.ToDateTime(node.SelectSingleNode("series_end").InnerText);
            newAnime.Series_Image = node.SelectSingleNode("series_image").InnerText;
            newAnime.My_Watched_Episodes = Convert.ToInt32(node.SelectSingleNode("my_watched_episodes").InnerText);
            newAnime.My_Start_Date = node.SelectSingleNode("my_start_date").InnerText == "0000-00-00" ?
                                     new DateTime() : Convert.ToDateTime(node.SelectSingleNode("my_start_date").InnerText);
            newAnime.My_Finish_Date = node.SelectSingleNode("my_finish_date").InnerText == "0000-00-00" ?
                                      new DateTime() : Convert.ToDateTime(node.SelectSingleNode("my_finish_date").InnerText);
            newAnime.My_Score = Convert.ToInt32(node.SelectSingleNode("my_score").InnerText);
            newAnime.My_Status = (MAL_MyAnimeStatus)Convert.ToInt32(node.SelectSingleNode("my_status").InnerText);
            //newAnime.My_Rewatching = node.SelectSingleNode("my_rewatching").InnerText == null ?
            //                         false : Convert.ToBoolean(node.SelectSingleNode("my_rewatching").InnerText);
            newAnime.My_Last_Updated = Convert.ToInt32(node.SelectSingleNode("my_last_updated").InnerText);
            newAnime.My_Tags = node.SelectSingleNode("my_tags").InnerText;

            switch(newAnime.My_Status)
            {
                case MAL_MyAnimeStatus.Watching:
                AnimeList_Watching.Add(newAnime);
                break;
                case MAL_MyAnimeStatus.Completed:
                AnimeList_Completed.Add(newAnime);
                break;
                case MAL_MyAnimeStatus.OnHold:
                AnimeList_OnHold.Add(newAnime);
                break;
                case MAL_MyAnimeStatus.Dropped:
                AnimeList_Dropped.Add(newAnime);
                break;
                case MAL_MyAnimeStatus.PlanToWatch:
                AnimeList_PlanToWatch.Add(newAnime);
                break;
            }
        }
#endregion Utilities
    }

    public class Anime
    {
        public int Series_ID { get; set; }  // The series ID.
        public string Series_Title { get; set; } // The series title.
        public string Series_Synonyms { get; set; }  // The series synonyms.
        public MAL_AnimeType Series_Type { get; set; }  // The type of series (TV, OVA, Movie, etc.).
        public int Series_Episodes { get; set; }  // How many episodes the series contains
        public MAL_AnimeStatus Series_Status { get; set; }  // What status the series is in (Airing, finished airing, etc.).
        public DateTime Series_Start { get; set; }  // The date the the series first aired.
        public DateTime Series_End { get; set; }  // The date the series finished airing.
        public string Series_Image { get; set; }  // The display image for the series.
        public int My_Watched_Episodes { get; set; }  // The amount of episodes the user has watched.
        public DateTime My_Start_Date { get; set; }  // The date the user started watching the series.
        public DateTime My_Finish_Date { get; set; }  // The date the user finished watching the series.
        public int My_Score { get; set; }  // The score the user gave the series (1-10).
        public MAL_MyAnimeStatus My_Status { get; set; }  // The users status on the anime (Watching, completed, etc.)
        public bool My_Rewatching { get; set; }  // Is the user rewatching the series?
        public int My_Rewatching_Ep { get; set; }  // What episode the user is on. (if rewatching)
        public int My_Last_Updated { get; set; }  // The last time the user updated the entry.
        public string My_Tags { get; set; }  // The tags the user gave the series.
    }
}
