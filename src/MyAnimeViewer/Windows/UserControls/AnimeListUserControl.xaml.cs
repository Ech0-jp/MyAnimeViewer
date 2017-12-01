using MyAnimeViewerInterfaces.GUI;
using System;
using System.Windows.Controls;
using System.Data.Common;
using System.Data;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace MyAnimeViewer.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for AnimeListUserControl.xaml
    /// </summary>
    public partial class AnimeListUserControl : UserControl, IAnimeListUserInterface
    {
        private DbDataAdapter _dataAdapter;
        private string _sortBy = "title";       // The header chosen to sort by (title, score, progress, series type).
        private string _sortDirection = "ASC";  // The direction of the sort (ASC, DESC).
        private string _statusFilter = "all";   // The filter for the sub lists. Determines whether to display all sub lists or an individual sub-list such as "watching".

        private DataSet _editingEntry;

        public AnimeListUserControl()
        {
            InitializeComponent();
        }

        public UserControl View { get { return this; } }

        public event EditAnime OnEditAnime;
        public event ViewAnimeInformation OnViewAnimeInformation;
        public event WatchAnime OnWatchAnime;
        
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

        /// <summary>
        /// Filter the list based on provided query data: _sortBy, _sortDirection, _statusFilter.
        /// </summary>
        private void SortList()
        {
            _dataAdapter.TableMappings.Clear();
            DataSet result = new DataSet();
            string[] tables = { "Watching", "Completed", "On-Hold", "Dropped", "Plan to Watch" };
            string sql = "";

            if (_statusFilter != "all")
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

        /// <summary>
        /// Set the filter for the list and sort.
        /// </summary>
        private void FilterList_OnClick(object sender, RoutedEventArgs e)
        {
            string result = (string)(sender as Button).Tag;
            if (_statusFilter == result)
                return;
            _statusFilter = result;
            SortList();
        }

        /// <summary>
        /// Set what to sort by and direction.
        /// </summary>
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

        /// <summary>
        /// Raise the event to view the anime information page.
        /// </summary>
        private void ViewAnime_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Tag);
            OnViewAnimeInformation(this, new AnimeEventArgs(id));
        }

        /// <summary>
        /// Raise the event to view the anime information page.
        /// </summary>
        private void tb_Title_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int id = Convert.ToInt32((sender as TextBlock).Tag);
            OnViewAnimeInformation(this, new AnimeEventArgs(id));
        }

#region EditScore
        private void EditScore_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SwapScoreView(sender, true);
            e.Handled = true;
        }

        private void EditScore_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\r")
                SwapScoreView(sender, false);
            else
                ValidateScoreText(sender as TextBox, e.Text);
            e.Handled = true;
        }

        /// <summary>
        /// Adjust the view to sumbit or edit the score for the series entry.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="focusTextBox">True: Display the TextBox. False: Display the TextBlock.</param>
        private void SwapScoreView (object sender, bool focusTextBox)
        {
            Grid grid = VisualTreeHelper.GetParent(sender as DependencyObject) as Grid;
            TextBlock textblock = grid.Children[0] as TextBlock;
            TextBox textbox = grid.Children[1] as TextBox;

            textblock.Visibility = ConvertVisibilityFromBool(!focusTextBox);
            textbox.Visibility = ConvertVisibilityFromBool(focusTextBox);

            if (focusTextBox)
            {
                textbox.Text = textblock.Text == "-" ? "" : textblock.Text;
                textbox.Focus();
                textbox.SelectionStart = 0;
                textbox.SelectionLength = textbox.Text.Length;
            }
            else
            {
                textblock.Text = string.IsNullOrEmpty(textbox.Text) ? "-" : textbox.Text;
            }
        }

        /// <summary>
        /// A cheeky "PreviewTextInput" override to garuntee the user is inputting a value between 0-10.
        /// This will also set the TextBox's text to the new valid integer.
        /// If the int is greater than 10, it will be set to 10.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="text">The new text that is to be inputted into the sender.</param>
        /// <returns>false</returns>
        private void ValidateScoreText(TextBox sender, string text)
        {
            int result = 0;
            string existingText = "";

            if (sender.SelectionLength > 0)
                existingText = sender.Text.Remove(sender.SelectionStart, sender.SelectionLength);
            else
                existingText = sender.Text;

            if (int.TryParse(existingText + text, out result))
            {
                if (result > 10)
                    result = 10;
                sender.Text = result.ToString();
                sender.SelectAll();
            }
        }
#endregion

#region EditProgress
        private void Progress_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SwapProgressView(sender, true);
            string sql = $"SELECT total_episodes FROM AnimeEntries WHERE id = '{(sender as TextBlock).Tag}'";
            _dataAdapter.SelectCommand.CommandText = sql;
            _editingEntry = new DataSet();
            _dataAdapter.Fill(_editingEntry);
            e.Handled = true;
        }

        private void Progress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\r")
                SwapProgressView(sender, false);
            else
            {
                int eps = Convert.ToInt32(_editingEntry.Tables[0].Rows[0]["total_episodes"].ToString());
                ValidateProgressText(sender as TextBox, e.Text, eps);
            }
            e.Handled = true;
        }

        private void SwapProgressView(object sender, bool focusTextBox)
        {
            StackPanel parent = VisualTreeHelper.GetParent(sender as DependencyObject) as StackPanel;
            TextBlock progress = parent.Children[0] as TextBlock;
            TextBlock total = parent.Children[1] as TextBlock;
            TextBox edit = parent.Children[2] as TextBox;

            progress.Visibility = ConvertVisibilityFromBool(!focusTextBox);
            total.Visibility = ConvertVisibilityFromBool(!focusTextBox);
            edit.Visibility = ConvertVisibilityFromBool(focusTextBox);

            if (focusTextBox)
            {
                progress.Text = progress.Text == "-" ? "" : progress.Text;
                edit.Focus();
                edit.SelectAll();
            }
            else
            {
                progress.Text = string.IsNullOrEmpty(edit.Text) ? "-" : edit.Text;
            }
        }

        private void ValidateProgressText(TextBox sender, string text, int max)
        {
            int result = 0;
            string existingText = "";

            if (sender.SelectionLength > 0)
                existingText = sender.Text.Remove(sender.SelectionStart, sender.SelectionLength);
            else
                existingText = sender.Text;

            if (int.TryParse(existingText + text, out result))
            {
                if (result > max)
                    result = max;
                sender.Text = result.ToString();
                sender.SelectionStart = sender.Text.Length;
                sender.SelectionLength = 0;
            }
        }
#endregion

        /// <summary>
        /// Converts true or false to Visibility.Visible or Visibility.Collapsed.
        /// </summary>
        /// <param name="isVisible">true || false</param>
        /// <returns>Visibility.Visible || Visibility.Collapsed</returns>
        private Visibility ConvertVisibilityFromBool(bool isVisible)
        {
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
