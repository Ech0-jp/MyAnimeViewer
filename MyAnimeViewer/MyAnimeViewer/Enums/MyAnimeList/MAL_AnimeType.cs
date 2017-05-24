using DocumentFormat.OpenXml;

namespace MyAnimeViewer.Enums.MyAnimeList
{
    /// <summary>
    /// The anime series type.
    /// </summary>
    public enum MAL_AnimeType
    { 
        [EnumString("Unknown")]
        Unknown = 0,
        [EnumString("TV")]
        TV = 1,
        [EnumString("OVA")]
        OVA = 2,
        [EnumString("Movie")]
        Movie = 3,
        [EnumString("Special")]
        Special = 4,
        [EnumString("ONA")]
        ONA = 5,
        [EnumString("Music")]
        Music = 6
    }
}
