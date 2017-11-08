using MyAnimeViewer.Plugins;
using MyAnimeViewer.Utility;
using MyAnimeViewer.Utility.Converters;
using MyAnimeViewer.Utility.Logging;
using MyAnimeViewerInterfaces;
using MyAnimeViewerInterfaces.AnimeDB;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyAnimeViewer.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        public PluginWrapper SelectedPlugin { get; private set; } // The plugin selected to login to.
        public bool LoggedIn { get; private set; } // Required for when this window closes, the Core will be able to determine successful login.
        private ImageConverter _imageConverter = new ImageConverter(); // The image converter utility to convert the Selected plugins login logo and add it to the flyout.

        public LoginWindow()
        {
            InitializeComponent();
            var source = PluginManager.Instance.Plugins.Where(p => p.Plugin.Type == PluginType.AnimeDatabase);
            foreach (var p in source)
                p.Load();
            lv_LoginButtons.ItemsSource = source;
        }

        /// <summary>
        /// Get the plugin that we are attempting to login to
        /// and open the corrosponding flyout.
        /// </summary>
        private void AnimeDbButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            SelectedPlugin = PluginManager.Instance.Plugins.Where(p => p.Name == btn.Tag.ToString()).FirstOrDefault();
            if (Config.Instance.RememberedLogins.Contains(SelectedPlugin.Name))
            {
                var info = LoadRememberedLogin();
                if (info != null)
                {
                    LoginHandler(info);
                    return;
                }
            }

            ImageSource img = (ImageSource)_imageConverter.Convert(SelectedPlugin.Plugin.AnimeDB.loginLogo, typeof(ImageSource), null, System.Globalization.CultureInfo.CurrentCulture);
            switch (SelectedPlugin.Plugin.AnimeDB.login.loginType)
            {
                case LoginType.Default:
                    FlyoutDefaultLogin.IsOpen = true;
                    img_default.Source = img;
                    break;
                case LoginType.OAuth:
                    FlyoutOAuthLogin.IsOpen = true;
                    img_oauth.Source = img;
                    break;
            }
            img_loggingin.Source = img;
        }

        /// <summary>
        /// Attempt a login to the selected plugin.
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool login = true;
            LoginInformation loginInfo = new LoginInformation();

            // Switch between login types.
            switch (SelectedPlugin.Plugin.AnimeDB.login.loginType)
            {
                case LoginType.Default:
                    loginInfo.Default = new LoginInformation.DEFAULT();
                    loginInfo.Default.username = tb_DefaultUsername.Text;
                    loginInfo.Default.password = tb_DefaultPassword.Text;
                    break;
                case LoginType.OAuth:
                    loginInfo.OAuth = new LoginInformation.OAUTH();
                    if (tb_OAuthPin.Visibility == Visibility.Collapsed)
                    {
                        login = false;
                        tb_OAuthPin.Visibility = Visibility.Visible;
                        (sender as Button).Content = "Login";
                    }
                    else
                    {
                        loginInfo.OAuth.authorization_pin = tb_OAuthPin.Text;
                    }
                    break;
            }
            LoginHandler(loginInfo, login);
        }

        private async void LoginHandler(LoginInformation info, bool login = true)
        {
            if (login)
                FlyoutLoggingIn.IsOpen = true;
            if (LoggedIn = await Login(info))
            {
                if (Config.Instance.RememberMe && !Config.Instance.RememberedLogins.Contains(SelectedPlugin.Name))
                {
                    if (RememberMe())
                        Config.Instance.RememberedLogins.Add(SelectedPlugin.Name);
                }
                var plugins = PluginManager.Instance.Plugins.Where(p => p != SelectedPlugin && p.Plugin.Type == PluginType.AnimeDatabase);
                foreach (var p in plugins)
                    p.Unload();
                Close();
            }
            else
                FlyoutLoggingIn.IsOpen = false;
        }

        /// <summary>
        /// Attempt the login to the selected plugin.
        /// </summary>
        /// <param name="loginInfo">Information required to login.</param>
        /// <returns>true/false</returns>
        private async Task<bool> Login(LoginInformation loginInfo)
        {
            try
            {
                return await SelectedPlugin.Plugin.AnimeDB.login.Login(loginInfo);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return false;
            }
        }

        private bool RememberMe()
        {
            if (SelectedPlugin.Plugin.PluginSettings.LoginInformation == null)
                return false;
            Log.Debug($"Saving user's login information for Plugin: {SelectedPlugin.Name}");
            try
            {
                if (!Directory.Exists(Path.Combine(Config.AppDataPath, "Plugins")))
                    Directory.CreateDirectory(Path.Combine(Config.AppDataPath, "Plugins"));

                string path = Path.Combine(Config.AppDataPath, "Plugins", SelectedPlugin.Name);
                string xml = XmlManager<LoginInformation>.ToXml(SelectedPlugin.Plugin.PluginSettings.LoginInformation);
                if (string.IsNullOrEmpty(xml))
                    return false;
                string encrypted = AES.Encrpyt(xml);
                using (StreamWriter writer = new StreamWriter(path))
                    writer.WriteLine(encrypted);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return false;
            }
            return true;
        }

        private LoginInformation LoadRememberedLogin()
        {
            Log.Debug($"Loading user's login information for Plugin: {SelectedPlugin.Name}");
            try
            {
                if (!Directory.Exists(Path.Combine(Config.AppDataPath, "Plugins")))
                    return null;
                string path = Path.Combine(Config.AppDataPath, "Plugins", SelectedPlugin.Name);
                string encrypted = File.ReadAllText(path);
                if (string.IsNullOrEmpty(encrypted))
                    return null;
                string xml = AES.Decrypt(encrypted);
                return XmlManager<LoginInformation>.LoadFromString(xml);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
    }
}
