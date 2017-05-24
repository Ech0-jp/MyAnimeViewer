using DocumentFormat.OpenXml;

namespace MyAnimeViewer.Enums.AniList
{
    public enum AL_MediaType
    {
        [EnumString("TV")]
        TV = 0,
        [EnumString("TV Short")]
        TVShort = 1,
        [EnumString("Movie")]
        Movie = 2,
        [EnumString("Special")]
        Special = 3,
        [EnumString("OVA")]
        OVA = 4,
        [EnumString("ONA")]
        ONA = 5,
        [EnumString("Music")]
        Music = 6,
        [EnumString("Manga")]
        Manga = 7,
        [EnumString("Novel")]
        Novel = 8,
        [EnumString("One Shot")]
        OneShot = 9,
        [EnumString("Doujin")]
        Doujin = 10,
        [EnumString("Manhua")]
        Manhua = 11,
        [EnumString("Manhwa")]
        Manhwa = 12
    }
}
