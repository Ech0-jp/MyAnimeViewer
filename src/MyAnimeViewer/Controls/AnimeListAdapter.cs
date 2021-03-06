﻿using MyAnimeViewer.Plugins;
using MyAnimeViewer.Utility.Database;
using MyAnimeViewer.Windows.UserControls;
using MyAnimeViewerInterfaces.GUI;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyAnimeViewer.Controls
{
    public class AnimeListAdapter
    {
        private static AnimeListAdapter _instance;
        public static AnimeListAdapter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AnimeListAdapter();
                return _instance;
            }
        }

        public DatabaseAdapter Adapter { get; private set; }
        private IUserInterface Interface;
        public UserControl View
        {
            get { return Interface.GetView(InterfaceType.AnimeList); }
        }

        private AnimeListAdapter()
        {
            if (Config.Instance.UserInterfacePlugin == "Default")
            {
                Interface = Core.DefaultInterface;
            }
            else
            {
                var plugin = PluginManager.Instance.Plugins.Where(p => p.Name == Config.Instance.UserInterfacePlugin).FirstOrDefault();
                if (plugin == null)
                    Interface = Core.DefaultInterface;
                else
                {
                    if (!plugin.IsEnabled) plugin.Load();
                    Interface = plugin.Plugin.UserInterface;
                }
            }
            Adapter = new DatabaseAdapter();
            Adapter.InitializeDatabase(Core.MainWindow.primaryPlugin);

            Interface.AnimeList.OnEditAnime += AnimeList_OnEditAnime;
            Interface.AnimeList.OnViewAnimeInformation += AnimeList_OnViewAnimeInformation;
            Interface.AnimeList.OnWatchAnime += AnimeList_OnWatchAnime;
            Interface.AnimeList.BindList(Adapter.CreateDataAdapter());
        }

        private void AnimeList_OnEditAnime(object sender, AnimeEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void AnimeList_OnViewAnimeInformation(object sender, AnimeEventArgs e)
        {
            MessageBox.Show(e.id.ToString());
        }

        private void AnimeList_OnWatchAnime(object sender, AnimeEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
