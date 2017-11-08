using System;
using System.Data.Common;
using System.Windows.Controls;

namespace MyAnimeViewerInterfaces.GUI
{
    public interface IAnimeListUserInterface
    {
        void BindList(DbDataAdapter dataAdapter);
        event EventHandler<AnimeEventArgs> OnViewAnimeInformation;
        event EventHandler<AnimeEventArgs> OnEditAnime;
        event EventHandler<AnimeEventArgs> OnWatchAnime;

        UserControl View { get; }
    }
}