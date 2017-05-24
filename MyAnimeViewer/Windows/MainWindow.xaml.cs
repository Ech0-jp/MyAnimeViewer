using MyAnimeViewer.AniList.API;
using MyAnimeViewer.Enums.MyAnimeList;
using MyAnimeViewer.Errors;
using MyAnimeViewer.Windows.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MyAnimeViewer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public static readonly IList<string> ScoreUiList = new ReadOnlyCollection<string> //Used to populate the SplitButton "sb_Score" in the Flyout "fo_EditListItem".
                                                           (new List<string>
                                                           {
                                                               "Select score", "(10) Masterpiece", "(9) Great", "(8) Very Good", "(7) Good", "(6) Fine", "(5) Average", "(4) Bad", "(3) Very Bad", "(2) Horrible", "(1) Appalling"
                                                           });
        public static readonly IList<string> MonthUiList = new ReadOnlyCollection<string>
                                                           (new List<string>
                                                           {
                                                               "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
                                                           });
        public static List<string> DayUiList { get; private set; }
        public static List<string> YearUiList { get; private set; }

        private AL_AnimeListUC m_aniListUC;
        public AL_AnimeListUC AniListUC { get { return m_aniListUC; } }

        private AL_User m_aniListUser;
        public AL_User AniListUser { get { return m_aniListUser; } }

        private MAL_LoginType m_usingMyAnimeList;
        private bool m_usingAniList;

        private AL_AnimeListModel m_item;
        private bool m_newEntry;
        private AL_AnimeModel m_model;

        public MainWindow(MAL_LoginType usingMyAnimeList, bool usingAniList)
        {
            DayUiList = new List<string>();
            for (int i = 1; i <= 31; i++)
            {
                DayUiList.Add(i.ToString());
            }
            YearUiList = new List<string>();
            DateTime dt = DateTime.Now;
            while (dt.Year > 1986)
            {
                YearUiList.Add(dt.Year.ToString());
                dt = dt.AddYears(-1);
            }
            InitializeComponent();
            m_usingMyAnimeList = usingMyAnimeList;
            m_usingAniList = usingAniList;
        }

        public async Task<bool> InitList()
        {
            m_aniListUser = new AL_User();
            await m_aniListUser.GetAuthenticatedUser();
            m_aniListUC = new AL_AnimeListUC();
            bool result = await m_aniListUC.InitAniList(m_aniListUser.ID);
            SetContentAnimeList();
            return true;
        }

        public void SetContentAnimeList()
        {
            tContent.Content = m_aniListUC;
            m_aniListUC.RefreshList();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            fo_Settings.IsOpen = !fo_Settings.IsOpen;
        }
        
        internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

