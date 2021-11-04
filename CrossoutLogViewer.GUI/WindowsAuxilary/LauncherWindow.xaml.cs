using System;
using System.Threading.Tasks;
using System.Windows;
using ControlzEx.Theming;
using CrossoutLogView.Common;
using CrossoutLogView.Database.Data;
using CrossoutLogView.GUI.Core;
using MahApps.Metro.Controls;
using NLog;

namespace CrossoutLogView.GUI.WindowsAuxilary
{
    /// <summary>
    ///     Interaction logic for LauncherWindow.xaml
    /// </summary>
    public partial class LauncherWindow : MetroWindow, ILogging
    {
        private LoadingWindow loadingWindow;

        public LauncherWindow()
        {
            logger.TraceResource("WinInit");
            InitializeComponent();
            if (!string.IsNullOrEmpty(Settings.Current.BaseColorScheme) &&
                !string.IsNullOrEmpty(Settings.Current.ColorScheme))
                App.Theme = ThemeManager.Current.ChangeTheme(Application.Current, Settings.Current.BaseColorScheme,
                    Settings.Current.ColorScheme);
            DataContext = new WindowViewModelBase();
            logger.TraceResource("WinInitD");
        }

        private void CollectedStatisticsClick(object sender, RoutedEventArgs e)
        {
            new CollectedStatisticsWindow().Show();
            Close();
        }

        private void LiveTrackingClick(object sender, RoutedEventArgs e)
        {
            new LiveTrackingWindow().Show();
            Close();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate { new SettingsWindow().ShowDialog(); }));
        }

        private void SessionReviewClick(object sender, RoutedEventArgs e)
        {
            new SessionReview().Show();
            Close();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            loadingWindow = new LoadingWindow
            {
                IsIndeterminate = true,
                Header = App.GetWindowResource("Lnch_LoadingHeader"),
                Message = App.GetWindowResource("Lnch_LoadingMessage"),
                ShowActivated = true
            };
            loadingWindow.Show();
            IsEnabled = false;
            await Task.Run(App.InitializeSession);
            IsEnabled = true;
            loadingWindow.Close();
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}