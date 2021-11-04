using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using CrossoutLogView.Common;
using CrossoutLogView.GUI.Events;
using CrossoutLogView.GUI.Models;

namespace CrossoutLogView.GUI.Controls.SessionCalendar
{
    /// <summary>
    ///     Interaction logic for SessionWeek.xaml
    /// </summary>
    public partial class SessionWeek : UserControl
    {
        public static readonly DependencyProperty StartOfWeekProperty = DependencyProperty.Register(nameof(StartOfWeek),
            typeof(DateTime), typeof(SessionWeek), new PropertyMetadata(OnStartOfWeekPropertyChanged));

        public static readonly DependencyProperty EndOfWeekProperty = DependencyProperty.Register(nameof(EndOfWeek),
            typeof(DateTime), typeof(SessionWeek), new PropertyMetadata(OnEndOfWeekPropertyChanged));

        private readonly BackgroundWorker generateButtonsWorker = new BackgroundWorker();
        private bool lockGeneration;
        private readonly SessionWeekModel viewModel;

        public SessionWeek()
        {
            InitializeComponent();
            InitializeWorkers();
            DataContext = viewModel = new SessionWeekModel();
        }


        public DateTime StartOfWeek
        {
            get => (DateTime)GetValue(StartOfWeekProperty);
            set => SetValue(StartOfWeekProperty, value);
        }

        public DateTime EndOfWeek
        {
            get => (DateTime)GetValue(EndOfWeekProperty);
            set => SetValue(EndOfWeekProperty, value);
        }

        public event SessionClickEventHandler SessionClickEvent;

        private static void OnStartOfWeekPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SessionWeek cntr && e.NewValue is DateTime)
                cntr.GenerateButtons();
        }

        private static void OnEndOfWeekPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is SessionWeek cntr && e.NewValue is DateTime)
                cntr.GenerateButtons();
        }

        public void LoadWeek(DateTime dayInWeek)
        {
            lockGeneration = true;
            StartOfWeek = dayInWeek.StartOfWeek();
            EndOfWeek = StartOfWeek.AddDays(7).AddSeconds(-1);
            lockGeneration = false;
            GenerateButtons();
        }

        private void InitializeWorkers()
        {
            generateButtonsWorker.DoWork += async delegate(object sender, DoWorkEventArgs e)
            {
                (var start, var end) = (ValueTuple<DateTime, DateTime>)e.Argument;
                var weekSpan = end - start;
                // Week cannot be longer then a 7 days
                if (weekSpan.Days > 7)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                        "The difference between the values of StartOfWeek and EndOfWeek cannot be greater then 7d23h59m59s.999. StartOfWeek: {0}, EndOfWeek {1}",
                        StartOfWeek, EndOfWeek));
                var days = new Collection<DateTime>().AddDays(start, end);
                await Dispatcher.InvokeAsync(delegate { viewModel.Days = new ObservableCollection<DateTime>(days); });
            };
        }

        private void GenerateButtons()
        {
            if (!lockGeneration && StartOfWeek != default && EndOfWeek != default)
                generateButtonsWorker.RunWorkerAsync((StartOfWeek, EndOfWeek));
        }

        private void DayButton_SessionClickEvent(object sender, SessionClickEventArgs e)
        {
            SessionClickEvent?.Invoke(sender, e);
        }
    }
}