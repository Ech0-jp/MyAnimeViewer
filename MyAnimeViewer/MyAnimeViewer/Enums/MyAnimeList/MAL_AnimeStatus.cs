using DocumentFormat.OpenXml;

namespace MyAnimeViewer.Enums.MyAnimeList
{
    /// <summary>
    /// The series' anime status.
    /// </summary>
    public enum MAL_AnimeStatus
    {
        [EnumString("Currently Airing")]
        CurrentlyAiring = 1,
        [EnumString("Finished Airing")]
        FinishedAiring = 2,
        [EnumString("Not Yet Aired")]
        NotYetAired = 3
    }
}