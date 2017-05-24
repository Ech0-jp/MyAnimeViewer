using MyAnimeViewer.Utility.Logging;
using MyAnimeViewer.Windows.UserControls.Settings.MyAnimeViewer;
using MyAnimeViewer.Windows.UserControls.Settings.Sites;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyAnimeViewer.Windows.UserControls.Settings
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public readonly SitesAniList OptionsSitesAniList = new SitesAniList(); 
        public readonly SitesCrunchyroll OptionsSitesCrunchyroll = new SitesCrunchyroll();
        public readonly MyAnimeViewerGeneral OptionsMyAnimeViewerGeneral = new MyAnimeViewerGeneral();

        public Settings()
        {
            InitializeComponent();
            try
            {
                foreach (var item in tv_Options.Items.Cast<TreeViewItem>())
                    item.ExpandSubtree();
                tv_Options.Items.Cast<TreeViewItem>().ToArray()[1].Items.Cast<TreeViewItem>().First().IsSelected = true;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            Load();
        }

        private void Load()
        {
            OptionsMyAnimeViewerGeneral.Load();
            //OptionsSitesAniList.Load();
            OptionsSitesCrunchyroll.Load();
        }

        public void CrunchyrollLogin()
        {
            tv_Options.Items.Cast<TreeViewItem>().ToArray()[0].Items.Cast<TreeViewItem>().ToArray()[1].IsSelected = true;
        }

        private void tvi_SitesAniList_Selected(object sender, RoutedEventArgs e) => cc_Options.Content = OptionsSitesAniList;
        private void tvi_SitesCrunchyroll_Selected(object sender, RoutedEventArgs e) => cc_Options.Content = OptionsSitesCrunchyroll;
        private void tvi_MyAnimeViewerGeneral_Selected(object sender, RoutedEventArgs e) => cc_Options.Content = OptionsMyAnimeViewerGeneral;
    }
}
