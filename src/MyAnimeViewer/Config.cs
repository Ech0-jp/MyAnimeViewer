using MyAnimeViewer.Utility;
using MyAnimeViewer.Utility.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyAnimeViewer
{
    public class Config
    {
    #region Settings
        private static Config _config;

        public static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MyAnimeViewer";

        [DefaultValue(".")]
        public string DataDirPath = ".";

        [DefaultValue("00000000-0000-0000-0000-000000000000")]
        public string Id = Guid.Empty.ToString();

        [DefaultValue(0)]
        public int LogLevel = 0;

        [DefaultValue(default(List<string>))]
        public List<string> RememberedLogins = new List<string>();

        [DefaultValue(true)]
        public bool RememberMe = true;

        [DefaultValue(true)]
        public bool? SaveConfigInAppData;

        [DefaultValue(true)]
        public bool? SaveDataInAppData = null;

        [DefaultValue(true)]
        public bool ShowLoginDialog = true;

        [DefaultValue(true)]
        public bool ShowSplashScreen = true;

        [DefaultValue("Default")]
        public string UserInterfacePlugin;

        [DefaultValue(null)]
        public string AutoLoginPlugin;
    #endregion

    #region Properties
        public string BackupDir
        {
            get { return Path.Combine(DataDir, "Backups"); }
        }

        public string ConfigDir
        {
            get { return Instance.SaveDataInAppData == false ? string.Empty : AppDataPath + "\\"; }
        }

        public string ConfigPath
        {
            get { return Instance.ConfigDir + "config.xml"; }
        }

        public string DataDir
        {
            get { return Instance.SaveDataInAppData == false ? DataDirPath + "\\" : AppDataPath + "\\"; }
        }

        public string DownloadDir
        {
            get { return Path.Combine(DataDir, "Downloads"); }
        }

        public static Config Instance
        {
            get
            {
                if (_config == null)
                {
                    _config = new Config();
                    _config.ResetAll();
                }
                return _config;
            }
        }
    #endregion

    #region Misc
        private Config() { }

        public static void Save() => XmlManager<Config>.Save(Instance.ConfigPath, Instance);

        public static void SaveBackup(bool deleteOriginal = false)
        {
            var configPath = Instance.ConfigPath;

            if (!File.Exists(configPath))
                return;

            File.Copy(configPath, configPath + DateTime.Now.ToFileTime());

            if (deleteOriginal)
                File.Delete(configPath);
        }

        public static void Load()
        {
            var foundConfig = false;
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                var config = Path.Combine(AppDataPath, "config.xml");

                if (File.Exists("config.xml"))
                {
                    _config = XmlManager<Config>.Load("config.xml");
                    foundConfig = true;
                }
                else if (File.Exists(config))
                {
                    _config = XmlManager<Config>.Load(config);
                    foundConfig = true;
                }
                else if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))
                    Instance.SaveConfigInAppData = false;
            }
            catch (Exception e)
            {
                Log.Error(e);
                try
                {
                    if (File.Exists("config.xml"))
                        File.Move("config.xml", Helper.GetValidFilePath(AppDataPath, "config_corrupted", "xml"));
                    else if (File.Exists(AppDataPath + @"\config.xml"))
                        File.Move(AppDataPath + @"\config.xml", Helper.GetValidFilePath(AppDataPath, "config_corrupted", "xml"));
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                _config = BackupManager.TryRestore<Config>("config.xml");
            }

            if (!foundConfig)
            {
                if (Instance.ConfigDir != string.Empty)
                    Directory.CreateDirectory(Instance.ConfigDir);
                Save();
            }
            else if (Instance.SaveConfigInAppData != null)
            {
                if (Instance.SaveConfigInAppData.Value) //check if config needs to be moved
                {
                    if (File.Exists("config.xml"))
                    {
                        Directory.CreateDirectory(Instance.ConfigDir);
                        SaveBackup(true); //backup in case the file already exists
                        File.Move("config.xml", Instance.ConfigPath);
                        Log.Info("Moved config to appdata");
                    }
                }
                else if (File.Exists(AppDataPath + @"\config.xml"))
                {
                    SaveBackup(true); //backup in case the file already exists
                    File.Move(AppDataPath + @"\config.xml", Instance.ConfigPath);
                    Log.Info("Moved config to local");
                }
            }

            if (Instance.Id == Guid.Empty.ToString())
            {
                Instance.Id = Guid.NewGuid().ToString();
                Save();
            }
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

        public void Reset(string name)
        {
            var proper = GetType().GetFields().First(x => x.Name == name);
            var attr = (DefaultValueAttribute)proper.GetCustomAttributes(typeof(DefaultValueAttribute), false).First();
            proper.SetValue(this, attr.Value);
        }

        [AttributeUsage(AttributeTargets.All, Inherited = false)]
        private sealed class DefaultValueAttribute : Attribute
        {
            public object Value { get; }

            public DefaultValueAttribute(object value)
            {
                Value = value;
            }
        }
    #endregion
    }
}
