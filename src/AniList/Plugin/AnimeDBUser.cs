using AniList.Enums;
using MyAnimeViewerInterfaces.AnimeDB;
using System;

namespace AniList.Plugin
{
    public class AnimeDBUser : IAnimeDBUser
    {
        public int ID { get; private set; }
        public string Username { get; private set; }

        public string ImageURL { get; private set; }
        public bool AdultContent { get; set; }
        public TitleLanguage TitleLanguage { get; set; }

        public AnimeDBUser(int id, string username, string imageurl)
        {
            ID = id;
            Username = username;
            ImageURL = imageurl;
        }
    }
}
