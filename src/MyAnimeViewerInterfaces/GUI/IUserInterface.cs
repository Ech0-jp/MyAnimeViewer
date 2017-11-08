using System.Windows.Controls;

namespace MyAnimeViewerInterfaces.GUI
{
    public enum InterfaceType
    {
        AnimeList,
        AnimeInformation,
        BrowseAnime,
        Simulcast
    }

    public interface IUserInterface
    {
        IAnimeListUserInterface AnimeList { get; }
        IAnimeInformationUserInterface AnimeInformation { get; }
        IBrowseAnimeUserInterface BrowseAnime { get; }
        ISimulcastUserInterface Simulcast { get; }

        UserControl GetView(InterfaceType type);
    }
}
