using MyAnimeViewer.Enums.AniList;
using MyAnimeViewer.Errors;
using MyAnimeViewer.Utility;
using MyAnimeViewer.Utility.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeViewer.AniList.API
{
    /// <summary>
    /// AL_AnimeModel is a data storage class. It's primary use is to store the data about the series from AniList's database (NOTE: this is NOT related to the user's
    /// data for the series.
    /// 
    /// This class also comes with a static method allowing for searching of AniList's database.
    /// 
    /// REFERENCE: http://anilist-api.readthedocs.io/en/latest/series.html
    /// 
    /// @author Robert Andrew Gray
    /// @date 1/26/2017
    /// </summary>
    public class AL_AnimeModel
    {
#region PrivateVars
        private int m_id;                       // The series ID.
        private string m_seriesType;            // Anime or Manga.
        private string m_titleEnglish;          // The English title for the series.
        private string m_titleRomaji;           // The Romaji title for the series.
        private string m_titleJapanese;         // The Japanese title for the series.
        private string m_hashtag;               // Official series twitter hashtag.
        private AL_MediaType m_type;            // The media type of the series.
        private DateTime m_startDate;           //
        private DateTime m_endDate;             //
        private int m_season;                   // First 2 numbers are the year (16 is 2016). Last number is the season starting 1 (3 is summer).
        private string m_description;           // Description of the series.
        private string[] m_synonyms;            // Alternative titles.
        private string[] m_genres;              // The series' genres.
        private bool m_adult;                   // True for adult series (Hentai). This does not include ecchi.
        private double m_averageScore;          // The average score given to the series.
        private int m_popularity;               // Number of users with series in their list.
        private string m_smallImageURL;         // Image url. 24x39* (Not available for manga)
        private string m_mediumImageURL;        // Image URL. 93x133*
        private string m_largeImageURL;         // Image URL. 255x323*
        private string m_bannerImageURL;        // Image URL. 1720x390*
        private AL_AnimeStatus m_airingStatus;  // The Airing Status of the series.
        private int m_totalEpisodes;            // The total episodes for the series.
        private int m_duration;                 // Minutes in the average anime episode.
        private List<Relation> m_relations;     // Related anime to the series.
        private List<Character> m_characters;   // The list of characters in the series.
        private List<Staff> m_staffList;        // The list of staff in the series.
        private string m_youtube;               // The youtube link to the preview video of the series.
#endregion

#region Getters
        public int ID { get { return m_id; } }
        public string SeriesType { get { return m_seriesType; } }
        public string TitleEnglish { get { return m_titleEnglish; } }
        public string TitleRomaji { get { return m_titleRomaji; } }
        public string TitleJapanese { get { return m_titleJapanese; } }
        public string Hashtag { get { return m_hashtag; } }
        public AL_MediaType Type { get { return m_type; } }
        public DateTime StartDate { get { return m_startDate; } }
        public DateTime EndDate { get { return m_endDate; } }
        public int Season { get { return m_season; } }
        public string SeasonString
        {
            get
            {
                string result = "";
                char season = m_season.ToString()[2];
                switch (season)
                { 
                    case '1':
                        result = "Winter";
                        break;
                    case '2':
                        result = "Spring";
                        break;
                    case '3':
                        result = "Summer";
                        break;
                    case '4':
                        result = "Fall";
                        break;
                }
                result += " 20" + m_season.ToString()[0] + m_season.ToString()[1];
                return result;
            }
        }
        public string Description { get { return m_description.Replace("\n", "").Replace("<br>", "\n"); } }
        public string[] Synonyms { get { return m_synonyms; } }
        public string SynonymsString 
        {
            get 
            {
                string result = "";
                foreach (string item in m_synonyms)
                {
                    result += item + "\n";
                }
                return result;
            }
        }
        public string[] Genres { get { return m_genres; } }
        public string GenresString
        {
            get
            {
                string result = "";
                for (int i = 0; i < m_genres.Length; i++)
                {
                    result += m_genres[i];
                    if (i != m_genres.Length - 1)
                        result += "\n";
                }
                return result;
            }
        }
        public bool Adult { get { return m_adult; } }
        public double AverageScore { get { return m_averageScore; } }
        public int Popularity { get { return m_popularity; } }
        public string SmallImageURL { get { return m_smallImageURL; } }
        public string MediumImageURL { get { return m_mediumImageURL; } }
        public string LargeImageURL { get { return m_largeImageURL; } }
        public string BannerImageURL { get { return m_bannerImageURL; } }
        public AL_AnimeStatus AiringStatus { get { return m_airingStatus; } }
        public int TotalEpisodes { get { return m_totalEpisodes; } }
        public int Duration { get { return m_duration; } }
        public string DurationString { get { return m_duration.ToString() + " mins"; } }
        public List<Relation> Relations { get { return m_relations; } }
        public List<Character> Characters { get { return m_characters; } }
        public List<Staff> StaffList { get { return m_staffList; } }
        public string YoutubeURL { get { return Config.Instance.Youtube_BaseURL + m_youtube + "?feature=player_embedded"; } }
        #endregion

        /// <summary>
        /// Initialize the AnimeModel.
        /// </summary>
        /// <param name="animeModel">A JObject required initialize the data in AnimeModel.</param>
        public AL_AnimeModel(JObject animeModel, bool smallModel = false)
        {
            m_id = (int)animeModel.Property("id").Value;
            m_seriesType = (string)animeModel.Property("series_type").Value;
            m_titleEnglish = (string)animeModel.Property("title_english").Value;
            m_titleRomaji = (string)animeModel.Property("title_romaji").Value;
            m_titleJapanese = (string)animeModel.Property("title_japanese").Value;
            string type = (string)animeModel.Property("type").Value;
            m_type = (AL_MediaType)Enum.Parse(typeof(AL_MediaType), type.Replace(" ", ""), true);
            m_synonyms = animeModel.Property("synonyms").Value.ToObject<string[]>();
            m_genres = animeModel.Property("genres").Value.ToObject<string[]>();
            m_adult = (bool)animeModel.Property("adult").Value;
            m_averageScore = (double)animeModel.Property("average_score").Value;
            m_popularity = (int)animeModel.Property("popularity").Value;
            m_smallImageURL = (string)animeModel.Property("image_url_sml").Value;
            m_mediumImageURL = (string)animeModel.Property("image_url_med").Value;
            m_largeImageURL = (string)animeModel.Property("image_url_lge").Value;
            
            if (smallModel) return;

            string startDate = (string)animeModel.Property("start_date").Value;
            m_startDate = Convert.ToDateTime(startDate);
            string endDate = (string)animeModel.Property("end_date").Value;
            m_endDate = Convert.ToDateTime(endDate);
            string seasonValue = (string)animeModel.Property("season").Value;
            m_season = string.IsNullOrEmpty(seasonValue) ? 0 : Convert.ToInt32(seasonValue);
            m_description = (string)animeModel.Property("description").Value;
            m_bannerImageURL = (string)animeModel.Property("image_url_banner").Value;
            m_hashtag = (string)animeModel.Property("hashtag").Value;
            string airingStatus = (string)animeModel.Property("airing_status").Value;
            m_airingStatus = (AL_AnimeStatus)Enum.Parse(typeof(AL_AnimeStatus), airingStatus.Replace(" ", ""), true);
            m_totalEpisodes = (int)animeModel.Property("total_episodes").Value;
            string duration = (string)animeModel.Property("duration").Value;
            m_duration = Convert.ToInt32(duration);
            m_youtube = (string)animeModel.Property("youtube_id");
            
            m_relations = new List<Relation>();
            foreach (var item in animeModel.Property("relations").Value.Children())
            {
                var itemProperties = item.Children<JProperty>();

                var idValue = itemProperties.FirstOrDefault(x => x.Name == "id");
                int id = (int)idValue.Value;

                var titleEnglishValue = itemProperties.FirstOrDefault(x => x.Name == "title_english");
                string titleEnglish = (string)titleEnglishValue.Value;

                var titleJapaneseValue = itemProperties.FirstOrDefault(x => x.Name == "title_japanese");
                string titleJapanese = (string)titleJapaneseValue.Value;

                var titleRomajiValue = itemProperties.FirstOrDefault(x => x.Name == "title_romaji");
                string titleRomaji = (string)titleRomajiValue.Value;

                var imgUrlLgeValue = itemProperties.FirstOrDefault(x => x.Name == "image_url_lge");
                string imgUrlLge = (string)imgUrlLgeValue.Value;

                var imgUrlMedValue = itemProperties.FirstOrDefault(x => x.Name == "image_url_med");
                string imgUrlMed = (string)imgUrlMedValue.Value;

                var imgUrlSmlValue = itemProperties.FirstOrDefault(x => x.Name == "image_url_sml");
                string imgUrlSml = (string)imgUrlSmlValue.Value;

                var relationTypeValue = itemProperties.FirstOrDefault(x => x.Name == "relation_type");
                string relationType = (string)relationTypeValue.Value;

                Relation relation = new Relation(id, titleEnglish, titleJapanese, titleRomaji, imgUrlLge, imgUrlMed, imgUrlSml, relationType);
                m_relations.Add(relation);
            }
            
            m_characters = new List<Character>();
            foreach (var item in animeModel.Property("characters").Value.Children())
            {
                var itemProperties = item.Children<JProperty>();

                var idValue = itemProperties.FirstOrDefault(x => x.Name == "id");
                int id = (int)idValue.Value;
                
                var firstNameValue = itemProperties.FirstOrDefault(x => x.Name == "name_first");
                string firstName = (string)firstNameValue.Value;

                var lastNameValue = itemProperties.FirstOrDefault(x => x.Name == "name_last");
                string lastName = (string)lastNameValue.Value;

                var imgUrlLgeValue = itemProperties.FirstOrDefault(x => x.Name == "image_url_lge");
                string imgUrlLge = (string)imgUrlLgeValue.Value;

                var imgUrlMedValue = itemProperties.FirstOrDefault(x => x.Name == "image_url_med");
                string imgUrlMed = (string)imgUrlMedValue.Value;

                var roleValue = itemProperties.FirstOrDefault(x => x.Name == "role");
                string role = (string)roleValue.Value;

                var actorValue = itemProperties.FirstOrDefault(x => x.Name == "actor");
                Staff actor = new Staff();
                foreach (var temp in actorValue.Value.Children())
                {
                    var property = temp.Children<JProperty>();

                    var actorIdValue = property.FirstOrDefault(x => x.Name == "id");
                    int actorId = (int)actorIdValue.Value;

                    var actorFirstNameValue = property.FirstOrDefault(x => x.Name == "name_first");
                    string actorFirstName = (string)actorFirstNameValue.Value;

                    var actorLastNameValue = property.FirstOrDefault(x => x.Name == "name_last");
                    string actorLastName = (string)actorLastNameValue.Value;

                    var actorImgUrlLgeValue = property.FirstOrDefault(x => x.Name == "image_url_lge");
                    string actorImgUrlLge = (string)actorImgUrlLgeValue.Value;

                    var actorImgUrlMedValue = property.FirstOrDefault(x => x.Name == "image_url_med");
                    string actorImgUrlMed = (string)actorImgUrlMedValue.Value;

                    var actorLinkIdValue = property.FirstOrDefault(x => x.Name == "link_id");
                    string actorLinkId = (string)actorLinkIdValue.Value;

                    var actorRoleValue = property.FirstOrDefault(x => x.Name == "role");
                    string actorRole = (string)actorRoleValue.Value;

                    actor = new Staff(actorId, actorFirstName, actorLastName, actorImgUrlLge, actorImgUrlMed, actorLinkId, actorRole);
                }
                
                Character character = new Character(id, firstName, lastName, imgUrlLge, imgUrlMed, role, actor);
                m_characters.Add(character);
            }

            m_staffList = new List<Staff>();
            foreach (var item in animeModel.Property("staff").Value.Children())
            {
                var itemProperties = item.Children<JProperty>();

                var idValue = itemProperties.FirstOrDefault(x => x.Name == "id");
                int id = (int)idValue.Value;

                var firstNameValue = itemProperties.FirstOrDefault(x => x.Name == "name_first");
                string firstName = (string)firstNameValue.Value;

                var lastNameValue = itemProperties.FirstOrDefault(x => x.Name == "name_last");
                string lastName = (string)lastNameValue.Value;

                var imgUrlLgeValue = itemProperties.FirstOrDefault(x => x.Name == "image_url_lge");
                string imgUrlLge = (string)imgUrlLgeValue.Value;

                var imgUrlMedValue = itemProperties.FirstOrDefault(x => x.Name == "image_url_med");
                string imgUrlMed = (string)imgUrlMedValue.Value;

                var linkIdValue = itemProperties.FirstOrDefault(x => x.Name == "link_id");
                string linkId = (string)linkIdValue.Value;

                var roleValue = itemProperties.FirstOrDefault(x => x.Name == "role");
                string role = (string)roleValue.Value;

                Staff staff = new Staff(id, firstName, lastName, imgUrlLge, imgUrlMed, linkId, role);
                m_staffList.Add(staff);
            }
        }

        /// <summary>
        /// Query AniList's database and retrieve an Anime Model based on the query string.
        /// </summary>
        /// <param name="query">The string used to query AniList's database.</param>
        /// <returns>AL_AnimeModel</returns>
        public static async Task<List<AL_AnimeModel>> Search(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                    throw new ArgumentNullException("query", "This argument cannot be null or empty!");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);
                    var response = await client.GetAsync(Config.Instance.AniList_BaseUrl + "anime/search/" + query);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        dynamic json = JsonConvert.DeserializeObject(responseString);
                        List<AL_AnimeModel> result = new List<AL_AnimeModel>();
                        foreach (var item in json)
                        {
                            AL_AnimeModel temp = new AL_AnimeModel(item, true);
                            result.Add(temp);
                        }
                        return result;
                    }
                    else
                    {
                    ErrorManager.AddError("Error searching AniList.co.", $"A problem occured when attempting to browse AniList.co. {response.StatusCode}: {response.ReasonPhrase}", true);
                    return null;
                }
            }
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Error searching AniList.co.", $"A fatal error occured when attemting to search AniList.co. {e}");
                return null;
            }
        }

        /// <summary>
        /// Retrieve the "Series Model" from AniLists database. (NOTE: the series data, not the users)
        /// </summary>
        /// <returns>AL_AnimeModel</returns>
        public static async Task<AL_AnimeModel> GetAnimeInfo(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);
                    var response = await client.GetAsync(Config.Instance.AniList_BaseUrl + string.Format("anime/{0}", id));
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        dynamic json = JsonConvert.DeserializeObject(responseString);

                        return new AL_AnimeModel(json);
                    }
                    else
                    {
                        ErrorManager.AddError("Error retrieving information.", $"A problem occured when attempting to retrieve this anime's information. {response.StatusCode}: {response.ReasonPhrase}", true);
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Error retrieving information", $"There was a fatal error when attempting to retrieve this anime's information. {e}", true);
                return null;
            }
        }

        /// <summary>
        /// Retrieves the "Series Model" from AniLists database including the following:
        /// Up to 9 small model characters (ordered by main role) with Japanese small model actors for anime.
        /// Up to 9 small model staff
        /// Up to 2 small model reviews with their users.
        /// Relations (small model)
        /// Anime/Manga relations (small model)
        /// Studios (anime)
        /// External links (anime)
        /// </summary>
        /// <returns>AL_AnimeModel</returns>
        public static async Task<AL_AnimeModel> GetAnimePage(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);
                    var response = await client.GetAsync(Config.Instance.AniList_BaseUrl + string.Format("anime/{0}/page", id));
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        dynamic json = JsonConvert.DeserializeObject(responseString);

                        return new AL_AnimeModel(json);
                    }
                    else
                    {
                        ErrorManager.AddError("Error retrieving anime page.", $"A problem occured when attempting to retrieve this anime's page. {response.StatusCode}: {response.ReasonPhrase}", true);
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Error retrieving anime page.", $"There was a fatal error when attempting to retrieve this anime's page. {e}", true);
                return null;
            }
        }

        public static async Task<List<AL_AnimeModel>> Browse(AL_BrowseParams data, bool sortdirDesc = true)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AL_Authentication.TokenType, AL_Authentication.AccessToken);
                    var url = string.Format("browse/anime?{0}", string.Join("&", data.ToDictionary().Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)))) + (sortdirDesc ? "-desc" : "");
                    var response = await client.GetAsync(Config.Instance.AniList_BaseUrl + url);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        dynamic json = JsonConvert.DeserializeObject(responseString);

                        List<AL_AnimeModel> result = new List<AL_AnimeModel>();
                        foreach (var item in json)
                        {
                            AL_AnimeModel temp = new AL_AnimeModel(item, true);
                            result.Add(temp);
                        }
                        return result;
                    }
                    else
                    {
                        ErrorManager.AddError("Error browsing AniList.co.", $"A problem occured when attempting to browse AniList.co. {response.StatusCode}: {response.ReasonPhrase}", true);
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Error browsing page.", $"A fatal error occured while trying to browse AniList.co. {e}", true);
                return null;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            AL_AnimeModel model = obj as AL_AnimeModel;
            if (model == null) return false;
            else return Equals(model);
        }

        public bool Equals(AL_AnimeModel other)
        {
            if (other == null) return false;
            return (ID.Equals(other.ID));
        }

        public class Relation
        {
            private int m_id;
            private string m_titleEnglish;
            private string m_titleJapanese;
            private string m_titleRomaji;
            private string m_imageUrlLarge;
            private string m_imageUrlMedium;
            private string m_imageUrlSmall;
            private string m_relationType;

            public int ID { get { return m_id; } }
            public string TitleEnglish { get { return m_titleEnglish; } }
            public string TitleJapanese { get { return m_titleJapanese; } }
            public string TitleRomaji { get { return m_titleRomaji; } }
            public string ImageUrlLarge { get { return m_imageUrlLarge; } }
            public string ImageUrlMedium { get { return m_imageUrlMedium; } }
            public string ImageUrlSmall { get { return m_imageUrlSmall; } }
            public string RelationType { get { return m_relationType; } }

            public Relation(int id, string titleEnglish, string titleJapanese, string titleRomaji, string imgUrlLge, string imgUrlMed, string imgUrlSml, string relationType)
            {
                m_id = id;
                m_titleEnglish = titleEnglish;
                m_titleJapanese = titleJapanese;
                m_titleRomaji = titleRomaji;
                m_imageUrlLarge = imgUrlLge;
                m_imageUrlMedium = imgUrlMed;
                m_imageUrlSmall = imgUrlSml;
                m_relationType = relationType;
            }
        }

        public class Character
        {
            private int m_id;
            private string m_firstName;
            private string m_lastName;
            private string m_imageUrlLarge;
            private string m_imageUrlMedium;
            private string m_role;
            private Staff m_actor;

            public int ID { get { return m_id; } }
            public string FirstName { get { return m_firstName; } }
            public string LastName { get { return m_lastName; } }
            public string FullName { get { return m_lastName + " " + m_firstName; } }
            public string ImageUrlLarge { get { return m_imageUrlLarge; } }
            public string ImageUrlMedium { get { return m_imageUrlMedium; } }
            public string Role { get { return m_role; } }
            public Staff Actor { get { return m_actor; } }

            public Character(int id, string firstName, string lastName, string imgUrlLge, string imgUrlMed, string role, Staff actor)
            {
                m_id = id;
                m_firstName = firstName;
                m_lastName = lastName;
                m_imageUrlLarge = imgUrlLge;
                m_imageUrlMedium = imgUrlMed;
                m_role = role;
                m_actor = actor;
            }
        }

        public class Staff
        {
            private int m_id;
            private string m_firstName;
            private string m_lastName;
            private string m_imageUrlLarge;
            private string m_imageUrlMedium;
            private string m_linkID;
            private string m_role;

            public int ID { get { return m_id; } }
            public string FirstName { get { return m_firstName; } }
            public string LastName { get { return m_lastName; } }
            public string FullName { get { return m_lastName + " " + m_firstName; } }
            public string ImageUrlLarge { get { return m_imageUrlLarge; } }
            public string ImageUrlMedium { get { return m_imageUrlMedium; } }
            public string LinkID { get { return m_linkID; } }
            public string Role { get { return m_role; } }

            public Staff()
            { 
            }

            public Staff(int id, string firstName, string lastName, string imgUrlLge, string imgUrlMed, string linkID, string role)
            {
                m_id = id;
                m_firstName = firstName;
                m_lastName = lastName;
                m_imageUrlLarge = imgUrlLge;
                m_imageUrlMedium = imgUrlMed;
                m_linkID = linkID;
                m_role = role;
            }
        }
    }
}
