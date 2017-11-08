using MyAnimeViewer.Errors;
using MyAnimeViewer.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MyAnimeViewer.Utility.Logging
{
    [DebuggerStepThrough]
    public class Log
    {
        private const int MaxLogFileAge = 2;
        private const int KeepOldLogs = 25;
        private static readonly Queue<string> LogQue = new Queue<string>();
        public static bool Initialized { get; private set; }

        internal static void Initialize()
        {
            if (Initialized)
                return;
            Trace.AutoFlush = true;
            var logDir = Path.Combine(Config.Instance.DataDir, "Logs");
            var logFile = Path.Combine(logDir, "mav_log.txt");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            else
            {
                try
                {
                    var fileInfo = new FileInfo(logFile);
                    if (fileInfo.Exists)
                    {
                        using (var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            //can access log file => no other instance of same installation running.
                        }
                        File.Move(logFile, logFile.Replace(".txt", "_" + DateTime.Now.ToUnixTime() + ".txt"));
                        foreach (var file in new DirectoryInfo(logDir).GetFiles("mav_log*")
                                                                      .Where(x => x.LastWriteTime < DateTime.Now.AddDays(-MaxLogFileAge))
                                                                      .OrderByDescending(x => x.LastWriteTime)
                                                                      .Skip(KeepOldLogs))
                        {
                            try
                            {
                                File.Delete(file.FullName);
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                        File.Create(logFile).Dispose();
                }
                catch (Exception)
                {
                    MessageBox.Show("Another instance of MyAnimeViewer is already running.", "Error starting MyAnimeViewer", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                    return;
                }
            }
            try
            {
                Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter(logFile, false)));
            }
            catch (Exception e)
            {
                ErrorManager.AddError("Can not access log file.", e.ToString());
            }
            Initialized = true;
            foreach (var line in LogQue)
                Trace.WriteLine(line);
        }

        public static void WriteLine(string msg, LogType type, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
#if (!DEBUG)
            if(type == LogType.Debug && Config.Instance.LogLevel == 0)
                return;
#endif
            var file = sourceFilePath?.Split('/', '\\').LastOrDefault()?.Split('.').FirstOrDefault();
            var line = $"{DateTime.Now.ToLongTimeString()}|{type}|{file}.{memberName} >> {msg}";
            if (Initialized)
                Trace.WriteLine(line);
            else
                LogQue.Enqueue(line);
        }

        public static void Debug(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            => WriteLine(msg, LogType.Debug, memberName, sourceFilePath);

        public static void Info(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            => WriteLine(msg, LogType.Info, memberName, sourceFilePath);

        public static void Warn(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            => WriteLine(msg, LogType.Warning, memberName, sourceFilePath);

        public static void Error(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            => WriteLine(msg, LogType.Error, memberName, sourceFilePath);

        public static void Error(Exception e, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
            => WriteLine(e.ToString(), LogType.Error, memberName, sourceFilePath);
    }
}
