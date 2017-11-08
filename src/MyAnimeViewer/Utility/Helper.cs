using Microsoft.Win32;
using MyAnimeViewer.Utility.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MyAnimeViewer.Utility
{
    public static class Helper
    {


        public static Version GetCurrentVersion() => Assembly.GetExecutingAssembly().GetName().Version;

        //See https://msdn.microsoft.com/en-us/library/hh925568(v=vs.110).aspx for value conversion
        public static int GetInstalledDotNetVersion()
        {
            try
            {
                const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
                using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                    return (int)(ndpKey?.GetValue("Release") ?? -1);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return -1;
            }
        }

        public static IEnumerable<FileInfo> GetFileInfos(string path, bool subDir)
        {
            var dirInfo = new DirectoryInfo(path);
            foreach (var fileInfo in dirInfo.GetFiles())
                yield return fileInfo;
            if (!subDir)
                yield break;
            foreach (var dir in dirInfo.GetDirectories())
            foreach (var fileInfo in dir.GetFiles())
                yield return fileInfo;   
        }

        public static string GetValidFilePath(string dir, string name, string extension)
        {
            var validDir = RemoveInvalidPathChars(dir);
            if (!Directory.Exists(validDir))
                Directory.CreateDirectory(validDir);

            if (!extension.StartsWith("."))
                extension = "." + extension;

            var path = validDir + "\\" + RemoveInvalidFileNameChars(name);
            if (File.Exists(path + extension))
            {
                var num = 1;
                while (File.Exists(path + "_" + num + extension))
                    num++;
                path += "_" + num;
            }

            return path + extension;
        }

        internal static string GetWindowsVersion()
        {
            try
            {
                var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                return reg == null ? "Unknown" : $"{reg.GetValue("ProductName")} {reg.GetValue("CurrentBuild")}";
            }
            catch (Exception e)
            {
                Log.Error(e);
                return "Unkown";
            }
        }

        public static string RemoveInvalidPathChars(string s) => RemoveChars(s, Path.GetInvalidPathChars());
        public static string RemoveInvalidFileNameChars(string s) => RemoveChars(s, Path.GetInvalidFileNameChars());
        public static string RemoveChars(string s, char[] c) => new Regex($"[{Regex.Escape(new string(c))}]").Replace(s, "");
    }
}
