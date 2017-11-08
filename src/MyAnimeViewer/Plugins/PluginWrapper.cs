using MyAnimeViewer.Annotations;
using MyAnimeViewer.Errors;
using MyAnimeViewer.Utility.Logging;
using MyAnimeViewerInterfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyAnimeViewer.Plugins
{
    public class PluginWrapper : INotifyPropertyChanged
    {
        private int _exceptions = 0;
        private int _unhandledExceptions = 0;
        private bool _isEnabled = false;
        private bool _loaded = false;

        public PluginWrapper()
        {
            _loaded = true;
        }

        public PluginWrapper(string fileName, IPlugin plugin)
        {
            FileName = fileName;
            Plugin = plugin;
        }

        public string FileName { get; set; }
        public IPlugin Plugin { get; set; }

        public string Name => Plugin != null ? Plugin.Name : FileName;

        public string NameAndVersion => $"{Name} {Plugin?.Version.ToString() ?? ""}";

        public string RelativeFilePath => new Uri(AppDomain.CurrentDomain.BaseDirectory).MakeRelativeUri(new Uri(FileName)).ToString();

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value)
                {
                    if (!_loaded)
                    {
                        var couldLoad = Load();
                        Log.Info($"Enabled {Name}");
                        if (!couldLoad)
                            return;
                    }
                }
                else
                {
                    if (_loaded)
                    {
                        Log.Info($"Disabled {Name}");
                        Unload();
                    }
                }
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool Load()
        {
            if (Plugin == null)
                return false;
            try
            {
                Log.Info($"Loading {Name}");
                Plugin.OnLoad();
                _loaded = true;
                _exceptions = 0;
                PluginSettings.Load(Plugin);
                //MenuItem..
            }
            catch (Exception e)
            {
                ErrorManager.AddError($"Error loading Plugin \"{Name}\"", $"Make sure you are using the latest version of the Plugin and MAV.\n\n{e}");
                Log.Error($"{Name}:\n{e}");
                return false;
            }
            return true;
        }

        public void Unload()
        {
            if (Plugin == null)
                return;
            try
            {
                Plugin.OnUnload();
            }
            catch (Exception e)
            {
                Log.Error($"{Name}:\n{e}");
                _loaded = false;
                // MenuItem = null;
            }
        }

        internal bool UnhandledException() => ++_unhandledExceptions > PluginManager.MaxExceptions / 10;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
