using System.ComponentModel;

namespace MyAnimeViewerInterfaces.AnimeDB.DataTypes
{
    public enum SeriesStatus
    {
        [Description("Airing")]
        Airing = 0,
        [Description("Finished")]
        Finished = 1,
        [Description("Not Yet Aired")]
        NotYetAired = 2,
        [Description("Cancelled")]
        Cancelled = 3
    }
}
