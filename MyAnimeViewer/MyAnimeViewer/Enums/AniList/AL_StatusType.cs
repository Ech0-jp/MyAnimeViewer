using DocumentFormat.OpenXml;

namespace MyAnimeViewer.Enums.AniList
{
    public enum AL_AnimeStatus
    {
        [EnumString("Finished Airing")]
        FinishedAiring  = 0,
        [EnumString("Currently Airing")]
        CurrentlyAiring = 1,
        [EnumString("Not Yet Aired")]
        NotYetAired     = 2,
        [EnumString("Cancelled")]
        Cancelled       = 3,
    }

    public enum AL_MangaStatus
    {
        [EnumString("Finished Publishing")]
        FinishedPublishing = 0,
        [EnumString("Publishing")]
        Publishing = 1,
        [EnumString("Not Yet Published")]
        NotYetPublished = 2,
        [EnumString("Cancelled")]
        Cancelled = 3
    }
}
