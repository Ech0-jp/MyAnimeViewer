using MyAnimeViewer.Plugins;
using MyAnimeViewer.Utility.Database;
using MyAnimeViewer.Windows.UserControls;
using MyAnimeViewerInterfaces;
using MyAnimeViewerInterfaces.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                {
                    _instance = new AnimeListAdapter();
                }
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
                Interface = new DefaultInterface();
            }
            else
            {
                var plugin = PluginManager.Instance.Plugins.Where(p => p.Name == Config.Instance.UserInterfacePlugin).FirstOrDefault();
                if (plugin == null)
                    Interface = new DefaultInterface();
                else
                {
                    plugin.Load();
                    Interface = plugin.Plugin.UserInterface;
                }
            }
            Adapter = new DatabaseAdapter();
            Adapter.InitializeDatabase(Core.MainWindow.primaryPlugin);

            Interface.AnimeList.BindList(Adapter.CreateDataAdapter());
        }
    }
}
