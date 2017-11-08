using MyAnimeViewerInterfaces;
using System.ComponentModel;
using System.Linq;
using MyAnimeViewerInterfaces.AnimeDB;

namespace AniList.Plugin
{
    public class Settings : PluginSettings<Settings>, IPluginSettings
    {
        [DefaultValue(null)]
        public LoginInformation LoginInformation { get; set; }

        public string BaseUrl
        {
            get { return "https://anilist.co/api/"; }
        }

        public void Reset(string name)
        {
            var proper = GetType().GetFields().First(x => x.Name == name);
            var attr = (DefaultValueAttribute)proper.GetCustomAttributes(typeof(DefaultValueAttribute), false).First();
            proper.SetValue(this, attr.Value);
        }

        public void ResetAll()
        {
            foreach (var field in GetType().GetFields())
            {
                var attr = (DefaultValueAttribute)field.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault();
                if (attr != null)
                    field.SetValue(this, attr.Value);
            }
        }
    }
}
