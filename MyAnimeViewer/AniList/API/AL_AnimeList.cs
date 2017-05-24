using MyAnimeViewer.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyAnimeViewer.AniList.API
{
    /// <summary>
    /// This class is a storage container used to store the data for each sub-list in a users list. (Eg; Completed, Watching, Plan To Watch)
    /// 
    /// @author Robert Andrew Gray
    /// @date 1/26/2017
    /// </summary>
    public class AL_AnimeList
    {
#region PrivateVars
        private string m_listName;                  // The name of the list. (Eg; Watching --OR-- Completed --OR-- Plan To Watch, etc.)
        private List<AL_AnimeListModel> m_list;     // A list of AL_AnimeListModels.
#endregion

#region Getters
        public string ListName { get { return m_listName; } }
        public string ListNameFormatted // Return the lists name in a formatted manner. (List names that consist of more than one word are generally named something like: "plan_to_watch", 
        {                               // this removes the underscores and capitalizes the first letter of each word and returns "Plan To Watch".
            get 
            {
                string temp = m_listName.Replace("_", " ");
                return Regex.Replace(temp, @"(^\w)|(\s\w)", m => m.Value.ToUpper()); 
            } 
        }
        public List<AL_AnimeListModel> AnimeList { get { return m_list; } }
#endregion

        public int Count { get { return m_list.Count; } }       // Return the total number of entries in m_list.

        /// <summary>
        /// Find an entry by its title from the list.
        /// </summary>
        /// <param name="name">The Title of the entry</param>
        /// <returns>AL_AnimeListModel</returns>
        public AL_AnimeListModel FindAnime(string name)
        {
            return m_list.FirstOrDefault(o => o.Title == name);
        }

        public AL_AnimeListModel FindAnime(int id)
        {
            return m_list.FirstOrDefault(o => o.ID == id);
        }

        /// <summary>
        /// Initialize AL_AnimeList
        /// </summary>
        /// <param name="name">The name of the list</param>
        public AL_AnimeList(string name)
        {
            Log.Debug($"Created new anime list {name}");
            m_listName = name;
            m_list = new List<AL_AnimeListModel>();
        }

        /// <summary>
        /// Add an entry to the list.
        /// </summary>
        /// <param name="anime">The AL_AnimeListModel to add to the list</param>
        public void AddEntry(AL_AnimeListModel anime)
        {
            Log.Debug($"Added anime {anime.Title} to list {m_listName}");
            m_list.Add(anime);
        }

        /// <summary>
        /// Remove an entry from the list.
        /// </summary>
        /// <param name="anime">The AL_AnimeListModel to remove from the list</param>
        public void RemoveEntry(AL_AnimeListModel anime)
        {
            Log.Debug($"Removed anime {anime.Title} to list {m_listName}");
            m_list.Remove(anime);
        }
    }
}
