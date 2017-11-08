using MyAnimeViewer.Utility;
using MyAnimeViewer.Utility.Logging;
using MyAnimeViewerInterfaces;
using System;
using System.IO;
using System.Reflection;

namespace MyAnimeViewer.Plugins
{
    public static class PluginSettings
    {
        private const string DefaultExtention = "_Settings.xml";
        
        public static void Load(IPlugin target)
        {
            try
            {
                Log.Info($"Loading Plugin {target.Name} Settings...");
                if (target.PluginSettings == null)
                    return;
                string path = Path.Combine(PluginManager.LocalPluginDirectory.FullName, target.Name + DefaultExtention);

                Type SettingsType = target.PluginSettings.GetType();
                var result = typeof(XmlManager<>)
                    .MakeGenericType(SettingsType)
                    .GetMethod("Load")
                    .Invoke(null, new[] { path });
                SettingsType.BaseType
                    .GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic)
                    .SetValue(target.PluginSettings, result);
            }
            catch (Exception e)
            {
                Log.Error($"Error loading Plugin {target.Name} Settings...\n{e}");
            }
        }
        
        public static void Save(IPlugin target)
        {
            try
            {
                Log.Info($"Saving Plugin {target.Name} Settings...");
                if (target.PluginSettings == null)
                    return;
                string path = Path.Combine(PluginManager.LocalPluginDirectory.FullName, target.Name + DefaultExtention);

                // Temporarily clear login information to ensure security of user's information.
                var temp = target.PluginSettings.LoginInformation;
                target.PluginSettings.LoginInformation = null;

                Type SettingsType = target.PluginSettings.GetType();
                typeof(XmlManager<>)
                    .MakeGenericType(SettingsType)
                    .GetMethod("Save")
                    .Invoke(null, new object[] { path, target.PluginSettings });

                // Return the login information to the plugin.
                target.PluginSettings.LoginInformation = temp;
            }
            catch (Exception e)
            {
                Log.Error($"Error saving Plugin {target.Name} Settings...\n{e}");
            }
        }
    }
}
