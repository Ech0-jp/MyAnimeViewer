using System.Windows;
using System.Windows.Controls;

namespace MyAnimeViewer.Windows.UserControls.Settings.MyAnimeViewer
{
    /// <summary>
    /// Interaction logic for MyAnimeViewerGeneral.xaml
    /// </summary>
    public partial class MyAnimeViewerGeneral : UserControl
    {
        private bool m_initialized;
        public MyAnimeViewerGeneral()
        {
            InitializeComponent();
        }

        public void Load()
        {
            cb_ShowLogin.IsChecked = Config.Instance.ShowLoginDialog;

            m_initialized = true;
        }

        private void cb_ShowLogin_Checked(object sender, RoutedEventArgs e)
        {
            if (!m_initialized)
                return;
            Config.Instance.ShowLoginDialog = true;
            Config.Save();
        }

        private void cb_ShowLogin_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!m_initialized)
                return;
            Config.Instance.ShowLoginDialog = false;
            Config.Save();
        }
    }
}
