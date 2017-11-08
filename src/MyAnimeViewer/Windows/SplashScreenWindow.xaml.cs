using System;
using System.Windows;

namespace MyAnimeViewer.Windows
{
    /// <summary>
    /// Interaction logic for SplashScreenWindow.xaml
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            InitializeComponent();
        }

        public void ShowConditional()
        {
            if (Config.Instance.ShowSplashScreen)
                Show();
        }
    }
}