#region EditListItem_FlyoutControls
        /// <summary>
        /// Initialize and open the flyout control for Editing List Item.
        /// </summary>
        /// <param name="item">The item to edit or add.</param>
        /// <param name="newEntry">true if adding a new entry || false if editing existing entry.</param>
        public void EditListItem(AL_AnimeListModel item, bool newEntry, AL_AnimeModel model = null)
        {
            if (item == null)
                throw new ArgumentNullException("item", "This argument cannot be null.");

            m_item = item;
            m_newEntry = newEntry;
            m_model = model;

            tb_title.Text = item.Title;
            
            Dictionary<string, string> lists = new Dictionary<string, string>();
            foreach (var list in m_aniListUC.UserList.AnimeList)
            {
                lists.Add(list.ListName, list.ListNameFormatted);
            }
            sb_Status.ItemsSource = lists;
            sb_Status.DisplayMemberPath = "Value";

            if (!newEntry)
            {
                for (int i = 0; i < sb_Status.Items.Count; i++)
                {
                    dynamic value = sb_Status.Items[i];
                    if (value.Value == item.ListStatusFormatted)
                    {
                        sb_Status.SelectedIndex = i;
                        break;
                    }
                }
            }

            tb_epWatched.Text = item.EpisodesWatched.ToString();
            tb_totalEp.Text = $" / {item.TotalEpisodes.ToString()}";

            sb_Score.ItemsSource = ScoreUiList;
            sb_Score.SelectedIndex = newEntry || item.Score == 0 ? 0 : (item.Score - 11) * -1;

            DateTime dt = new DateTime(); // used to see if start and finish date was assigned or not.

            sb_StartDateMonth.ItemsSource = MonthUiList;
            sb_StartDateDay.ItemsSource = DayUiList;
            sb_StartDateYear.ItemsSource = YearUiList;
            if (item.StartedOn != dt)
            {
                int month = item.StartedOn.Month;
                int day = item.StartedOn.Day;
                int year = item.StartedOn.Year;
                int yearIndex = DateTime.Now.Year - item.StartedOn.Year;

                sb_StartDateMonth.SelectedIndex = month - 1;
                sb_StartDateDay.SelectedIndex = day - 1;
                sb_StartDateYear.SelectedIndex = yearIndex;
                cb_StartDateUnkown.IsChecked = false;
            }
            else
            {
                sb_StartDateMonth.SelectedIndex = -1;
                sb_StartDateDay.SelectedIndex = -1;
                sb_StartDateYear.SelectedIndex = -1;
                cb_StartDateUnkown.IsChecked = true;
            }

            sb_FinishDateMonth.ItemsSource = MonthUiList;
            sb_FinishDateDay.ItemsSource = DayUiList;
            sb_FinishDateYear.ItemsSource = YearUiList;
            if (item.FinishedOn != dt)
            {
                int month = item.FinishedOn.Month;
                int day = item.FinishedOn.Day;
                int year = item.FinishedOn.Year;
                int yearIndex = DateTime.Now.Year - item.FinishedOn.Year;

                sb_FinishDateMonth.SelectedIndex = month - 1;
                sb_FinishDateDay.SelectedIndex = day - 1;
                sb_FinishDateYear.SelectedIndex = yearIndex;
                cb_FinishDateUnkown.IsChecked = false;
            }
            else
            {
                sb_FinishDateMonth.SelectedIndex = -1;
                sb_FinishDateDay.SelectedIndex = -1;
                sb_FinishDateYear.SelectedIndex = -1;
                cb_FinishDateUnkown.IsChecked = true;
            }

            fo_EditListItem.IsOpen = !fo_EditListItem.IsOpen;
        }

        /// <summary>
        /// Assigns the SplitButtons for either the Start Date or Finish Date to DateTime.Now
        /// </summary>
        private void InsertToday_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "btn_InsertStartDate":
                    if ((bool)cb_StartDateUnkown.IsChecked)
                        cb_StartDateUnkown.IsChecked = false;
                    sb_StartDateMonth.SelectedIndex = DateTime.Now.Month - 1;
                    sb_StartDateDay.SelectedIndex = DateTime.Now.Day - 1;
                    sb_StartDateYear.SelectedIndex = 0;
                    break;
                case "btn_InsertFinishDate":
                    if ((bool)cb_FinishDateUnkown.IsChecked)
                        cb_FinishDateUnkown.IsChecked = false;
                    sb_FinishDateMonth.SelectedIndex = DateTime.Now.Month - 1;
                    sb_FinishDateDay.SelectedIndex = DateTime.Now.Day - 1;
                    sb_FinishDateYear.SelectedIndex = 0;
                    break;
            }
        }

        /// <summary>
        /// Add a new entry to the user's list or edit an existing entry in the user's list.
        /// </summary>
        private async void EditListItemSubmit_Click(object sender, RoutedEventArgs e)
        {
            int id = m_item.ID;
            string status = (sb_Status.SelectedItem as dynamic).Key;
            int episodesWatched = string.IsNullOrEmpty(tb_epWatched.Text) ? 0 : Convert.ToInt32(tb_epWatched.Text);
            int score = (sb_Score.SelectedIndex - 11) * -1;
            score = score == 11 ? 0 : score;
            int rewatched = -1;
            string notes = "";
            DateTime startDate;
            if ((bool)cb_StartDateUnkown.IsChecked)
            {
                startDate = new DateTime();
            }
            else
            {
                var startYear = sb_StartDateYear.SelectedItem;
                int startMonth = sb_StartDateMonth.SelectedIndex + 1;
                int startDay = sb_StartDateDay.SelectedIndex + 1;
                startDate = (startYear != null && startMonth != 0 && startDay != 0) ? new DateTime(Convert.ToInt32(startYear.ToString()), startMonth, startDay) : new DateTime();
            }
            DateTime finishDate;
            if ((bool)cb_FinishDateUnkown.IsChecked)
            {
                finishDate = new DateTime();
            }
            else
            {
                var finishYear = sb_FinishDateYear.SelectedItem;
                int finishMonth = sb_FinishDateMonth.SelectedIndex + 1;
                int finishDay = sb_FinishDateDay.SelectedIndex + 1;
                finishDate = (finishYear != null && finishMonth != 0 && finishDay != 0) ? new DateTime(Convert.ToInt32(finishYear.ToString()), finishMonth, finishDay) : new DateTime();
            }

            if (m_newEntry)
            {
                if (await m_aniListUC.UserList.CreateAnimeEntry(m_model, status, episodesWatched, score, rewatched, notes, startDate, finishDate))
                    m_aniListUC.RefreshList();
            }
            else
            {
                if (await m_aniListUC.UserList.EditAnimeEntry(id, status, episodesWatched, score, rewatched, notes, startDate, finishDate))
                    m_aniListUC.RefreshList();
            }
            fo_EditListItem.IsOpen = false;
            m_item = null;
        }

        /// <summary>
        /// Cancel the edit.
        /// </summary>
        private void EditListItemCancel_Click(object sender, RoutedEventArgs e)
        {
            fo_EditListItem.IsOpen = false;
            m_item = null;
        }

        /// <summary>
        /// Preview the inputted text to make sure that it is a positive Integer.
        /// </summary>
        private void tb_epWatched_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        /// <summary>
        /// Check to see if the text is a postive integer.
        /// </summary>
        /// <param name="text">Text to compare.</param>
        /// <returns>True if text is integer.</returns>
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex(@"^[1-9]\d*$");
            return regex.IsMatch(text);
        }

        /// <summary>
        /// Compares the existing text in "tb_epWatched" with the total number of episodes for the series. If this number is greater than the total episodes, set the text to total episodes.
        /// </summary>
        private void tb_epWatched_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num = !string.IsNullOrEmpty(tb_epWatched.Text) ? Convert.ToInt32(tb_epWatched.Text) : -1;
            if (num != -1)
            {
                if (num > m_item.TotalEpisodes)
                {
                    tb_epWatched.Text = m_item.TotalEpisodes.ToString();
                    tb_epWatched.CaretIndex = tb_epWatched.Text.Length;
                }
            }
        }

        private void DateUnkown_Changed(object sender, RoutedEventArgs e)
        {
            switch ((sender as CheckBox).Name)
            {
                case "cb_StartDateUnkown":
                    sb_StartDateMonth.IsEnabled = !(bool)cb_StartDateUnkown.IsChecked;
                    sb_StartDateDay.IsEnabled = !(bool)cb_StartDateUnkown.IsChecked;
                    sb_StartDateYear.IsEnabled = !(bool)cb_StartDateUnkown.IsChecked;
                    break;
                case "cb_FinishDateUnkown":
                    sb_FinishDateMonth.IsEnabled = !(bool)cb_FinishDateUnkown.IsChecked;
                    sb_FinishDateDay.IsEnabled = !(bool)cb_FinishDateUnkown.IsChecked;
                    sb_FinishDateYear.IsEnabled = !(bool)cb_FinishDateUnkown.IsChecked;
                    break;
            }
        }
