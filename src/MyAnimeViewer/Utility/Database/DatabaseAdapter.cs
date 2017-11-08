using MyAnimeViewer.Errors;
using MyAnimeViewer.Plugins;
using MyAnimeViewer.Utility.Logging;
using MyAnimeViewerInterfaces.AnimeDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace MyAnimeViewer.Utility.Database
{
    public class DatabaseAdapter
    {
        private SQLiteConnection connection;
        private string path;
        private PluginWrapper source;

        public void InitializeDatabase(PluginWrapper source)
        {
            if (source.Plugin.AnimeDB == null)
                return;
            path = Path.Combine(PluginManager.LocalPluginDirectory.FullName, source.Name + ".sqlite");
            this.source = source;
            if (!File.Exists(path))
                CreateDatabase();
            else
                LoadDatabase();
        }

        private void CreateDatabase()
        {
            Log.Debug($"Creating Database for: {source.Name}. Path: {path}");
            try
            {
                SQLiteConnection.CreateFile(path);
                connection = new SQLiteConnection($"Data Source={path};Version=3;");
                connection.Open();

                // Create tables.
                string sql = "CREATE TABLE AnimeEntries (" +
                    "id INT, title VARCHAR(200), image_url VARCHAR(300), list_status VARCHAR(50)," +
                    "media_type VARCHAR(15), series_status VARCHAR(50), score INT, episodes_watched INT," + 
                    "total_episodes INT, rewatched INT, notes VARCHAR(500)" +
                    ")";
                var command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();

                connection.Close();
                RetrieveList();
            }
            catch (Exception e)
            {
                Log.Error($"Error creating database for {source.Name}.\n{e}");
            }
        }

        private void LoadDatabase()
        {
            Log.Debug($"Loading Database for: {source.Name}. Path: {path}");
            try
            {
                connection = new SQLiteConnection($"Data Source={path};Version=3;");
                // If Config.AlwaysRetrieveList == true 
                // Then RetrieveList()
            }
            catch (Exception e)
            {
                Log.Error($"Error loading database for {source.Name}.\n{e}");
            }
        }

        private async void RetrieveList()
        {
            Log.Info($"Retrieving user's list from {source.Name}");
            try
            {
                List<IAnimeDBUserEntry> data;

                try
                {
                    data = await source.Plugin.AnimeDB.animeList.GetUsersList();
                }
                catch (Exception e)
                {
                    ErrorManager.AddError(source.Name, $"Error retrieving list from {source.Name}", true);
                    Log.Error(e);
                    return;
                }

                connection.Open();
                foreach (var item in data)
                {
                    string sql = "INSERT INTO AnimeEntries (id, title, image_url, list_status, media_type, series_status, score, episodes_watched, total_episodes, rewatched, notes)" +
                        $"VALUES ({item.id}, '{item.title}', '{item.image_url}', '{item.list_status}', '{item.media_type}', '{item.series_status}', {item.score}, {item.episodes_watched}, {item.total_episodes}, {item.rewatched}, '{item.notes}')";
                    var command = new SQLiteCommand(sql, connection);
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                ErrorManager.AddError($"Error retrieving user list from {source.Name}", $"{e.Message}", true);
                Log.Error($"Error retrieving user list from {source.Name}.\n{e}");
            }
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return DbProviderFactories.GetFactory(connection).CreateDataAdapter();
        }

        // ********************************************************************************************
        // *********************************** POTENTIALLY IRELLIVANT *********************************
        // ********************************************************************************************
        public async Task<ObservableCollection<string>> GetLists()
        {
            if (connection == null)
                return null;
            Log.Debug($"Retrieving lists from {source.Name}");
            try
            {
                connection.Open();
                string sql = "SELECT list_status FROM AnimeEntries GROUP BY list_status";
                var command = new SQLiteCommand(sql, connection);
                var reader = await command.ExecuteReaderAsync();
                var result = new ObservableCollection<string>();
                while (reader.Read())
                    result.Add(reader["list_status"].ToString());
                return result;
            }
            catch (Exception e)
            {
                Log.Error($"Error retrieving lists from {source.Name}\n{e}");
                return null;
            }
        }

        public async Task<ObservableCollection<AnimeEntry>> GetSubLists(string list, string orderBy = "title", bool desc = true)
        {
            if (connection == null)
                return null;
            Log.Debug($"Retrieving entries of sublist {list} from {source.Name}");
            try
            {
                connection.Open();
                string sql = "SELECT * FROM AnimeEntries" +
                             $"WHERE list_status='{list}'";
                if (!string.IsNullOrEmpty(orderBy))
                    sql += $"ORDER BY {orderBy} {(desc ? "DESC" : "ASC")}";
                var command = new SQLiteCommand(sql, connection);
                var reader = await command.ExecuteReaderAsync();

                var result = new ObservableCollection<AnimeEntry>();
                DataTable data = new DataTable();
                data.Load(reader);
                foreach (DataRow row in data.Rows)
                {
                    result.Add(new AnimeEntry(row));
                }
                return result;
            }
            catch (Exception e)
            {
                Log.Error($"Error retrieving sublist of list {list} from {source.Name}.\n{e}");
                return null;
            }
        } 
    }
}
