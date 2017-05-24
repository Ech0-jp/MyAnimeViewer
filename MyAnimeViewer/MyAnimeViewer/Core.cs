using MyAnimeViewer.Enums;
using MyAnimeViewer.Enums.MyAnimeList;
using MyAnimeViewer.MyAnimeList.API;
using MyAnimeViewer.Utility;
using MyAnimeViewer.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyAnimeViewer
{
    public static class Core
    {
        public static MainWindow MainWindow { get; set; }

        public static void Initialize()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var newUser = !Directory.Exists(Config.AppDataPath);
            Logger.Initialize();
            Config.Load();
            //>> UPDATE APP THEME WITH HELPER.
            var splashScreenWindow = new SplashScreenWindow();
            splashScreenWindow.ShowConditional();
            //>> ADD SOME SORT OF CLASS TO HELP TRACK ANIME LIST FROM MAL.

            MAL_LoginType loginType;
            var loggedIn = MyAnimeListAPI.LoadCredentials();
            var loginWindow = new LoginWindow();
            if (Config.Instance.ShowLoginDialog)
            {
                splashScreenWindow.Close();
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
                splashScreenWindow = new SplashScreenWindow();
                splashScreenWindow.ShowConditional();
            }
            else
                loginType = MAL_LoginType.AutoLogin;

            MainWindow = new MainWindow(loginWindow.MAL_LoginResult, loginWindow.AniList_LoginResult);
            //MainWindow.LoadConfigSettings();
            MainWindow.Show();
            splashScreenWindow.Close();
        }
    }
}
