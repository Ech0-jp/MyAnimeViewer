using MyAnimeViewer.Plugins;
using MyAnimeViewerInterfaces.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyAnimeViewer.Controls
{
    public class BrowseAnimeAdapter
    {
        private BrowseAnimeAdapter _instance;
        public BrowseAnimeAdapter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BrowseAnimeAdapter();
                return _instance;
            }
        }

        private IUserInterface Interface;
        public UserControl View
        {
            get { return Interface.GetView(InterfaceType.BrowseAnime); }
        }

        private BrowseAnimeAdapter()
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
        }
    }
}
