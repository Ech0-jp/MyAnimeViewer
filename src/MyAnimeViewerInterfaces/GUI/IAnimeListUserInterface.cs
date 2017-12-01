using System.Data.Common;
using System.Windows.Controls;

namespace MyAnimeViewerInterfaces.GUI
{
    public interface IAnimeListUserInterface
    {
        void BindList(DbDataAdapter dataAdapter);
        event ViewAnimeInformation OnViewAnimeInformation;
        event EditAnime OnEditAnime;
        event WatchAnime OnWatchAnime;

        UserControl View { get; }
    }
}