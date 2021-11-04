using System.ComponentModel;
using System.Timers;
using System.Windows;
using CrossoutLogView.Common;
using CrossoutLogView.GUI.Core;
using MahApps.Metro.Controls;
using NLog;

namespace CrossoutLogView.GUI.WindowsAuxilary
{
    /// <summary>
    ///     Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : MetroWindow, ILogging
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(LoadingWindow));

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(LoadingWindow));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(LoadingWindow));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(LoadingWindow));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(LoadingWindow));

        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register(nameof(IsIndeterminate), typeof(bool), typeof(LoadingWindow));

        private readonly Timer closeTimer = new Timer { Interval = 250, AutoReset = false };
        private bool forceClose;

        public LoadingWindow()
        {
            logger.TraceResource("WinInit");
            InitializeComponent();
            DataContext = new WindowViewModelBase();
            closeTimer.Elapsed += (s, e) => Dispatcher.Invoke(Close);
            Topmost = true;
            logger.TraceResource("WinInitD");
        }

        public string Header
        {
            get => GetValue(HeaderProperty) as string;
            set => SetValue(HeaderProperty, value);
        }

        public string Message
        {
            get => GetValue(MessageProperty) as string;
            set => SetValue(MessageProperty, value);
        }

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (e is null || forceClose)
            {
                closeTimer.Dispose();
            }
            else
            {
                closeTimer.Start();
                e.Cancel = true;
                forceClose = true;
            }
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}