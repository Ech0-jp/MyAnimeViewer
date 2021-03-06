﻿using MyAnimeViewer.Plugins;
using MyAnimeViewer.Utility;
using MyAnimeViewer.Utility.Logging;
using MyAnimeViewer.Windows;
using MyAnimeViewer.Windows.UserControls;
using System;
using System.IO;
using System.Windows;

namespace MyAnimeViewer
{
    public static class Core
    {
        private static MainWindow _mainWindow;
        private static SplashScreenWindow _splashScreenWindow;
        private static DefaultInterface _defaultInterface;

        public static Version Version { get; set; }
        public static MainWindow MainWindow
        {
            get
            {
                if (_mainWindow == null)
                    _mainWindow = new MainWindow();
                return _mainWindow;
            }
        }
        public static SplashScreenWindow SplashScreen
        {
            get
            {
                if (_splashScreenWindow == null || _splashScreenWindow.IsLoaded == false)
                    _splashScreenWindow = new SplashScreenWindow();
                return _splashScreenWindow;
            }
        }
        public static DefaultInterface DefaultInterface
        {
            get
            {
                if (_defaultInterface == null)
                    _defaultInterface = new DefaultInterface();
                return _defaultInterface;
            }
        }

        public static bool Initialized { get; private set; }

        /// <summary>
        /// Initialize the core application.
        /// </summary>
        public static void Initialize()
        {
            Log.Info($"MAV: {Helper.GetCurrentVersion()}, Operating System: {Helper.GetWindowsVersion()}, .NET Framework: {Helper.GetInstalledDotNetVersion()}");
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            SplashScreen.Show();

            Config.Load();
            Log.Initialize();
            PluginManager.Instance.LoadPluginsFromDefaultPath();

            SplashScreen.Close();
            Login();
            Initialized = true;
        }

        // TODO:
        // Setup for automatic login!!
        private static async void Login()
        {
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            if (!loginWindow.LoggedIn)
            {
                Application.Current.Shutdown();
                return;
            }
            SplashScreen.ShowConditional();
            await MainWindow.Initialize(loginWindow.SelectedPlugin);
            SplashScreen.Close();
            MainWindow.Show();
        }
    }
}
