using MyAnimeViewer.Utility;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace MyAnimeViewer
{
    public class Config
    {
#region Settings
        private static Config m_config;

        public static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MyAnimeViewer";

        [DefaultValue(".")]
        public string DataDirPath = ".";

        [DefaultValue(0)]
        public int LogLevel = 0;

        [DefaultValue(true)]
        public bool MyAnimeListRememberLogin = false;

        [DefaultValue(false)]
        public bool AniListHasAccessToken = false;

        [DefaultValue(true)]
        public bool? SaveDataInAppData = null;

        [DefaultValue(true)]
        public bool ShowLoginDialog = true;

        [DefaultValue(true)]
        public bool ShowSplashScreen = true;
#endregion Settings

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
                if (m_config == null)
                {
                    m_config = new Config();
                    m_config.ResetAll();
                }
                return m_config;
            }
        }
    #region MyAnimeList
        public string MyAnimeList_FilePath
        {
            get { return Path.Combine(DataDir, "MyAnimeListAuth.xml"); }
        }

        public string MyAnimeList_BaseUrl
        {
            get { return "http://myanimelist.net/api/"; }
        }
    #endregion MyAnimeList

    #region AniList
        public string AniList_BaseUrl
        {
            get { return "https://anilist.co/api/"; }
        }

        public string AniList_ClientID
        {
            get { return "yuukll-jnutu"; }
        }

        public string AniList_ClientSecret
        {
            get { return "PIQ9UfMNp2V3ti3l4aEIAQAFwbPZO3"; }
        }

        public string AniList_ClientRedirectUri
        {
            get { return "MyAnimeViewer"; }
        }
    #endregion AniList
#endregion Properties

        #region Misc
        public Config()
        { 
        }

        /// <summary>
        /// Save the Config.
        /// </summary>
        public static void Save()
        {
            XmlManager<Config>.Save(Instance.ConfigPath, Instance);
        }

        /// <summary>
        /// Backup config.xml.
        /// </summary>
        public static void SaveBackup(bool deleteOriginal = false)
        {
            var configPath = Instance.ConfigPath;

            if (File.Exists(configPath))
            {
                File.Copy(configPath, configPath + DateTime.Now.ToFileTime());

                if (deleteOriginal)
                    File.Delete(configPath);
            }
        }

        /// <summary>
        /// Load the Config.
        /// </summary>
        public static void Load()
        {
            var foundConfig = false;
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                if (File.Exists("config.xml"))
                {
                    m_config = XmlManager<Config>.Load("config.xml");
                    foundConfig = true;
                }
                else if (File.Exists(AppDataPath + @"\config.xml"))
                {
                    m_config = XmlManager<Config>.Load(AppDataPath + @"\config.xml");
                    foundConfig = true;
                }
                else if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))
                    // Save locally if appdata doesn't exist (when e.g. not on C)
                    Instance.SaveDataInAppData = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.InnerException + "\n\nIf you don't know how to fix this, please delete "
                                + Instance.ConfigPath, "Error loading config.xml");
                Application.Current.Shutdown();
            }

            if (!foundConfig)
            {
                if (Instance.ConfigDir != string.Empty)
                    Directory.CreateDirectory(Instance.ConfigDir);
                Save();
            }
            else if (Instance.SaveDataInAppData != null)
            {
                if (Instance.SaveDataInAppData.Value)
                {
                    if (File.Exists("config.xml"))
                    {
                        Directory.CreateDirectory(Instance.ConfigDir);
                        SaveBackup(true); // Backup in case the file already exists.
                        File.Move("config.xml", Instance.ConfigPath);
                        Logger.WriteLine("Moved config to appdata", "Config");
                    }
                }
                else if (File.Exists(AppDataPath + @"\config.xml"))
                {
                    SaveBackup(true); // Backup in case the file already exists.
                    File.Move(AppDataPath + @"\config.xml", Instance.ConfigPath);
                    Logger.WriteLine("Moved config to local", "Config");
                }
            }
        }

        /// <summary>
        /// Reset all values in the Config.
        /// </summary>
        public void ResetAll()
        {
            foreach (var field in GetType().GetFields())
            {
                var attr = (DefaultValueAttribute)field.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault();
                if (attr != null)
                    field.SetValue(this, attr.Value);
            }
        }

        /// <summary>
        /// Reset a value to its default value.
        /// </summary>
        /// <param name="name">Parameter to reset</param>
        public void Reset(string name)
        {
            var proper = GetType().GetFields().First(x => x.Name == name);
            var attr = (DefaultValueAttribute)proper.GetCustomAttributes(typeof(DefaultValueAttribute), false).First();
            proper.SetValue(this, attr.Value);
        }

        [AttributeUsage(AttributeTargets.All, Inherited = false)]
        private sealed class DefaultValueAttribute : Attribute
        {
            // This is a positional argument
            public DefaultValueAttribute(object value)
            {
                Value = value;
            }

            public object Value { get; private set; }
        }
#endregion Misc
    }
}
