﻿using System.Windows;
using System.Windows.Controls;

namespace MyAnimeViewer.Errors
{
    /// <summary>
    /// Interaction logic for ErrorItem.xaml
    /// </summary>
    public partial class ErrorItem : UserControl
    {
        public ErrorItem()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            var error = DataContext as Error;
            if (error != null)
                ErrorManager.RemoveError(error);
        }
    }
}
