using Awesomium.Core;
using System.IO;
using System.Windows;

namespace MyAnimeViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Core.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!WebCore.IsInitialized)
            {
                WebCore.Initialize(new WebConfig()
                {
                    HomeURL = "http://www.awesomium.com".ToUri(),
                    LogPath = Path.Combine(Config.Instance.DataDir, "Logs/Awesomium/log.txt"),
                    LogLevel = LogLevel.Verbose
                });
            }
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (WebCore.IsInitialized)
            {
                foreach (var item in WebCore.Sessions)
                    item.ClearCache();
                WebCore.Shutdown();
            }
            base.OnExit(e);
        }
    }
}
