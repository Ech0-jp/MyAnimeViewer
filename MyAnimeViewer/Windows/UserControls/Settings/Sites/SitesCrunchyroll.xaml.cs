using System.Windows.Controls;
using System.Windows;

namespace MyAnimeViewer.Windows.UserControls.Settings.Sites
{
    /// <summary>
    /// Interaction logic for SitesCrunchyroll.xaml
    /// </summary>
    public partial class SitesCrunchyroll : UserControl
    {
        private bool m_initialized;

        public SitesCrunchyroll()
        {
            InitializeComponent();
        }

        public void Load()
        {
            //if (!Core.CrLoggedIn)
            //{
            //    grd_Login.Visibility = Visibility.Visible;
            //    sp_options.Visibility = Visibility.Collapsed;
            //}
            m_initialized = true;
        }

        public void LoggedIn()
        {
            grd_Login.Visibility = Visibility.Collapsed;
            sp_options.Visibility = Visibility.Visible;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            //Core.CrClient.Logout();
            //Core.CrLoggedIn = false;

            //Properties.Settings.Default.CR_Username = null;
            //Properties.Settings.Default.CR_Password = null;
            //Config.Instance.CrunchyrollRememberLogin = false;
            //Config.Save();

            //grd_Login.Visibility = Visibility.Visible;
            //sp_options.Visibility = Visibility.Collapsed;
        }
    }
}
