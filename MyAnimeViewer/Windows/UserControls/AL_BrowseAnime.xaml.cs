using MyAnimeViewer.AniList.API;
using MyAnimeViewer.Enums.AniList;
using MyAnimeViewer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MyAnimeViewer.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for AL_BrowseAnime.xaml
    /// </summary>
    public partial class AL_BrowseAnime : UserControl
    {
        public List<AL_AnimeModel> Anime { get; private set; }
        private AL_BrowseParams m_browseParams;
        private List<AL_Genres?> m_genres;
        private List<AL_Genres?> m_genresExclude;

        private DispatcherTimer m_timer;

        public AL_BrowseAnime()
        {
            InitializeComponent();
            m_genres = new List<AL_Genres?>();
            m_genresExclude = new List<AL_Genres?>();
            m_timer = new DispatcherTimer();
            m_timer.Tick += m_timer_Tick;
            m_timer.Interval = new TimeSpan(0, 0, 1);
            Init();
        }

        private async void Init()
        {
            m_browseParams = new AL_BrowseParams
            {
                sort = SortBy.score,
            };
            if (!Core.MainWindow.AniListUser.AdultContent)
            {
                m_browseParams.genres_exclude = AL_Genres.Hentai.ToString();
                m_genresExclude.Add(AL_Genres.Hentai);
            }

            Anime = await AL_AnimeModel.Browse(m_browseParams);
            ic_Browse.ItemsSource = Anime;

            List<AL_MediaType> Type = new List<AL_MediaType>()
            {
                AL_MediaType.TV,
                AL_MediaType.Movie,
                AL_MediaType.Special,
                AL_MediaType.OVA,
                AL_MediaType.ONA,
                AL_MediaType.TVShort
            };
            lv_Type.ItemsSource = Type;
            
            List<string> Sort = new List<string>()
            {
                "Score",
                "Popularity",
                "Start Date",
                "End Date",
                "Date Added"
            };
            lv_Sort.ItemsSource = Sort;

            lv_Year.ItemsSource = MainWindow.YearUiList;
        }

        private async void Browse()
        {
            m_timer.Stop();
            Anime = await AL_AnimeModel.Browse(m_browseParams);
            ic_Browse.ItemsSource = Anime;
        }

        private async void AnimeItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = UIHelper.FindChild<Image>(sender as Canvas, "img_Background");
            string tag = img.Tag.ToString();

            if (string.IsNullOrEmpty(tag))
                return;

            var anime = await AL_AnimeModel.GetAnimePage(Convert.ToInt32(tag));

            if (anime != null)
            {
                UserControl info = new AL_AnimeInformation(anime, this);
                Core.MainWindow.tContent.Content = info;
            }
        }
        
        private void BrowseParamItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string name = (sender as Grid).Name.Replace("grd", "lv");

            if (name == lv_Year.Name)
                lv_Year.Visibility = lv_Year.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            else
                lv_Year.Visibility = Visibility.Collapsed;

            if (name == lv_Season.Name)
                lv_Season.Visibility = lv_Season.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            else
                lv_Season.Visibility = Visibility.Collapsed;

            if (name == lv_Status.Name)
                lv_Status.Visibility = lv_Status.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            else
                lv_Status.Visibility = Visibility.Collapsed;

            if (name == lv_Type.Name)
                lv_Type.Visibility = lv_Type.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            else
                lv_Type.Visibility = Visibility.Collapsed;

            if (name == lv_Sort.Name)
                lv_Sort.Visibility = lv_Sort.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            else
                lv_Sort.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Event triggered when a new sorting description is applied from one of the sorting dropdowns.
        /// </summary>
        private void ListViewDropDownItem_Click(object sender, RoutedEventArgs e)
        {
            m_timer.Stop();
            string parent = UIHelper.FindParent<ListView>(sender as Button).Name;

            switch (parent)
            {
                case "lv_Year":
                    SortYear(sender);
                    break;
                case "lv_Season":
                    SortSeason(sender);
                    break;
                case "lv_Status":
                    SortStatus(sender);
                    break;
                case "lv_Type":
                    SortType(sender);
                    break;
                case "lv_Sort":
                    SortSort(sender);
                    break;
            }
            m_timer.Start();
        }

        private void SortYear(object sender)
        {
            m_browseParams.year =  Convert.ToInt32((sender as Button).Tag);
            tb_YearSelectedItem.Text = m_browseParams.year.ToString();
            btn_RemoveYear.Visibility = Visibility.Visible;
            lv_Year.Visibility = Visibility.Collapsed;
        }

        private void SortSeason(object sender)
        {
            m_browseParams.season = (sender as Button).Tag as Season?;
            tb_SeasonSelectedItem.Text = m_browseParams.season.ToString();
            btn_RemoveSeason.Visibility = Visibility.Visible;
            lv_Season.Visibility = Visibility.Collapsed;
        }

        private void SortStatus(object sender)
        {
            m_browseParams.status = (sender as Button).Tag as AL_AnimeStatus?;
            tb_StatusSelectedItem.Text = m_browseParams.status.ToString();
            btn_RemoveStatus.Visibility = Visibility.Visible;
            lv_Status.Visibility = Visibility.Collapsed;
        }

        private void SortType(object sender)
        {
            m_browseParams.type = (sender as Button).Tag as AL_MediaType?;
            tb_TypeSelectedItem.Text = m_browseParams.type.ToString();
            btn_RemoveType.Visibility = Visibility.Visible;
            lv_Type.Visibility = Visibility.Collapsed;
        }

        private void SortSort(object sender)
        {
            string result = (sender as Button).Tag as string;
            m_browseParams.sort = Enum.Parse(typeof(SortBy), result.Replace(" ", "_").ToLower()) as SortBy?;
            tb_SortSelectedItem.Text = result;
            btn_RemoveSort.Visibility = Visibility.Visible;
            lv_Sort.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Event is triggered when user clicks the "X" on an applied sort description.
        /// </summary>
        private void ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            m_timer.Stop();
            var btn = sender as Button;
            btn.Visibility = Visibility.Hidden;
            switch (btn.Name)
            {
                case "btn_RemoveYear":
                    tb_YearSelectedItem.Text = "";
                    m_browseParams.year = null;
                    break;
                case "btn_RemoveSeason":
                    tb_SeasonSelectedItem.Text = "";
                    m_browseParams.season = null;
                    break;
                case "btn_RemoveStatus":
                    tb_StatusSelectedItem.Text = "";
                    m_browseParams.status = null;
                    break;
                case "btn_RemoveType":
                    tb_TypeSelectedItem.Text = "";
                    m_browseParams.type = null;
                    break;
                case "btn_RemoveSort":
                    tb_SortSelectedItem.Text = "";
                    m_browseParams.sort = null;
                    break;
            }
            m_timer.Start();
        }

        /// <summary>
        /// When ListView Genres loads, check to see if user has "Adult Content" disabled. If true, remove the sorting option "Hentai" from the genres list.
        /// </summary>
        private void lv_Genres_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Core.MainWindow.AniListUser.AdultContent)
            {
                AL_Genres[] items = lv_Genres.ItemsSource as AL_Genres[];
                lv_Genres.ItemsSource = items.Where(val => val != AL_Genres.Hentai).ToArray();
                lv_Genres.Items.Refresh();
            }
        }

        private void GenreItem_Click(object sender, RoutedEventArgs e)
        {
            m_timer.Stop();
            /* The Order that ToggleButtons set isChecked when clicked
             * false >> true >> null
             *
             * The order we want isChecked to be set in.
             * true >> false >> null
             *
             * The conversion to get the order we want.
             * false -> true
             * null -> false
             * true -> null
             */
            ToggleButton btn = sender as ToggleButton;
            switch (btn.IsChecked)
            {
                case null:
                    btn.IsChecked = false;
                    btn.Foreground = new SolidColorBrush(Colors.Red);
                    if (m_genres.Contains(btn.Content as AL_Genres?))
                        m_genres.Remove(btn.Content as AL_Genres?);
                    m_genresExclude.Add(btn.Content as AL_Genres?);
                    break;
                case true:
                    btn.IsChecked = null;
                    btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0c597b"));
                    if (m_genres.Contains(btn.Content as AL_Genres?))
                        m_genres.Remove(btn.Content as AL_Genres?);
                    if (m_genresExclude.Contains(btn.Content as AL_Genres?))
                        m_genresExclude.Remove(btn.Content as AL_Genres?);
                    break;
                case false:
                    btn.IsChecked = true;
                    btn.Foreground = new SolidColorBrush(Colors.Green);
                    if (m_genresExclude.Contains(btn.Content as AL_Genres?))
                        m_genresExclude.Remove(btn.Content as AL_Genres?);
                    m_genres.Add(btn.Content as AL_Genres?);
                    break;
            }
            m_browseParams.genres = string.Join(",", m_genres.ToArray());
            m_browseParams.genres_exclude = string.Join(",", m_genresExclude.ToArray());
            m_timer.Start();
        }

        private void m_timer_Tick (object sender, EventArgs e)
        {
            Browse();
        }

        private async void tb_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            e.Handled = true;

            if (string.IsNullOrEmpty(tb_Search.Text))
                Browse();
            else
                ic_Browse.ItemsSource = await AL_AnimeModel.Search(tb_Search.Text);
        }
    }
}
