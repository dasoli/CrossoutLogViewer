using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using CrossoutLogView.Common;
using CrossoutLogView.Database.Collection;
using CrossoutLogView.Database.Data;
using CrossoutLogView.Database.Events;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.WindowsAuxilary;
using CrossoutLogView.Log;
using CrossoutLogView.Statistics;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;

namespace CrossoutLogView.GUI
{
    /// <summary>
    ///     Interaction logic for LiveTracking.xaml
    /// </summary>
    public partial class LiveTrackingWindow : MetroWindow, ILogging
    {
        private Game currentGame = new Game();
        private List<ILogEntry> gameLog;
        private readonly LoadingWindow loadingWindow;

        private LiveTrackingWindowViewModel viewModel;


        public LiveTrackingWindow()
        {
            if (Settings.Current.StartupMaximized)
                WindowState = WindowState.Maximized;
            loadingWindow = new LoadingWindow
            {
                IsIndeterminate = true,
                Title = App.GetSharedResource("Loading"),
                Header = App.GetWindowResource("Live_LoadingHeader"),
                Message = App.GetWindowResource("Live_LoadingMessage"),
                ShowActivated = true
            };
            loadingWindow.Show();
            IsEnabled = false;
            CallbackTask.Run(App.InitializeSession, InitializedSession);

            logger.TraceResource("WinInit");
            InitializeComponent();
            logger.TraceResource("WinInitD");
        }

        private void InitializedSession()
        {
            this.Invoke(delegate
            {
                //TODO: call initializer of WindowViewModel
                //      subscribe to Initialized event
                logger.TraceResource("ViewModelInit");
                DataContext = viewModel = new LiveTrackingWindowViewModel(Dispatcher);
                viewModel.Initialized += ApplyViewModel;
            });
            LogUploader.LogUploadEvent += OnLogUpload;
        }

        private void ApplyViewModel(object sender, EventArgs e)
        {
            //TODO: apply data from viewmodel to window content
            IsEnabled = true;
            loadingWindow.Close();
            logger.TraceResource("ViewModelInitD");
        }

        private void OnLogUpload(object sender, LogUploadEventArgs e)
        {
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (IsLoaded)
                Settings.Current.StartupMaximized = WindowState == WindowState.Maximized;
        }

        #region Confim close

        private bool forceClose;

        protected override void OnClosing(CancelEventArgs e)
        {
            if (e.Cancel) return;
            if (!forceClose)
            {
                e.Cancel = true;
                Dispatcher.BeginInvoke(new Action(async delegate { await ConfirmClose(); }));
            }
            else
            {
                base.OnClosing(e);
            }
        }

        private async Task ConfirmClose()
        {
            var settings = new MetroDialogSettings
            {
                AnimateHide = false,
                AnimateShow = false,
                AffirmativeButtonText = "Laucher",
                NegativeButtonText = "Exit",
                FirstAuxiliaryButtonText = "Cancel",
                MaximumBodyHeight = 80,
                ColorScheme = MetroDialogOptions.ColorScheme
            };
            var result = await this.ShowMessageAsync(
                "Exit confirmation",
                "You mean to leave me, or just go back to the launcher?",
                MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                settings);
            switch (result)
            {
                case MessageDialogResult.Affirmative:
                    new LauncherWindow().Show();
                    forceClose = true;
                    break;
                case MessageDialogResult.FirstAuxiliary:
                    forceClose = false;
                    break;
                default:
                    forceClose = true;
                    break;
            }

            if (forceClose) Close();
        }

        #endregion

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}