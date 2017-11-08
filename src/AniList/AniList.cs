using System;
using MyAnimeViewerInterfaces.AnimeDB;
using MyAnimeViewerInterfaces;
using MyAnimeViewerInterfaces.GUI;
using AniList.Plugin;

namespace AniList
{
    public class AniList : IPlugin
    {
        public IAnimeDB AnimeDB { get; private set; }

        public string Author => "MyAnimeViewer";

        public string Description => "Allows MyAnimeViewer to connect, view and edit entries on AniList.co";

        public string Name => "AniList";

        public IPluginSettings PluginSettings => Settings.Instance;

        public PluginType Type => PluginType.AnimeDatabase;

        public IUserInterface UserInterface => null;

        public Version Version => null;

        public void OnLoad()
        {
            Core.PluginController = this;
            AnimeDB = new AnimeDB();
            (AnimeDB as AnimeDB).Load();
        }

        public void OnUnload()
        {
            (AnimeDB as AnimeDB).Unload();
            AnimeDB = null;
        }
    }
}
