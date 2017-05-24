using CloudFlareUtilities;
using HtmlAgilityPack;
using MyAnimeViewer.AniList.API;
using MyAnimeViewer.Errors;
using MyAnimeViewer.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyAnimeViewer.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for CR_Simulcast.xaml
    /// </summary>
    public partial class CR_Simulcast : UserControl
    {
        private const string baseURL = @"http://kissanime.ru/";

        private AL_AnimeListModel m_anime;
        private int m_currentEpisode;
        private List<string> m_episodeLinks;
        private Uri m_episodeUri;

        private bool m_videoLoaded;
        private bool m_isMaximized;
        private WindowState m_previouseState;

        public CR_Simulcast()
        {
            InitializeComponent();
        }

        public async void Load(AL_AnimeListModel anime)
        {
            Log.Info("Loading anime simulcast..");
            m_anime = anime;
            m_currentEpisode = anime.EpisodesWatched + 1;
            tb_Title.Text = $"{anime.Title} Episode {anime.EpisodesWatched + 1}";
            m_episodeLinks = new List<string>();
            try
            {
                var handler = new ClearanceHandler();
                using (var client = new HttpClient(handler))
                {
                    Log.Info("Generating anime episode URLs..");

                    string url = $"{baseURL}Anime/{anime.Title.Replace(":", "").Replace(" ", "-")}";
                    var response = await client.GetAsync(url);
                    pb_Loading.Value = 20;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        pb_Loading.Value += 20;

                        HtmlDocument document = new HtmlDocument();
                        document.LoadHtml(result);
                        
                        var node = document.DocumentNode.SelectSingleNode("//table[@class='listing']");
                        var collection = node.SelectNodes(".//a");
                        int progressAmount = 20 / collection.Count;
                        foreach (var item in collection)
                        {
                            string target = item.Attributes["href"].Value;
                            if (!string.IsNullOrEmpty(target))
                                m_episodeLinks.Add(target + "&s=openload");
                            pb_Loading.Value += progressAmount;
                        }
                        m_episodeLinks.Sort();
                        pb_Loading.Value += 20;

                        Log.Info("Anime episode URLs generated.");

                        string episodeUrl = baseURL + m_episodeLinks[m_currentEpisode - 1];
                        LoadVideo(new Uri(episodeUrl));

                        List<string> episodeList = new List<string>();
                        for (int i = 1; i <= anime.TotalEpisodes; i++)
                        {
                            episodeList.Add($"Episode {i}");
                        }
                        cb_Episodes.ItemsSource = episodeList;
                        cb_Episodes.SelectedIndex = m_currentEpisode - 1;
                        sp_Episodes.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (AggregateException ex) when (ex.InnerException is CloudFlareClearanceException)
            {
                Log.Error($"Clearance failed when attempting to generate URLs.. {ex}");
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                Log.Error($"Client timed out when attempting to generate URLs.. {ex}");
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Error retrieving video.", $"A fatal error occured when attempting to generate URLs. {e}");
                return;
            }
            #region Crunchyroll
            //if (!Core.CrLoggedIn)
            //{
            //    img_Error.Source = new BitmapImage(new Uri("/MyAnimeViewer;component/Resources/Crunchyroll_LoginRequired.png", UriKind.RelativeOrAbsolute));
            //    img_Error.Visibility = Visibility.Visible;
            //    me_Simulcast.Visibility = Visibility.Collapsed;
            //    Core.MainWindow.fo_Settings.IsOpen = true;
            //    (Core.MainWindow.fo_Settings.Content as Settings.Settings).CrunchyrollLogin();
            //}
            //else
            //{
            //    var serieslist = await Core.CrClient.GetSeriesList("anime", null, 0, 0);
            //    m_series = serieslist.FirstOrDefault(s => s.Name.ToLower().StartsWith(seriesName.ToLower()));
            //    if (m_series != null)
            //    {
            //        m_episodes = await Core.CrClient.GetMediaList(null, m_series.SeriesId, null, null, null);
            //        var episode = m_episodes.ElementAt(episodeNumber == 0 ? 0 : episodeNumber);
            //        if (episode != null)
            //        {
            //            var stream = await Core.CrClient.GetMediaStream(episode.MediaId);

            //            int debug = 1;
            //        }
            //    }
            //    else
            //    {
            //        img_Error.Source = new BitmapImage(new Uri("/MyAnimeViewer;component/Resources/Crunchyroll_VideoUnavailable.png", UriKind.RelativeOrAbsolute));
            //        img_Error.Visibility = Visibility.Visible;
            //        me_Simulcast.Visibility = Visibility.Collapsed;
            //    }
            //}
            #endregion
        }

        private async void LoadVideo(Uri episodeUri)
        {
            btn_Fullscreen.Visibility = Visibility.Collapsed;
            grd_Loading.Visibility = Visibility.Visible;
            pr_Loading.IsActive = true;

            try
            {
                var handler = new ClearanceHandler();
                using (var client = new HttpClient(handler))
                {
                    Log.Info("Retrieving episode..");
                    
                    var response = await client.GetAsync(episodeUri);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        pb_Loading.Value += 20;

                        HtmlDocument document = new HtmlDocument();
                        document.LoadHtml(result);

                        var script = document.DocumentNode.SelectSingleNode("//script[contains(text(), 'ReloadIfNeed')]").InnerHtml;
                        var src = Regex.Match(script, "src=.*?[a-zA-Z0-9]+\"");
                        if (src.Success)
                        {
                            string episodeUrl = src.Captures[0].Value;
                            episodeUrl = episodeUrl.Replace("src=", "").Replace("\"", "");
                            m_episodeUri = new Uri(episodeUrl);
                            wb_simulcast.Source = m_episodeUri;

                            btn_Fullscreen.Visibility = Visibility.Visible;
                            grd_Loading.Visibility = Visibility.Collapsed;
                            pr_Loading.IsActive = false;
                            pb_Loading.Visibility = Visibility.Collapsed;

                            m_videoLoaded = true;
                            Log.Info("Episode retrieved.");
                        }
                    }
                }
            }
            catch (AggregateException ex) when (ex.InnerException is CloudFlareClearanceException)
            {
                Log.Error($"Clearance failed when attempting to retrieve episode.. {ex}");
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                Log.Error($"Client timed out when attempting to retrieve episode.. {ex}");
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Error retrieving video.", $"A fatal error occured when attempting to retrieve video. {e}");
            }
        }

        // have to bootleg fullscreen the video as Awesomium apparently doesn't like to acknowledge fullscreen requests from flashplayer .....
        private void btn_Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            if (!wb_simulcast.IsLoaded || wb_simulcast.Source == null)
                return;
            if (m_isMaximized)
            {
                Core.MainWindow.IgnoreTaskbarOnMaximize = false;
                Core.MainWindow.ShowTitleBar = true;
                Core.MainWindow.WindowState = m_previouseState;
                m_isMaximized = false;

                Core.MainWindow.btn_Menu.Visibility = Visibility.Visible;
                tb_Title.Visibility = Visibility.Visible;
                sp_Episodes.Visibility = Visibility.Visible;
                grd_Video.Width = 640;
                grd_Video.Height = 360;
            }
            else
            {
                m_previouseState = Core.MainWindow.WindowState;
                Core.MainWindow.IgnoreTaskbarOnMaximize = true;
                Core.MainWindow.ShowTitleBar = false;
                Core.MainWindow.WindowState = WindowState.Maximized;
                m_isMaximized = true;

                Core.MainWindow.btn_Menu.Visibility = Visibility.Collapsed;
                tb_Title.Visibility = Visibility.Collapsed;
                sp_Episodes.Visibility = Visibility.Collapsed;
                grd_Video.Width = Core.MainWindow.ActualWidth;
                grd_Video.Height = Core.MainWindow.ActualHeight;
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            cb_Episodes.SelectedIndex--;
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            cb_Episodes.SelectedIndex++;
        }

        private void cb_Episodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Episodes.SelectedIndex == 0)
                btn_Back.Visibility = Visibility.Collapsed;
            else
                btn_Back.Visibility = Visibility.Visible;

            if (cb_Episodes.SelectedIndex == m_anime.TotalEpisodes - 1)
                btn_Next.Visibility = Visibility.Collapsed;
            else
                btn_Next.Visibility = Visibility.Visible;

            if (!m_videoLoaded)
                return;
            m_currentEpisode = cb_Episodes.SelectedIndex + 1;
            tb_Title.Text = $"{m_anime.Title} Episode {m_currentEpisode}";
            string episodeUrl = baseURL + m_episodeLinks[m_currentEpisode - 1];
            LoadVideo(new Uri(episodeUrl));
        }
    }
}
