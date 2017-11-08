using MyAnimeViewer.Utility;
using MyAnimeViewer.Utility.Logging;
using MyAnimeViewerInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MyAnimeViewer.Plugins
{
    /// <summary>
    /// Inspired from: https://github.com/HearthSim/Hearthstone-Deck-Tracker/blob/master/Hearthstone%20Deck%20Tracker/Plugins/PluginManager.cs
    /// </summary>
    internal class PluginManager
    {
        private const string DefaultPath = "Plugins";
        private const string TriggerTypeName = "MergedTrigger";
        private static PluginManager _instance;
        public static DirectoryInfo LocalPluginDirectory => new DirectoryInfo(DefaultPath);
        
        private PluginManager()
        {
            Plugins = new ObservableCollection<PluginWrapper>();
            try
            {
                if (!LocalPluginDirectory.Exists)
                    LocalPluginDirectory.Create();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static PluginManager Instance => _instance ?? (_instance = new PluginManager());

        public static int MaxExceptions => 100;

        public ObservableCollection<PluginWrapper> Plugins { get; }

        public void LoadPluginsFromDefaultPath() => LoadPluginsFromPath(DefaultPath, true);

        public void LoadPluginsFromPath(string pluginPath, bool checkSubDirs)
        {
            if (!Directory.Exists(pluginPath))
                return;
            if (Plugins.Any())
                UnloadPlugins();
            var files = Helper.GetFileInfos(pluginPath, checkSubDirs);
            Log.Info("Loading Plugins...");
            LoadPlugins(files);
        } 

        public void LoadPlugins(IEnumerable<FileInfo> files)
        {
            foreach(var file in files.Where(f => f.Extension.Equals(".dll")))
            {
                var plugins = GetModule(file.FullName, typeof(IPlugin));
                foreach (var p in plugins)
                    Plugins.Add(p);
            }
        }

        private IEnumerable<PluginWrapper> GetModule(string fileName, Type interfaceType)
        {
            var plugins = new List<PluginWrapper>();
            try
            {
                var assembly = Assembly.LoadFrom(fileName);
                TriggerAssembly(assembly);
                foreach (var type in assembly.GetTypes())
                {
                    try
                    {
                        if (!type.IsPublic || type.IsAbstract)
                            continue;
                        var tInterface = type.GetInterface(interfaceType.ToString(), true);
                        if (tInterface == null)
                            continue;
                        var instance = Activator.CreateInstance(type);
                        if (instance is IPlugin)
                            plugins.Add(new PluginWrapper(fileName, instance as IPlugin));
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Error loading {fileName}:\n{e}");
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error loading {fileName}:\n{e}");
            }
            return plugins;
        }

        internal void UnloadPlugins()
        {
            foreach (var plugin in Plugins)
                plugin.Unload();
            Plugins.Clear();
        }
        
        // Triggers an assembly with embedded dependencies to load its internal assembly
        // resolver by instantiating a known type (TriggerType) in the default namespace, 
        // this avoids errors when using reflection. Costura.Fody and other tools can 
        // create these kinds of merged assemblies.
        private void TriggerAssembly(Assembly assembly)
        {
            try
            {
                if (assembly.CreateInstance(TriggerTypeName) != null)
                    Log.Debug($"Created {TriggerTypeName} in {assembly.GetName().Name}", "Trigger");
            }
            catch (Exception ex)
            {
                Log.Debug($"Creating {TriggerTypeName} in {assembly.GetName().Name} errored ({ex.Message})", "Trigger");
            }
        }

        // ************ OLD ****************
        //private const string DefaultPath = "Plugins";
        //private static PluginManager _instance;

        //public List<Plugin> Plugins { get; }
        //public static DirectoryInfo LocalPluginDirectory => new DirectoryInfo(DefaultPath);
        //public static DirectoryInfo PluginDirectory => new DirectoryInfo(Path.Combine(Config.Instance.DataDir, DefaultPath));
        //public static PluginManager Instance => _instance ?? (_instance = new PluginManager());

        //private PluginManager()
        //{
        //    Plugins = new List<Plugin>();
        //    try
        //    {
        //        if (!LocalPluginDirectory.Exists)
        //            LocalPluginDirectory.Create();
        //        if (!PluginDirectory.Exists)
        //            PluginDirectory.Create();
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error(e);
        //        throw;
        //    }
        //}



        //public void LoadPlugins() => LoadPlugins(DefaultPath, true);

        //public void LoadPlugins(string pluginPath, bool checkSubDirs)
        //{
        //    try
        //    {
        //        Log.Info("Loading plugins...");
        //        if (!Directory.Exists(pluginPath))
        //        {
        //            Log.Info("Failed to retrieve plugins... Directory does not exist.");
        //            return;
        //        }
        //        var dirInfo = new DirectoryInfo(pluginPath);
        //        var files = dirInfo.GetFiles().Select(f => f.FullName).ToList();
        //        if (checkSubDirs)
        //        {
        //            foreach (var dir in dirInfo.GetDirectories())
        //                files.AddRange(dir.GetFiles().Select(f => f.FullName));
        //        }

        //        foreach (var file in files)
        //        {
        //            Assembly asm = Assembly.LoadFrom(file);
        //            Plugin plugin = new Plugin(asm.GetName().Name, asm);
        //            Plugins.Add(plugin);
        //            Log.Info($"Loaded plugin {plugin.name}");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error($"Failed to load plugins...\n{e}");
        //    }
        //}

        //public Plugin GetPlugin(string name)
        //{
        //    foreach (var item in Plugins)
        //    {
        //        if (item.name == name)
        //            return item;
        //    }
        //    return null;
        //}

        //public List<Plugin> GetAnimeDBPlugins()
        //{
        //    List<Plugin> result = new List<Plugin>();
        //    foreach (var plugin in Plugins)
        //    {
        //        if (plugin.IsAnimeDB)
        //            result.Add(plugin);
        //    }
        //    return result;
        //}
    }
}
