using MyAnimeViewer.Enums.MyAnimeList;
using System;

namespace MyAnimeViewer.MyAnimeList.API
{
    /// <summary>
    /// A storage class used to store the 
    /// </summary>
    public class AnimeEntry
    {
        public int Series_ID { get; set; }                  // The series ID.
        public string Series_Title { get; set; }            // The series title.
        public string Series_Synonyms { get; set; }         // The series synonyms.
        public MAL_AnimeType Series_Type { get; set; }      // The type of series (TV, OVA, Movie, etc.).
        public int Series_Episodes { get; set; }            // How many episodes the series contains
        public MAL_AnimeStatus Series_Status { get; set; }      // What status the series is in (Airing, finished airing, etc.). ***THIS DATA TYPE IS INCORRECT. FIX***
        public DateTime Series_Start { get; set; }          // The date the the series first aired.
        public DateTime Series_End { get; set; }            // The date the series finished airing.
        public string Series_Image { get; set; }            // The display image for the series.
        public int My_Watched_Episodes { get; set; }        // The amount of episodes the user has watched.
        public DateTime My_Start_Date { get; set; }         // The date the user started watching the series.
        public DateTime My_Finish_Date { get; set; }        // The date the user finished watching the series.
        public int My_Score { get; set; }                   // The score the user gave the series (1-10).
        public MAL_MyAnimeStatus My_Status { get; set; }    // The users status on the anime (Watching, completed, etc.)
        public bool My_Rewatching { get; set; }             // Is the user rewatching the series?
        public int My_Rewatching_Ep { get; set; }           // What episode the user is on. (if rewatching)
        public int My_Last_Updated { get; set; }            // The last time the user updated the entry.
        public string My_Tags { get; set; }                 // The tags the user gave the series.
    }
}
