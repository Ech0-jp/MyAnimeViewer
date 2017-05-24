using System;
using System.Collections.Generic;
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
using MahApps.Metro.Controls;
using System.ComponentModel;
using MyAnimeViewer.MyAnimeList.API;
using System.Collections.ObjectModel;
using MyAnimeViewer.AniList.API;
using MyAnimeViewer.Enums.MyAnimeList;

namespace MyAnimeViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
    {
        private MAL_LoginType m_usingMyAnimeList;
        private bool m_usingAniList;
        
        // AniList;
        private AL_User m_ALUser;
        private AL_UserList m_ALUserList;

        public MainWindow(MAL_LoginType usingMyAnimeList, bool usingAniList)
        {
            InitializeComponent();
            m_usingMyAnimeList = usingMyAnimeList;
            m_usingAniList = usingAniList;

            if (m_usingAniList)
                InitAniList();
        }

        private async void InitAniList()
        {
            m_ALUser = new AL_User();
            bool userResult = await m_ALUser.GetAuthenticatedUser();
            m_ALUserList = new AL_UserList(m_ALUser.getID);
        }

        private async void InitMAL()
        {
            if (await MyAnimeListAPI.GenerateAnimeList())
            {
                lv_Watching.ItemsSource = MyAnimeListAPI.AnimeList_Watching;
                lv_Completed.ItemsSource = MyAnimeListAPI.AnimeList_Completed;
                lv_OnHold.ItemsSource = MyAnimeListAPI.AnimeList_OnHold;
                lv_Dropped.ItemsSource = MyAnimeListAPI.AnimeList_Dropped;
                lv_PlanToWatch.ItemsSource = MyAnimeListAPI.AnimeList_PlanToWatch;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                Config.Save();
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
            if (workingWidth <= 0)
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

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Window window = sender as Window;
           sv_AnimeList.Height = window.ActualHeight - 60;
        }

        /// <summary>
        /// Hides all lists except for the one that is determined by the button (Watching, Completed, OnHold, Dropped, Plan to Watch).
        /// </summary>
        /// <param name="sender">The Button that triggered the click event.</param>
        private void HideAnimeList_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string list = "";
            string text = "";

            switch ((string)button.Content)
            { 
                case "Currently Watching":
                    list = "lv_Watching";
                    text = "tb_Watching";
                    break;
                case "Completed":
                    list = "lv_Completed";
                    text = "tb_Completed";
                    break;
                case "On Hold":
                    list = "lv_OnHold";
                    text = "tb_OnHold";
                    break;
                case "Dropped":
                    list = "lv_Dropped";
                    text = "tb_Dropped";
                    break;
                case "Plan to Watch":
                    list = "lv_PlanToWatch";
                    text = "tb_PlanToWatch";
                    break;
                case "All Anime":
                    list = "All Anime";
                    break;
            }

            ListView[] views = { lv_Watching, lv_Completed, lv_OnHold, lv_Dropped, lv_PlanToWatch };
            TextBlock[] blocks = { tb_Watching, tb_Completed, tb_OnHold, tb_Dropped, tb_PlanToWatch };

            foreach (TextBlock block in blocks)
            {
                if (list == "All Anime")
                    block.Visibility = System.Windows.Visibility.Collapsed;
                if (block.Name != text)
                    block.Visibility = System.Windows.Visibility.Collapsed;
                else
                    block.Visibility = System.Windows.Visibility.Visible;
            }

            foreach (ListView view in views)
            {
                if (list == "All Anime")
                    view.Visibility = System.Windows.Visibility.Visible;
                else if (view.Name != list)
                    view.Visibility = System.Windows.Visibility.Collapsed;
                else
                    view.Visibility = System.Windows.Visibility.Visible;
            }
            
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            fo_Settings.IsOpen = !fo_Settings.IsOpen;
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
                    sortBy = "Series_Title";
                    break;
                case "Score":
                    sortBy = "My_Score";
                    break;
                case "Type":
                    sortBy = "Series_Type";
                    break;
                case "Progress":
                    sortBy = "My_Watched_Episodes";
                    break;
            }

            CollectionView[] views = { (CollectionView)CollectionViewSource.GetDefaultView(lv_Watching.ItemsSource),
                                       (CollectionView)CollectionViewSource.GetDefaultView(lv_Completed.ItemsSource),
                                       (CollectionView)CollectionViewSource.GetDefaultView(lv_OnHold.ItemsSource),
                                       (CollectionView)CollectionViewSource.GetDefaultView(lv_Dropped.ItemsSource),
                                       (CollectionView)CollectionViewSource.GetDefaultView(lv_PlanToWatch.ItemsSource) };

            foreach (CollectionView view in views)
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

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Image clicked!");
        }
    }
}
