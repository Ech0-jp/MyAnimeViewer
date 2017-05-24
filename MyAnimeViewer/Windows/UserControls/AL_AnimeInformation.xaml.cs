using MyAnimeViewer.AniList.API;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using Awesomium.Windows.Controls;
using MyAnimeViewer.Utility.Logging;
using Awesomium.Core;
using System.Windows.Controls;
using MyAnimeViewer.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace MyAnimeViewer.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for AL_AnimeInformation.xaml
    /// </summary>
    public partial class AL_AnimeInformation : INotifyPropertyChanged
    {
        // ************ NEED MAJOR PERFORMANCE TWEEKS WITH PAGE SWAPPING IN RELATIONS !!!!!!! ************

        private AL_BrowseAnime m_browseAnime;
        private List<AL_AnimeModel> m_animeStack;
        private AL_AnimeModel m_original; // The original anime model for this page. (used to delete the stack for relations.)
        
        private AL_AnimeModel m_anime;
        public AL_AnimeModel Anime
        {
            get { return m_anime; }
            set
            {
                if (m_anime != value)
                {
                    m_anime = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AL_AnimeInformation(AL_AnimeModel anime, AL_BrowseAnime browseAnime = null)
        {
            InitializeComponent();
            m_animeStack = new List<AL_AnimeModel>();
            m_browseAnime = browseAnime;
            DataContext = this;
            Anime = anime;
            m_original = Anime;
            if (Anime.Relations.Count == 0)
            {
                tb_relations.Visibility = Visibility.Collapsed;
                ic_relations.Visibility = Visibility.Collapsed;
            }
            if (Anime.Characters.Count == 0)
            {
                tb_characters.Visibility = Visibility.Collapsed;
                ic_characters.Visibility = Visibility.Collapsed;
            }
            wb_youtube.WebSession = Core.Session;
        }

        private void EditListItem_Click(object sender, RoutedEventArgs e)
        {
            AL_AnimeListModel listModel = Core.MainWindow.AniListUC.UserList.FindAnime(Anime.ID);
            if (listModel == null)
            {
                listModel = new AL_AnimeListModel(Anime.ID, Anime.TitleRomaji, Anime.MediumImageURL, Anime.Type, Anime.TotalEpisodes);
                Core.MainWindow.EditListItem(listModel, true, m_anime);
            }
            else
                Core.MainWindow.EditListItem(listModel, false);
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            if (m_animeStack.Count != 0)
            {
                Core.MainWindow.tContent.Content = null;

                Anime = m_animeStack[m_animeStack.Count - 1];
                m_animeStack.RemoveAt(m_animeStack.Count - 1);

                Thread.Sleep(50);
                
                Core.MainWindow.tContent.Content = this;
            }
            else
            {
                if (m_browseAnime != null)
                    Core.MainWindow.tContent.Content = m_browseAnime;
                else
                    Core.MainWindow.SetContentAnimeList();
                wb_youtube.Dispose();
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            if (m_browseAnime != null)
                Core.MainWindow.tContent.Content = m_browseAnime;
            else
                Core.MainWindow.SetContentAnimeList();
            wb_youtube.Dispose();
        }

        private void Canvas_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MessageBox.Show("Mouse Enter!");
        }

        // Used to force the scrollviewer to scroll even when it is ontop of the WebControl.
        private void sv_Main_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var scv = sender as ScrollViewer;
            if (scv == null) return;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta * 0.5);
            e.Handled = true;
        }

        private async void ic_relations_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Core.MainWindow.tContent.Content = null;

            Image img = UIHelper.FindChild<Image>(sender as Canvas, "img_Relation");
            string tag = img.Tag.ToString();

            if (string.IsNullOrEmpty(tag))
                return;

            AL_AnimeModel relation = await AL_AnimeModel.GetAnimePage(Convert.ToInt32(tag));
            AL_AnimeListModel relationListModel = Core.MainWindow.AniListUC.UserList.FindAnime(Convert.ToInt32(tag));

            if (relation.Equals(m_original))
                m_animeStack.Clear();
            else if (m_animeStack.Contains(relation))
            {
                m_animeStack.Remove(relation);
                m_animeStack.Add(Anime);
            }
            else
                m_animeStack.Add(Anime);
            
            Anime = relation;
            
            if (Anime.Relations.Count == 0)
            {
                tb_relations.Visibility = Visibility.Collapsed;
                ic_relations.Visibility = Visibility.Collapsed;
            }
            if (Anime.Characters.Count == 0)
            {
                tb_characters.Visibility = Visibility.Collapsed;
                ic_characters.Visibility = Visibility.Collapsed;
            }

            Thread.Sleep(100);

            Core.MainWindow.tContent.Content = this;
        }
    }
}
