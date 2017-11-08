using MyAnimeViewer.Utility.Logging;
using System;
using System.Data.SQLite;
using System.IO;

namespace MyAnimeViewer.Utility
{
    public class SQLite
    {
        private const string _dbName = "MAV_AnimeList.sqlite";
        private SQLite _instance;
        public SQLite Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SQLite();
                return _instance;
            }
        }

        public SQLiteConnection Connection { get; private set; }

        public void Initialize()
        {
            Log.Info("Initializing SQLite");
            if (!File.Exists(Config.Instance.DataDir + _dbName))
                CreateDataBase();
            else
                Connection = new SQLiteConnection(Config.Instance.DataDir + _dbName);
        }

        // LOOP THIS X TIMES UPON FAILURE TO ENSURE THIS CREATES !! ON THE X'TH TIME.. NOTIFY USER AND EXIT APPLICATION.
        private async void CreateDataBase()
        {
            try
            {
                Log.Info("Creating new SQLite DB...");
                SQLiteConnection.CreateFile(Config.Instance.DataDir + _dbName);
                using (Connection = new SQLiteConnection(Config.Instance.DataDir + _dbName))
                {
                    Connection.Open();
                    string sql = "CREATE TABLE animelist (id int, name VARCHAR(400), imageURL VARCHAR(450), mediaType VARCHAR(50), seriesType VARCHAR(100), score FLOAT, userStartDate DATETIME, userFinishDate DATETIME)";
                    SQLiteCommand command = new SQLiteCommand(sql, Connection);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
