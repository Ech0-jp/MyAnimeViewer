using System;

namespace MyAnimeViewerInterfaces.GUI
{
    public class AnimeEventArgs : EventArgs
    {
        public int id;
        public string name;

        public AnimeEventArgs()
        {
            id = 0;
            name = "";
        }

        public AnimeEventArgs(int id)
        {
            this.id = id;
            name = "";
        }

        public AnimeEventArgs(string name)
        {
            id = 0;
            this.name = name;
        }

        public AnimeEventArgs(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
