using MyAnimeViewerInterfaces.GUI;
using System.Windows.Controls;

namespace MyAnimeViewer.Windows.UserControls
{
    class DefaultInterface : IUserInterface
    {
        public IAnimeInformationUserInterface AnimeInformation { get { return null; } }

        public IAnimeListUserInterface AnimeList { get { return new AnimeListUserControl(); } }

        public IBrowseAnimeUserInterface BrowseAnime { get { return null; } }

        public ISimulcastUserInterface Simulcast { get { return null; } }

        public UserControl GetView(InterfaceType type)
        {
            switch (type)
            {
                case InterfaceType.AnimeList:
                    return AnimeList.View;
                default:
                    return null;
            }
        }
    }
}