#endregion

#region Errors
        public ObservableCollection<Error> Errors => ErrorManager.Errors;

        public Visibility ErrorIconVisibility => ErrorManager.ErrorIconVisibility;

        public string ErrorCount => ErrorManager.Errors.Count > 1 ? $"({ErrorManager.Errors.Count})" : "";

        private void BtnErrors_OnClick(object sender, RoutedEventArgs e) => fo_Errors.IsOpen = !fo_Errors.IsOpen;

        public void ErrorsPropertyChanged()
        {
            OnPropertyChanged(nameof(Errors));
            OnPropertyChanged(nameof(ErrorIconVisibility));
            OnPropertyChanged(nameof(ErrorCount));
        }
#endregion

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
        
        private void btn_Menu_Loaded(object sender, RoutedEventArgs e)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(m_aniListUser.ImageUrlMedium);
            bitmap.EndInit();

            (btn_Menu.Template.FindName("img_UserImage", btn_Menu) as Image).Source = bitmap;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta * 0.5);
            e.Handled = true;
        }

        private void btn_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            UserControl uc = tContent.Content as UserControl;

            switch (btn.Name)
            {
                case "btn_list":
                    if (uc is AL_AnimeListUC)
                        return;
                    tContent.Content = m_aniListUC;
                    break;
                case "btn_Browse":
                    if (uc is AL_BrowseAnime)
                        return;
                    AL_BrowseAnime control = new AL_BrowseAnime();
                    tContent.Content = control;
                    break;
                case "btn_Logout":
                    // IMPLEMENT LOGOUT !!!
                    break;
                case "btn_Settings":
                    fo_Settings.IsOpen = !fo_Settings.IsOpen;
                    break;
                default:
                    break;
            }
        }
    }
}
