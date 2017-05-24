using DocumentFormat.OpenXml;
using System.Windows.Data;
using System;
using System.Globalization;
using System.Windows;

namespace MyAnimeViewer.Enums.AniList
{
    public enum AL_AnimeStatus
    {
        [EnumString("Finished Airing")]
        FinishedAiring  = 0,
        [EnumString("Currently Airing")]
        CurrentlyAiring = 1,
        [EnumString("Not Yet Aired")]
        NotYetAired     = 2,
        [EnumString("Cancelled")]
        Cancelled       = 3,
    }

    public class AL_AnimeStatusAiredToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is AL_AnimeStatus)) return DependencyProperty.UnsetValue;

            var temp = value as AL_AnimeStatus?;
            return temp == AL_AnimeStatus.Cancelled || temp == AL_AnimeStatus.NotYetAired ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class AL_AnimeStatusNotAiredToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is AL_AnimeStatus)) return DependencyProperty.UnsetValue;

            var temp = value as AL_AnimeStatus?;
            return temp == AL_AnimeStatus.Cancelled || temp == AL_AnimeStatus.NotYetAired ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public enum AL_MangaStatus
    {
        [EnumString("Finished Publishing")]
        FinishedPublishing = 0,
        [EnumString("Publishing")]
        Publishing = 1,
        [EnumString("Not Yet Published")]
        NotYetPublished = 2,
        [EnumString("Cancelled")]
        Cancelled = 3
    }
}
