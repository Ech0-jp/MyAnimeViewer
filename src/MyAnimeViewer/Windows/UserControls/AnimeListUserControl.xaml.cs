using MyAnimeViewerInterfaces.GUI;
using System;
using System.Windows.Controls;
using System.Data.Common;

namespace MyAnimeViewer.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for AnimeListUserControl.xaml
    /// </summary>
    public partial class AnimeListUserControl : UserControl, IAnimeListUserInterface
    {
        public AnimeListUserControl()
        {
            InitializeComponent();
        }

        public UserControl View { get { return this; } }

        public event EventHandler<AnimeEventArgs> OnEditAnime;
        public event EventHandler<AnimeEventArgs> OnViewAnimeInformation;
        public event EventHandler<AnimeEventArgs> OnWatchAnime;

        public void BindList(DbDataAdapter dataAdapter)
        {

        }
    }
}
