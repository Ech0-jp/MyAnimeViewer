using MyAnimeViewer.AniList.API;
using MyAnimeViewer.Enums.MyAnimeList;
using MyAnimeViewer.Errors;
using MyAnimeViewer.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyAnimeViewer.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for AL_AnimeList.xaml
    /// </summary>
    public partial class AL_AnimeListUC : UserControl
    {
        public AL_UserList UserList { get; private set; }   // Authenticated user's list.

        public AL_AnimeListUC()
        {
            InitializeComponent();
        }

        public async Task<bool> InitAniList(int userID)
        {
            UserList = new AL_UserList();
            if (await UserList.GenerateUserList(userID))
            {
                lv_AnimeList.ItemsSource = UserList.AnimeList;
                ListCollectionView view = (ListCollectionView)CollectionViewSource.GetDefaultView(lv_AnimeList.ItemsSource);
                view.CustomSort = new ListNameSorter();
                Sort("Title", ListSortDirection.Ascending);
                return true;
            }
            return false;
        }

        public void RefreshList()
        {
            lv_AnimeList.Items.Refresh();
            foreach (var item in lv_AnimeList.Items)
            {
                var list = item as ListView;
                if (list is ListView)
                    list.Items.Refresh();
            }
        }

        // IDK if this is still being used || Look into it later and remove if unused.
        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
            if (workingWidth <= 0 || gridView.Columns.Count < 4)
                return;
            var imageCol = 0.09;
            var titleCol = 0.67;
            var scoreCol = 0.07;
            var typeCol = 0.07;
            var progressCol = 0.1;

            gridView.Columns[0].Width = workingWidth * imageCol;
            gridView.Columns[1].Width = workingWidth * titleCol;
            gridView.Columns[2].Width = workingWidth * scoreCol;
            gridView.Columns[3].Width = workingWidth * typeCol;
            gridView.Columns[4].Width = workingWidth * progressCol;
        }

        /// <summary>
        /// Hides all lists except for the one that is determined by the button (Watching, Completed, OnHold, Dropped, Plan to Watch).
        /// </summary>
        private void HideAnimeList_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string list = "";

            switch ((string)button.Content)
            {
                case "Currently Watching":
                    list = "watching";
                    break;
                case "Completed":
                    list = "completed";
                    break;
                case "On Hold":
                    list = "on_hold";
                    break;
                case "Dropped":
                    list = "dropped";
                    break;
                case "Plan to Watch":
                    list = "plan_to_watch";
                    break;
                case "All Anime":
                    list = "All Anime";
                    break;
            }

            foreach (var item in lv_AnimeList.Items)
            {
                if (list == "All Anime")
                {
                    ListViewItem test = lv_AnimeList.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                    test.Visibility = Visibility.Visible;
                }
                else
                {
                    var animeItem = item as AL_AnimeList;
                    if (animeItem.ListName != list)
                    {
                        ListViewItem test = lv_AnimeList.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                        test.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ListViewItem test = lv_AnimeList.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                        test.Visibility = Visibility.Visible;
                    }
                }
            }
        }
        
        /// <summary>
        /// Sorts the list by the category determined by the button that was clicked (Anime Title, Score, Type, Progress).
        /// </summary>
        /// <param name="sender">The Button that triggered the click event.</param>
        private void AnimeListSort_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string sortBy = "";

            switch ((string)button.Content)
            {
                case "Anime Title":
                    sortBy = "Title";
                    break;
                case "Score":
                    sortBy = "Score";
                    break;
                case "Type":
                    sortBy = "Type";
                    break;
                case "Progress":
                    sortBy = "EpisodesWatched";
                    break;
                default:
                    return;
            }

            Sort(sortBy);
        }

        private void Sort(string sortBy, ListSortDirection? direction = null)
        {
            var temp = UIHelper.FindChildrenOfType<ListView>(lv_AnimeList);
            foreach (var item in temp)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(item.ItemsSource);

                if (direction != null)
                {
                    if (view.SortDescriptions.Count <= 0)
                        view.SortDescriptions.Add(new SortDescription(sortBy, (ListSortDirection)direction));
                    else
                        view.SortDescriptions[0] = new SortDescription(sortBy, (ListSortDirection)direction);
                }
                else
                {
                    if (view.SortDescriptions.Count <= 0)
                        view.SortDescriptions.Add(new SortDescription(sortBy, ListSortDirection.Descending));
                    else if (view.SortDescriptions[0].PropertyName == sortBy)
                    {
                        if (view.SortDescriptions[0].Direction == ListSortDirection.Descending)
                            view.SortDescriptions[0] = new SortDescription(sortBy, ListSortDirection.Ascending);
                        else
                            view.SortDescriptions[0] = new SortDescription(sortBy, ListSortDirection.Descending);
                    }
                    else
                        view.SortDescriptions[0] = new SortDescription(sortBy, ListSortDirection.Descending);
                }
            }
        }

        /// <summary>
        /// Directs the page to AL_AnimeInformation displaying all the information for the desired series.
        /// </summary>
        private async void AnimeImage_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem item = UIHelper.FindParent<ListViewItem>(sender as Button);
            TextBlock tb = UIHelper.FindChild<TextBlock>(item, "tb_Title");

            var anime = UserList.FindAnime(tb.Text);
            AL_AnimeModel animeModel = await anime.GetAnimePage();

            if (animeModel != null)
            {
                UserControl info = new AL_AnimeInformation(animeModel);
                Core.MainWindow.tContent.Content = info;
            }
        }

        /// <summary>
        /// Open up the Edit Anime Flyout to allow the user to edit the entry.
        /// </summary>
        private void EditAnime_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem item = UIHelper.FindParent<ListViewItem>(sender as Button);
            TextBlock tb = UIHelper.FindChild<TextBlock>(item, "tb_Title");

            var anime = UserList.FindAnime(tb.Text);
            Core.MainWindow.EditListItem(anime, false);
        }

        // Had to add this event to fix the issue with the scrollviewer not scrolling with the mouse wheel.
        private void sv_AnimeList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem item = UIHelper.FindParent<ListViewItem>(sender as Button);
            TextBlock tb = UIHelper.FindChild<TextBlock>(item, "tb_Title");
            var anime = UserList.FindAnime(tb.Text);

            CR_Simulcast content = new CR_Simulcast();
            content.Load(anime);
            Core.MainWindow.tContent.Content = content;
        }
    }
}
