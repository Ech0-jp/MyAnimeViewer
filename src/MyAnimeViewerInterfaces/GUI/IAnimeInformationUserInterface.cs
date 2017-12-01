using System.Data;
using System.Windows.Controls;

namespace MyAnimeViewerInterfaces.GUI
{
    public interface IAnimeInformationUserInterface
    {
        void Bind(DataRow data);
        event EditAnime OnEditAnime;
        event WatchAnime OnWatchAnime;

        UserControl view;
    }
}
