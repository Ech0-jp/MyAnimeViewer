using DocumentFormat.OpenXml;
using MyAnimeViewer.Utility;
using System.ComponentModel;

namespace MyAnimeViewer.Enums.AniList
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum AL_Genres
    {
        [Description("Action")]
        Action,
        [Description("Adventure")]
        Adventure,
        [Description("Comedy")]
        Comedy,
        [Description("Drama")]
        Drama,
        [Description("Ecchi")]
        Ecchi,
        [Description("Fantasy")]
        Fantasy,
        [Description("Hentai")]
        Hentai,
        [Description("Horror")]
        Horror,
        [Description("Mahou Shoujo")]
        MahouShoujo,
        [Description("Mecha")]
        Mecha,
        [Description("Music")]
        Music,
        [Description("Mystery")]
        Mystery,
        [Description("Psychological")]
        Psychological,
        [Description("Romance")]
        Romance,
        [Description("Sci-Fi")]
        SciFi,
        [Description("Slice of Life")]
        SliceOfLife,
        [Description("Sports")]
        Sports,
        [Description("Supernatural")]
        Supernatural,
        [Description("Thriller")]
        Thriller,
    }
}
