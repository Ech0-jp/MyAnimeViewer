using MyAnimeViewerInterfaces.AnimeDB;
using System.Data;
using System.Windows.Controls;

namespace MyAnimeViewerInterfaces.GUI
{
    public interface IBrowseAnimeUserInterface
    {
        void BindData(IAnimeDBSeriesModel data);
        event EditAnime OnEditAnime;

        UserControl View { get; }
    }
}
