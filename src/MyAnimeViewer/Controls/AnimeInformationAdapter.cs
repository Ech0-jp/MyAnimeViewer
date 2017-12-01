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
    public class AnimeInformationAdapter
    {
        private static AnimeInformationAdapter _instance;
        public static AnimeInformationAdapter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AnimeInformationAdapter();
                return _instance;
            }
        }

        private IUserInterface Interface;
        public UserControl View
        {
            get { return Interface.GetView(InterfaceType.AnimeList); }
        }

        private AnimeInformationAdapter()
        {
            if (Config.Instance.UserInterfacePlugin == "Default")
                Interface = Core.DefaultInterface;
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
            Interface.AnimeInformation.OnEditAnime += AnimeInformation_OnEditAnime;
            Interface.AnimeInformation.OnWatchAnime += AnimeInformation_OnWatchAnime;
        }

        private void AnimeInformation_OnEditAnime(object sender, AnimeEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AnimeInformation_OnWatchAnime(object sender, AnimeEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
