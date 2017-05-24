using DocumentFormat.OpenXml;
using System.Collections.Generic;

namespace MyAnimeViewer.Enums.AniList
{
    public struct AL_BrowseParams
    {
        public int? year;               // 4 digit year e.g. "2014"
        public Season? season;          // "winter" || "spring" || "summer" || "fall"
        public AL_MediaType? type;      // Media type
        public AL_AnimeStatus? status;  // Status type
        public string genres;           // Comma separated genre strings. e.g. "Action,Comedy" Returns series that have ALL the genres.
        public string genres_exclude;   // Comma separated genre strings. e.g. "Drama" Excludes series that have ANY of the genres.
        public SortBy? sort;            // "id" || "score" || "popularity" || "start_date" || "end_date" Sorts results, default ascending order. Append "-desc" for descending order e.g. "id-desc"
        public bool airing_data;        // "true" Includes anime airing data in small models
        public bool full_page;          // "true" Returns all available results. Ignores pages. Only available when status="Currently Airing" or season is included
        public int? page;               // used if paginating
        
        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            if (year != null)
                temp.Add("year", year.ToString());
            if (season != null)
                temp.Add("season", season.ToString());
            if (type != null)
                temp.Add("type", type.ToString());
            if (status != null)
                temp.Add("status", status.ToString());
            if (!string.IsNullOrEmpty(genres))
                temp.Add("genres", genres);
            if (!string.IsNullOrEmpty(genres_exclude))
                temp.Add("genres_exclude", genres_exclude);
            if (sort != null)
                temp.Add("sort", sort.ToString());
            if (airing_data)
                temp.Add("airing_data", airing_data.ToString());
            if (full_page)
                temp.Add("full_page", full_page.ToString());
            if (page != null)
                temp.Add("page", page.ToString());
            return temp;
        }
    }

    public enum SortBy
    {
        [EnumString("score")]
        score,
        [EnumString("popularity")]
        popularity,
        [EnumString("start_date")]
        start_date,
        [EnumString("end_date")]
        end_date,
        [EnumString("date_added")]
        date_added,
    }
}
