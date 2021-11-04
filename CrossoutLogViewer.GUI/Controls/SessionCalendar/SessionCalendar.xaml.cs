using System;
using System.Windows;
using System.Windows.Controls;
using CrossoutLogView.GUI.Events;

namespace CrossoutLogView.GUI.Controls.SessionCalendar
{
    /// <summary>
    ///     Interaction logic for SessionCalendar.xaml
    /// </summary>
    public partial class SessionCalendar : UserControl
    {
        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof(Date),
            typeof(DateTime), typeof(SessionCalendar), new PropertyMetadata(DateTime.Now, OnDatePropertyChanged));

        public SessionCalendar()
        {
            InitializeComponent();
        }

        public DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public event DateChangedEventHandler DateChanged;
        public event SessionClickEventHandler SessionClick;

        private static void OnDatePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SessionCalendar cntr && e.NewValue is DateTime newValue)
            {
                cntr.DateChanged?.Invoke(cntr, new DateChangedEventArgs((DateTime?)e.OldValue, newValue));
                cntr.SelectedMonth.LoadMonth(newValue);
            }
        }

        private void Button_PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            Date = Date.AddMonths(-1);
        }

        private void Button_NextMonth_Click(object sender, RoutedEventArgs e)
        {
            Date = Date.AddMonths(1);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedMonth.LoadMonth(Date);
        }

        private void SelectedMonth_SessionClickEvent(object sender, SessionClickEventArgs e)
        {
            SessionClick?.Invoke(sender, e);
        }
    }
}