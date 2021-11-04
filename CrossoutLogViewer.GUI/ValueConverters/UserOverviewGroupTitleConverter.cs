using System;
using System.Globalization;
using System.Windows.Data;
using CrossoutLogView.GUI.Helpers;

namespace CrossoutLogView.GUI.ValueConverters
{
    public class UserOverviewGroupTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) || targetType == typeof(object))
            {
                if (value is null)
                    return string.Empty;
                if (value is DisplayMode mode)
                {
                    if (parameter is null || !(parameter is string kind) || string.IsNullOrEmpty(kind))
                        throw new ArgumentException("Parameter must be a string, and cannot be null or empty,");
                    return App.GetControlResource("UserOverview_" + kind) + mode switch
                    {
                        DisplayMode.GameAvg => App.GetControlResource("UserOverview_GameAvg"),
                        DisplayMode.RoundAvg => App.GetControlResource("UserOverview_RoundAvg"),
                        _ => string.Empty
                    };
                    ;
                }
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}