using MyAnimeViewerInterfaces.AnimeDB;
using MyAnimeViewerInterfaces.GUI;
using System;

namespace MyAnimeViewerInterfaces
{
    // TODO:
    // Add Menu-Item for the Settings of the plugin.
    // Add button text that will displayed in the options for the Plugin.
    public interface IPlugin
    {
        /// <summary>
        /// Name of the Plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the Plugin.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Author's name of the Plugin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// The type('s) of Plugin this is.
        /// </summary>
        PluginType Type { get; }

        /// <summary>
        /// The Anime Database Plugin. Return null to not add one.
        /// </summary>
        IAnimeDB AnimeDB { get; }

        /// <summary>
        /// The User Interface Plugin. Return null to not add one.
        /// </summary>
        IUserInterface UserInterface { get; }

        /// <summary>
        /// The settings for the Plugin. Return null to not add any.
        /// Return <YOUR_CLASS_NAME>.Instance
        /// </summary>
        IPluginSettings PluginSettings { get; }

        /// <summary>
        /// Version of the Plugin.
        /// </summary>
        Version Version { get; }
        
        /// <summary>
        /// Called when the Plugin is loaded (or enabled) by MyAnimeViewer.
        /// </summary>
        void OnLoad();

        /// <summary>
        /// Called when the Plugin is unloaded (or disabled) by MyAnimeViewer.
        /// </summary>
        void OnUnload();
    }
}
