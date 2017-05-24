using MahApps.Metro.Controls.Dialogs;
using MyAnimeViewer.AniList.API;
using MyAnimeViewer.Enums;
using MyAnimeViewer.Enums.MyAnimeList;
using MyAnimeViewer.MyAnimeList.API;
using MyAnimeViewer.Utility;
using MyAnimeViewer.Windows.UserControls.LoginControls;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MyAnimeViewer.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MahApps.Metro.Controls.MetroWindow
    {
        private readonly bool m_initialized;
        private ProgressDialogController m_controller;
        private MAL_LoginType m_MAL_LoginResult = MAL_LoginType.None;
        private bool m_AniList_LoginResult = false;

        public MAL_LoginType MAL_LoginResult { get { return m_MAL_LoginResult; } }
        public bool AniList_LoginResult { get { return m_AniList_LoginResult; } }

        public LoginWindow()
        {
            InitializeComponent();
            #region OLD
            //cb_RememberLogin.IsChecked = Config.Instance.MyAnimeListRememberLogin;
            //if (Config.Instance.MyAnimeListRememberLogin)
            //{
            //    tb_Email.Text = Properties.Settings.Default.Username;
            //    tb_Password.Password = Properties.Settings.Default.Password;
            //}
            //m_initialized = true;
            #endregion
        }

        private void AniList_Click(object sender, RoutedEventArgs e)
        {
            if (Config.Instance.AniListHasAccessToken)
                btn_AniList_Authorize.Content = "Login";
            fo_AniList.IsOpen = true;
        }

        private void MyAnimeList_Click(object sender, RoutedEventArgs e)
        {
            fo_MyAnimeList.IsOpen = true;
        }

        private async void btn_AniList_Authorize_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "There was an error requesting access to AniList. Please try again. If the problem persists, restart the application and try again.";

            if (Config.Instance.AniListHasAccessToken)
            {
                sp_ALLogin.Visibility = Visibility.Collapsed;
                sp_ALLoggingIn.Visibility = Visibility.Visible;
                pr_LoggingIn.IsActive = true;
                bool result = await AL_Authentication.RefreshAccessToken(Properties.Settings.Default.AL_RefreshToken);
                if (result)
                {
                    m_AniList_LoginResult = true;
                    fo_AniList.IsOpen = false;
                    if (Config.Instance.UseCrunchyrollService)
                        CrunchyrollLogin();
                    else
                        Close();
                }
                else
                {
                    sp_ALLogin.Visibility = Visibility.Visible;
                    sp_ALLoggingIn.Visibility = Visibility.Collapsed;
                    pr_LoggingIn.IsActive = false;
                    Config.Instance.AniListHasAccessToken = false;
                    Config.Save();
                    AL_Authentication.RemoveRefreshToken();
                    await this.ShowMessageAsync("Error", errorMessage);
                }
            }
            else if (tb_AuthorizationPin.Visibility == Visibility.Collapsed)
            {
                tb_AuthorizationPin.Visibility = Visibility.Visible;
                AL_Authentication.RequestAuthorizationPin();
            }
            else if (tb_AuthorizationPin.Visibility == Visibility.Visible && string.IsNullOrWhiteSpace(tb_AuthorizationPin.Text))
            {
                await this.ShowMessageAsync("Error", "Please copy and paste your Authorization Pin inside the textbox to proceed!");
                sp_ALLogin.Visibility = Visibility.Visible;
                sp_ALLoggingIn.Visibility = Visibility.Collapsed;
                pr_LoggingIn.IsActive = false;
            }
            else
            {
                sp_ALLogin.Visibility = Visibility.Collapsed;
                sp_ALLoggingIn.Visibility = Visibility.Visible;
                pr_LoggingIn.IsActive = true;
                bool result = await AL_Authentication.RequestAccessToken(tb_AuthorizationPin.Text);

                if (result)
                {
                    m_AniList_LoginResult = true;
                    Config.Instance.AniListHasAccessToken = true;
                    Config.Save();
                    AL_Authentication.SaveRefreshToken();
                    fo_AniList.IsOpen = false;
                    if (Config.Instance.UseCrunchyrollService)
                        CrunchyrollLogin();
                    else
                        Close();
                }
                else
                {
                    await this.ShowMessageAsync("Error", errorMessage);
                    sp_ALLogin.Visibility = Visibility.Visible;
                    sp_ALLoggingIn.Visibility = Visibility.Collapsed;
                    pr_LoggingIn.IsActive = false;
                }
            }
        }

        private void CrunchyrollLogin()
        {
            fo_Crunchyroll.IsOpen = true;
            (fo_Crunchyroll.Content as CR_Login).CheckLogin();
        }

        public void CrunchyrollLoginComplete()
        {
            fo_Crunchyroll.IsOpen = false;
            this.Close();
        }

        #region OLD
        //public MAL_LoginType LoginResult
        //{
        //    get { return m_loginResult; }
        //    private set { m_loginResult = value; }
        //}

        //private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        //{
        //    Process.Start(e.Uri.AbsoluteUri);
        //}

        //private async void btn_Login_Click(object sender, RoutedEventArgs e)
        //{
        //    var email = tb_Email.Text;
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        DisplayLoginError("Please enter a valid email address.");
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(tb_Password.Password))
        //    {
        //        DisplayLoginError("Please enter a password.");
        //        return;
        //    }
        //    IsEnabled = false;
        //    fo_LoggingIn.IsOpen = true;
        //    var result = await MyAnimeListAPI.LoginAsync(email, tb_Password.Password);
        //    if (result.Success)
        //    {
        //        if (Config.Instance.MyAnimeListRememberLogin)
        //        {
        //            Properties.Settings.Default.Username = tb_Email.Text;
        //            Properties.Settings.Default.Password = tb_Password.Password;
        //            Properties.Settings.Default.Save();
        //        }

        //        LoginResult = MAL_LoginType.Login;
        //        Close();
        //    }
        //    else if (result.Message.Contains("401"))
        //        DisplayLoginError("Invalid email or password");
        //    else
        //        DisplayLoginError(result.Message);

        //    fo_LoggingIn.IsOpen = false;
        //    tb_Password.Clear();
        //}

        //private async void DisplayLoginError(string error)
        //{
        //    tb_ErrorMessage.Text = error;
        //    tb_ErrorMessage.Visibility = Visibility.Visible;
        //    IsEnabled = true;
        //    if (m_controller != null)
        //    {
        //        if (m_controller.IsOpen)
        //            await m_controller.CloseAsync();
        //    }
        //}

        //private void cb_RememberLogin_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (!m_initialized)
        //        return;
        //    Config.Instance.MyAnimeListRememberLogin = true;
        //    Config.Save();
        //}

        //private void cb_RememberLogin_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    if (!m_initialized)
        //        return;
        //    Config.Instance.MyAnimeListRememberLogin = false;
        //    Config.Save();

        //    Properties.Settings.Default.Username = null;
        //    Properties.Settings.Default.Password = null;
        //    Properties.Settings.Default.Save();

        //    try
        //    {
        //        if (File.Exists(Config.Instance.MyAnimeList_FilePath))
        //            File.Delete(Config.Instance.MyAnimeList_FilePath);
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.WriteLine("Error deleting MyAnimeList credentials file\n" + ex, "MyAnimeListAPI");
        //    }
        //}
        #endregion
    }
}
