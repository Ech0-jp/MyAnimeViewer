using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeViewer.AniList.API
{
    public class AL_AnimeList
    {
        private string m_listName;
        private List<AL_AnimeListModel> m_list;

        public string ListName { get { return m_listName; } }
        public int Count { get { return m_list.Count; } }

        public AL_AnimeList(string name)
        {
            m_listName = name;
            m_list = new List<AL_AnimeListModel>();
        }

        public void AddEntry(AL_AnimeListModel anime)
        {
            m_list.Add(anime);
        }

        public void RemoveEntry(AL_AnimeModel anime)
        { 
            
        }
    }
}
