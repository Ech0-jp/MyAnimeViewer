using MyAnimeViewerInterfaces.AnimeDB;

namespace MyAnimeViewerInterfaces
{
    public abstract class PluginSettings<T> where T : IPluginSettings, new()
    {
        static PluginSettings() { }
        private static T _instance;

        /// <summary>
        /// The instance for the Plugin's Settings.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.ResetAll();

                }
                return _instance;
            }
        }
    }

    public interface IPluginSettings
    {
        /// <summary>
        /// The login information for the plugin.
        /// Leave null if Plugin doesn't support logging into anything.
        /// </summary>
        LoginInformation LoginInformation { get; set; }

        /// <summary>
        /// Reset all of the Plugin's Settings to their default value.
        /// </summary>
        void ResetAll();

        /// <summary>
        /// Reset target Setting to its default value.
        /// </summary>
        /// <param name="name">The targeted setting.</param>
        void Reset(string name);
    }
}
