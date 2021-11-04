using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using CrossoutLogView.GUI.Models;

namespace CrossoutLogView.GUI.ValueConverters
{
    public class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Brush) || targetType == typeof(object))
            {
                if (value is PlayerGameModel playerGame)
                    return playerGame.Won ? Application.Current.Resources["TeamWon"] as Brush :
                        !playerGame.Unfinished ? Application.Current.Resources["TeamLost"] as Brush : default;
                if (value is GameModel game)
                    return game.Won ? Application.Current.Resources["TeamWon"] as Brush :
                        !game.Unfinished ? Application.Current.Resources["TeamLost"] as Brush : default;
                if (value is RoundModel round)
                    return Application.Current.Resources[round.Won ? "TeamWon" : "TeamLost"] as Brush;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}