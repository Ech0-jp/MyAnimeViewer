using MyAnimeViewer.Utility.Logging;
using System;
using System.IO;
using System.Xml.Serialization;

namespace MyAnimeViewer.Utility
{
    public static class XmlManager<T>
    {
        public static T Load(string path)
        {
            Log.Debug("Loading file: " + path);
            T instance;
            using (TextReader reader = new StreamReader(path))
            {
                var xml = new XmlSerializer(typeof(T));
                instance = (T)xml.Deserialize(reader);
            }

            Log.Debug("File loaded: " + path);
            return instance;
        }

        public static T LoadFromString(string xmlString)
        {
            T instance;
            using (TextReader reader = new StringReader(xmlString))
            {
                var xml = new XmlSerializer(typeof(T));
                instance = (T)xml.Deserialize(reader);
            }
            return instance;
        }

        public static void Save(string path, object obj)
        {
            Log.Debug("Saving file: " + path);
            try
            {
                using (TextWriter writer = new StreamWriter(path))
                {
                    var xml = new XmlSerializer(typeof(T));
                    xml.Serialize(writer, obj);
                }
                Log.Debug("File saved: " + path);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static string ToXml(object obj)
        {
            Log.Debug($"Converting {obj.GetType().Name} to XML String.");
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    var xml = new XmlSerializer(typeof(T));
                    xml.Serialize(sw, obj);
                    return sw.ToString();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                return "";
            }
        }
    }
}
