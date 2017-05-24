using MyAnimeViewer.AniList.API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeViewer.Utility
{
    public class ListNameSorter : IComparer
    {
        private static Dictionary<string, int> DEFAULT = new Dictionary<string, int>
                                                         {
                                                            { "watching", 0 },
                                                            { "completed", 1 },
                                                            { "on hold", 2 },
                                                            { "dropped", 3 },
                                                            { "plan to watch", 4 }
                                                         };
        public int Compare(object x, object y)
        {
            string xValue = (x as AL_AnimeList).ListNameFormatted.ToLower();
            string yValue = (y as AL_AnimeList).ListNameFormatted.ToLower();

            if (DEFAULT[xValue] > DEFAULT[yValue])
                return 1;
            else if (DEFAULT[xValue] < DEFAULT[yValue])
                return -1;
            return 0;
        }
    }
}
