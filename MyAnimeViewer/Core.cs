using Awesomium.Core;
using MyAnimeViewer.Enums.MyAnimeList;
using MyAnimeViewer.Errors;
using MyAnimeViewer.MyAnimeList.API;
using MyAnimeViewer.Utility.Logging;
using MyAnimeViewer.Windows;
using System;
using System.IO;
using System.Windows;

namespace MyAnimeViewer
{
    public static class Core
    {
        public static MainWindow MainWindow { get; set; }
        public static SplashScreenWindow SplashScreenWindow { get; set; }

        public static bool Initialized { get; private set; }

        //public static CrunchyrollClient CrClient { get; set; }
        //public static bool CrLoggedIn { get; set; }

        public static WebSession Session;

        public static void Initialize()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var newUser = !Directory.Exists(Config.AppDataPath);
            Log.Initialize();
            Config.Load();
            //>> UPDATE APP THEME WITH HELPER.
            SplashScreenWindow = new SplashScreenWindow();
            //>> ADD SOME SORT OF CLASS TO HELP TRACK ANIME LIST FROM MAL.
            Login();
            Initialized = true;
        }

        private static void Login()
        { 
            MAL_LoginType loginType;
            var loggedIn = MyAnimeListAPI.LoadCredentials();
            var loginWindow = new LoginWindow();
            if (Config.Instance.ShowLoginDialog)
            {
                loginWindow.ShowDialog();
                if (loginWindow.MAL_LoginResult == MAL_LoginType.None && loginWindow.AniList_LoginResult == false)
                {
                    Application.Current.Shutdown();
                    return;
                }
                //if (loginWindow.LoginResult == MAL_LoginType.None)
                //{
                //    Application.Current.Shutdown();
                //    return;
                //}
                //loginType = loginWindow.LoginResult;
                SplashScreenWindow = new SplashScreenWindow();
                SplashScreenWindow.ShowConditional();
                InitializeMainWindow(loginWindow.MAL_LoginResult, loginWindow.AniList_LoginResult);
            }
            else
                loginType = MAL_LoginType.AutoLogin;
        }

        private static async void InitializeMainWindow(MAL_LoginType MAL_loginType, bool AL_LoginResult)
        { 
            MainWindow = new MainWindow(MAL_loginType, AL_LoginResult);
            //MainWindow.LoadConfigSettings();
            if (await MainWindow.InitList())
            {
                Core.Session = WebCore.CreateWebSession(Config.Instance.DataDir, WebPreferences.Default);
                MainWindow.Show();
                SplashScreenWindow.Close();
            }
        }
    }
}
