using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using CrossoutLogView.Common;
using CrossoutLogView.Database.Data;
using CrossoutLogView.Database.Events;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.Events;
using CrossoutLogView.GUI.Models;
using CrossoutLogView.GUI.WindowsAuxilary;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;

namespace CrossoutLogView.GUI
{
    /// <summary>
    ///     Interaction logic for CollectedStatisticsWindow.xaml
    /// </summary>
    public partial class CollectedStatisticsWindow : MetroWindow, ILogging
    {
        private bool forceClose;
        private readonly LoadingWindow loadingWindow;

        private CollectedStatisticsWindowViewModel viewModel;

        public CollectedStatisticsWindow()
        {
            if (Settings.Current.StartupMaximized)
                WindowState = WindowState.Maximized;
            loadingWindow = new LoadingWindow
            {
                IsIndeterminate = true,
                Title = App.GetSharedResource("Loading"),
                Header = App.GetWindowResource("Stat_LoadingHeader"),
                Message = App.GetWindowResource("Stat_LoadingMessage"),
                ShowActivated = true
            };
            loadingWindow.Show();
            IsEnabled = false;
            CallbackTask.Run(App.InitializeSession, InitializedSession);
            logger.TraceResource("WinInit");
            InitializeComponent();
            logger.TraceResource("WinInitD");
        }

        private TaskFactory SyncedFactory { get; } = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

        private void InitializedSession()
        {
            this.Invoke(delegate
            {
                logger.TraceResource("ViewModelInit");
                DataContext = viewModel = new CollectedStatisticsWindowViewModel(Dispatcher);
                viewModel.Initialized += ApplyViewModel;
            });
        }

        private void UserGamesViewGames_OpenViewModel(object sender, OpenModelViewerEventArgs e)
        {
            new NavigationWindow(e.ViewModel).ShowDialog();
        }

        private void ApplyViewModel(object sender, EventArgs e)
        {
            UserGamesViewGames.DataGridGames.OpenViewModel += GamesOpenGame;

            if (UsersListControl.ItemsSource != null && UsersListControl.ItemsSource.Count > 0)
                UsersListControl.SelectedItem = UsersListControl.ItemsSource[0];

            CollectedStatisticsWindowViewModel.InvalidatedCachedData += OnInvalidateCachedData;

            Title = string.Concat(App.GetWindowResource("Stat_Title"), " (", viewModel.MeUser?.User?.Name ?? "", ")");
            HamburgerMenuControl.Focus();

            IsEnabled = true;
            loadingWindow.Close();
            logger.TraceResource("ViewModelInitD");
        }

        private void OnInvalidateCachedData(object sender, InvalidateCachedDataEventArgs e)
        {
            this.BeginInvoke(delegate
            {
                CollectionViewSource.GetDefaultView(WeaponListViewWeapons.ItemsSource).Refresh();
                CollectionViewSource.GetDefaultView(UserGamesViewGames.DataGridGames.ItemsSource).Refresh();
                CollectionViewSource.GetDefaultView(MapsView.Maps).Refresh();
            });
        }

        private void HamburgerMenuControl_ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs args)
        {
            if (args.InvokedItem == MenuGlyphItemSettings)
            {
                SyncedFactory.StartNew(new SettingsWindow().ShowDialog);
                HamburgerMenuControl.SelectedIndex = ContentTabControl.SelectedIndex;
                args.Handled = true;
            }
            else
            {
                ContentTabControl.SelectedIndex = HamburgerMenuControl.SelectedIndex;
                HamburgerMenuControl.IsPaneOpen = false;
            }
        }

        private void CloseHamburgerMenuPane(object sender, RoutedEventArgs e)
        {
            HamburgerMenuControl.IsPaneOpen = false;
        }

        private void GamesOpenGame(object sender, OpenModelViewerEventArgs e)
        {
            if (e.ViewModel is PlayerGameModel pgc)
                new NavigationWindow(pgc.Game).Show();
            else if (e.ViewModel is UserListModel ul) new NavigationWindow(ul).Show();
        }

        private void UserOpenUser(object sender, OpenModelViewerEventArgs e)
        {
            if (e.ViewModel is UserModel user) new NavigationWindow(new UserModel(user.User)).Show();
        }

        private void WeaponOpenUser(object sender, OpenModelViewerEventArgs e)
        {
            if (e.ViewModel is UserModel weapon) new NavigationWindow(weapon).Show();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (IsLoaded)
                Settings.Current.StartupMaximized = WindowState == WindowState.Maximized;
        }

        #region Confim close

        protected override void OnClosing(CancelEventArgs e)
        {
            if (e.Cancel) return;
            if (!forceClose)
            {
                e.Cancel = true;
                Dispatcher.BeginInvoke(new Action(async delegate { await ConfirmClose().ConfigureAwait(false); }));
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
                    await Dispatcher.BeginInvoke(new Action(delegate { new LauncherWindow().Show(); }));
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