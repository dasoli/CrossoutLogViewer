using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CrossoutLogView.Common;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.Events;
using CrossoutLogView.GUI.Helpers;
using CrossoutLogView.GUI.Models;
using CrossoutLogView.Statistics;
using NLog;

namespace CrossoutLogView.GUI.Controls
{
    /// <summary>
    ///     Interaction logic for MapsControl.xaml
    /// </summary>
    public partial class MapsControl : ILogging
    {
        public static readonly DependencyProperty MapsProperty = DependencyProperty.Register(nameof(Maps),
            typeof(ObservableCollection<MapModel>), typeof(MapsControl), new PropertyMetadata(OnMapsPropertyChanged));

        private GameFilter filter = new GameFilter(GameMode.All);
        private MapModel selectedItem;

        public MapsControl()
        {
            InitializeComponent();
            PlayerGamesDataGrid = ScrollableHeaderedControl_Scroller.Content as PlayerGamesDataGrid;
            // Hide map column
            PlayerGamesDataGrid.Columns.FirstOrDefault(x =>
                    string.Equals(x.Header as string, "Map", StringComparison.InvariantCultureIgnoreCase)).Visibility =
                Visibility.Hidden;
            // Passthought OpenViewModel event
            PlayerGamesDataGrid.OpenViewModel += (s, e) => OpenViewModel?.Invoke(s, e);
        }

        public PlayerGamesDataGrid PlayerGamesDataGrid { get; }

        /// <summary>
        ///     Gets or sets the <see cref="ObservableCollection{MapModel}" /> used to generate the selection of the
        ///     <see cref="MapsControl" />.
        /// </summary>
        public ObservableCollection<MapModel> Maps
        {
            get => GetValue(MapsProperty) as ObservableCollection<MapModel>;
            set => SetValue(MapsProperty, value);
        }

        public event OpenModelViewerEventHandler OpenViewModel;

        private static void OnMapsPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is MapsControl cntr && e.NewValue is ObservableCollection<MapModel> newValue)
            {
                newValue.Sort(new MapModelGamesPlayedDecending());
                cntr.ListBoxMaps.SelectedIndex = 0;
            }
        }

        private void ListBoxMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxMaps.SelectedItem is MapModel map)
            {
                if (selectedItem != null) map.StatDisplayMode = selectedItem.StatDisplayMode;
                UserOverview.DataContext = map;
                PlayerGamesDataGrid.ItemsSource = map.Games;
                RefreshGameFilter();
                selectedItem = map;
                MapBackgroundImage.Source = ImageHelper.GetMapImage(map.GameMap.Map.Name);
            }
        }

        private void GameListFilter_FilterChanged(object sender, ValueChangedEventArgs<GameFilter> e)
        {
            filter = e.NewValue;
            RefreshGameFilter();
        }

        private void RefreshGameFilter()
        {
            if (PlayerGamesDataGrid.ItemsSource == null) return;
            var view = CollectionViewSource.GetDefaultView(PlayerGamesDataGrid.ItemsSource);
            view.Filter = filter.Filter;
            view.Refresh();
        }

        private async void OpenUsersClick(object sender, RoutedEventArgs e)
        {
            await PlayerGamesDataGrid.OpenAllGamesUsers().ConfigureAwait(false);
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}