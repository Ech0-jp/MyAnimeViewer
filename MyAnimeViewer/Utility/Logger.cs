using MyAnimeViewer.Utility.Logging;
using System;
using System.Diagnostics;

/// <summary>
/// Log Debug, Info, Warnings, and Errors to a text file located in the chosen Data Directory.
/// 
/// REFERENCE: Hearthstone Deck Tracker; log.cs
///            https://github.com/HearthSim/Hearthstone-Deck-Tracker/blob/master/Hearthstone%20Deck%20Tracker/Utility/Logging/Log.cs
/// 
/// @author Robert Andrew Gray
/// @date 1/26/2017
/// </summary>

namespace MyAnimeViewer.Utility
{
    [DebuggerStepThrough]
    [Obsolete("Use Utility.Logging.Log", true)]
    public static class Logger
    {
        [Obsolete("Use Utility.Logging.Log", true)]
        public static void WriteLine(string line, int logLevel = 0)
        {
            WriteLine(line, "", logLevel);
        }

        [Obsolete("Use Utiliy.Logging.Log", true)]
        public static void WriteLine(string line, string category, int logLevel = 0)
        {
            Log.WriteLine(line, logLevel > 0 ? LogType.Debug : LogType.Info, category);
        }
    }
}
