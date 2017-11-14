using MyAnimeViewerInterfaces.GUI;
using System;
using System.Windows.Controls;
using System.Data.Common;
using System.Data;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;

namespace MyAnimeViewer.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for AnimeListUserControl.xaml
    /// </summary>
    public partial class AnimeListUserControl : UserControl, IAnimeListUserInterface
    {
        private DbDataAdapter _dataAdapter;
        private string _sortBy = "title";
        private string _sortDirection = "ASC";
        private string _statusFilter = "All";

        public AnimeListUserControl()
        {
            InitializeComponent();
        }

        public UserControl View { get { return this; } }

        public event EventHandler<AnimeEventArgs> OnEditAnime;
        public event EventHandler<AnimeEventArgs> OnViewAnimeInformation;
        public event EventHandler<AnimeEventArgs> OnWatchAnime;

        /// <summary>
        /// Bind the user's list to the control.
        /// </summary>
        /// <param name="dataAdapter">The DBDataAdapter sent from the AnimeListAdapter.</param>
        public void BindList(DbDataAdapter dataAdapter)
        {
            _dataAdapter = dataAdapter;
            DataSet result = new DataSet();
            string sql = "";
            string[] tables = { "Watching", "Completed", "On-Hold", "Dropped", "Plan to Watch" };

            // Create Query and specify the table's name for the UI.
            for (int i = 0; i < tables.Length; i++)
            {
                sql += $"SELECT * FROM AnimeEntries WHERE list_status='{tables[i].ToLower()}' ORDER BY {_sortBy} {_sortDirection};";
                string tablenumber = i == 0 ? "" : i.ToString();
                dataAdapter.TableMappings.Add($"Table{tablenumber}", tables[i]);
            }

            dataAdapter.SelectCommand.CommandText = sql;
            dataAdapter.Fill(result);
            lv_Main.ItemsSource = result.Tables;
        }

        private void SortList()
        {
            _dataAdapter.TableMappings.Clear();
            DataSet result = new DataSet();
            string[] tables = { "Watching", "Completed", "On-Hold", "Dropped", "Plan to Watch" };
            string sql = "";

            if (_statusFilter != "All")
            {
                tables = new string[1];
                tables[0] = _statusFilter;
            }

            for (int i = 0; i < tables.Length; i++)
            {
                sql += $"SELECT * FROM AnimeEntries WHERE list_status='{tables[i].ToLower()}' ORDER BY {_sortBy} {_sortDirection};";
                string tablenumber = i == 0 ? "" : i.ToString();
                _dataAdapter.TableMappings.Add($"Table{tablenumber}", tables[i]);
            }

            _dataAdapter.SelectCommand.CommandText = sql;
            _dataAdapter.Fill(result);
            lv_Main.ItemsSource = result.Tables;
            lv_Main.Items.Refresh();
        }

        private void SortList_OnClick(object sender, RoutedEventArgs e)
        {
            string result = (string)(sender as Button).Tag;
            if (_sortBy == result)
            {
                if (_sortDirection == "ASC")
                    _sortDirection = "DESC";
                else
                    _sortDirection = "ASC";
            }
            else
            {
                if (result == "title")
                    _sortDirection = "ASC";
                else
                    _sortDirection = "DESC";
            }
            _sortBy = result;
            SortList();
        }

        /// <summary>
        /// Bubble scroll event to lv_Main.
        /// src: https://stackoverflow.com/questions/3498686/wpf-remove-scrollviewer-from-treeview
        /// </summary>
        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled)
                return;
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = MouseWheelEvent;
            eventArg.Source = sender;
            var parent = ((Control)sender).Parent as UIElement;
            parent.RaiseEvent(eventArg);
        }
    }
}
