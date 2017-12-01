using MyAnimeViewerInterfaces.GUI;
using System.Windows.Controls;
using System;
using MyAnimeViewerInterfaces.AnimeDB;
using System.Data;

namespace MyAnimeViewer.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for AnimeInformationUserControl.xaml
    /// </summary>
    public partial class AnimeInformationUserControl : UserControl, IAnimeInformationUserInterface
    {
        public AnimeInformationUserControl()
        {
            InitializeComponent();
        }

        public event EditAnime OnEditAnime;
        public event WatchAnime OnWatchAnime;

        public void Bind(DataRow data)
        {
            throw new NotImplementedException();
        }
    }
}
