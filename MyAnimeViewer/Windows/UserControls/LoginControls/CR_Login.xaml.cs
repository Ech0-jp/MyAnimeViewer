using MyAnimeViewer.Crunchyroll.API;
using MyAnimeViewer.Errors;
using MyAnimeViewer.Utility.Logging;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyAnimeViewer.Windows.UserControls.LoginControls
{
    /// <summary>
    /// Interaction logic for CR_Login.xaml
    /// </summary>
    public partial class CR_Login : UserControl
    {
        public CR_Login()
        {
            InitializeComponent();
            
            tbx_Username.Focus();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(Window.GetWindow(this) is LoginWindow))
                btn_Skip.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// TEMP SOLUTION !!!!!!!! FIX THIS WHEN CHANGING THE LOGIN WINDOW TO BE USER CONTROL COMPATIBLE.
        /// </summary>
        public void CheckLogin()
        {
            if (Config.Instance.CrunchyrollRememberLogin)
            {
                sp_LogIn.Visibility = Visibility.Collapsed;
                sp_LoggingIn.Visibility = Visibility.Visible;
                pr_LoggingIn.IsActive = true;

                Login(Properties.Settings.Default.CR_Username, Properties.Settings.Default.CR_Password, false);
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbx_Username.Text) || string.IsNullOrEmpty(tbx_Password.Password))
                return;

            sp_LogIn.Visibility = Visibility.Collapsed;
            sp_LoggingIn.Visibility = Visibility.Visible;
            pr_LoggingIn.IsActive = true;

            Login(tbx_Username.Text, tbx_Password.Password, cb_RememberLogin.IsChecked.Value, true);
        }

        private async void Login(string username, string password, bool rememberLogin, bool rememberLoginCB = false)
        {
            try
            {
                var crClient = new CrunchyrollClient();
                if (await crClient.Login(username, password))
                {

                }

                #region OLD
                //Log.Info("Attempting Crunchyroll login...");
                //var appState = new ApplicationState();
                //var clientInfo = new ClientInformation();

                //Core.CrClient = new CrunchyrollClient(appState, clientInfo);
                //Core.CrLoggedIn = await Core.CrClient.Login(username, password);

                //if (Core.CrLoggedIn)
                //{
                //    Log.Info("Successfully logged into Crunchyroll.");
                //    Core.CrClient.StartSession();
                //    if (rememberLogin)
                //    {
                //        Properties.Settings.Default.CR_Username = username;
                //        Properties.Settings.Default.CR_Password = password;
                //        Config.Instance.CrunchyrollRememberLogin = true;
                //        Config.Save();
                //    }
                //    else if (rememberLoginCB)
                //    {
                //        if (cb_RememberLogin.IsChecked == false && Config.Instance.CrunchyrollRememberLogin)
                //        {
                //            Properties.Settings.Default.CR_Username = null;
                //            Properties.Settings.Default.CR_Password = null;
                //            Config.Instance.CrunchyrollRememberLogin = false;
                //            Config.Save();
                //        }
                //    }

                //    var window = Window.GetWindow(this);
                //    if (window is LoginWindow)
                //        (window as LoginWindow).CrunchyrollLoginComplete();
                //    else
                //    {
                //        var settings = (window as MainWindow).fo_Settings.Content as Settings.Settings;
                //        settings.OptionsSitesCrunchyroll.LoggedIn();
                //    }
                //}
                //else
                //{
                //    sp_LogIn.Visibility = Visibility.Visible;
                //    tb_Invalid.Visibility = Visibility.Visible;
                //    sp_LoggingIn.Visibility = Visibility.Collapsed;
                //    pr_LoggingIn.IsActive = false;
                //}
                #endregion
            }
            catch (Exception e)
            {
                Log.Error($"A fatal error occured when logging into Crunchyroll. {e}");
                sp_LogIn.Visibility = Visibility.Visible;
                tb_Invalid.Visibility = Visibility.Visible;
                sp_LoggingIn.Visibility = Visibility.Collapsed;
                pr_LoggingIn.IsActive = false;
            }
        }

        private void btn_Skip_Click(object sender, RoutedEventArgs e)
        {
            (Window.GetWindow(this) as LoginWindow).CrunchyrollLoginComplete();
        }

        private void tbx_Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbx_Username.Text))
                EmptyUsername.Visibility = Visibility.Visible;
            else
                EmptyUsername.Visibility = Visibility.Collapsed;
        }

        private void tbx_Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbx_Password.Password))
                EmptyPassword.Visibility = Visibility.Visible;
            else
                EmptyPassword.Visibility = Visibility.Collapsed;
        }
    }
}
