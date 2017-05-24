using DocumentFormat.OpenXml;

namespace MyAnimeViewer.Enums.MyAnimeList
{
    /// <summary>
    /// The user's anime status.
    /// </summary>
    public enum MAL_MyAnimeStatus
    { 
        [EnumString("Watching")]
        Watching = 1,
        [EnumString("Completed")]
        Completed = 2,
        [EnumString("On-Hold")]
        OnHold = 3,
        [EnumString("Dropped")]
        Dropped = 4,
        [EnumString("Plan to Watch")]
        PlanToWatch = 6
    }
}
