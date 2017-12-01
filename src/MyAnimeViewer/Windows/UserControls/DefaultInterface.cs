using MyAnimeViewerInterfaces.GUI;
using System.Windows.Controls;

namespace MyAnimeViewer.Windows.UserControls
{
    public class DefaultInterface : IUserInterface
    {
        public IAnimeInformationUserInterface AnimeInformation { get { return null; } }

        private AnimeListUserControl _animeList;
        public IAnimeListUserInterface AnimeList
        {
            get
            {
                if (_animeList == null)
                    _animeList = new AnimeListUserControl();
                return _animeList;
            }
        }

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
