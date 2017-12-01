using MyAnimeViewer.Annotations;
using MyAnimeViewer.Errors;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System;
using System.Threading.Tasks;
using MyAnimeViewer.Plugins;
using System.Windows.Controls;
using MyAnimeViewer.Windows.UserControls;
using MyAnimeViewer.Utility.Logging;
using MyAnimeViewer.Controls;
using System.IO;
using MyAnimeViewerInterfaces.GUI;

namespace MyAnimeViewer.Windows
{
    /// <summary>
    /// This window is used mainly for "docking" purposes.
    /// Every other "window" such as the user's Anime List window will be created as a "UserControl" and docked
    /// to this window.
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public PluginWrapper primaryPlugin { get; private set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the application using the defualt/primary plugin.
        /// </summary>
        /// <param name="primaryPlugin">The primary plugin used to initialize the application.</param>
        /// <returns>Task.CompletedTask</returns>
        public Task Initialize(PluginWrapper primaryPlugin)
        {
            this.primaryPlugin = primaryPlugin;
            PluginSettings.Load(primaryPlugin.Plugin);
            Transition(InterfaceType.AnimeList);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Change the displayed content in the main window.
        /// </summary>
        /// <param name="content">Next content to display.</param>
        public void Transition(InterfaceType target)
        {
            switch (target)
            {
                case InterfaceType.AnimeList:
                    tContent.Content = AnimeListAdapter.Instance.View;
                    break;
                case InterfaceType.AnimeInformation:
                    throw new NotImplementedException();
                case InterfaceType.BrowseAnime:
                    throw new NotImplementedException();
                case InterfaceType.Simulcast:
                    throw new NotImplementedException();
                default:
                    ErrorManager.AddError("Error resolving targeted page.", "Unable to resolve target interface.", true);
                    break;
            }
        }

        #region Errors
        public ObservableCollection<Error> Errors => ErrorManager.Errors;

        public Visibility ErrorIconVisibility => ErrorManager.ErrorIconVisibility;

        public string ErrorCount => ErrorManager.Errors.Count > 1 ? $"({ErrorManager.Errors.Count})" : "";

        private void BtnErrors_OnClick(object sender, RoutedEventArgs e) => FlyoutErrors.IsOpen = !FlyoutErrors.IsOpen;

        public void ErrorsPropertyChanged()
        {
            OnPropertyChanged(nameof(Errors));
            OnPropertyChanged(nameof(ErrorIconVisibility));
            OnPropertyChanged(nameof(ErrorCount));
        }
        #endregion

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                Config.Save();
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }
    }
}
