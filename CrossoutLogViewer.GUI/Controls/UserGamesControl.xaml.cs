using System.Windows;
using System.Windows.Controls;
using CrossoutLogView.Common;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.Events;
using CrossoutLogView.GUI.Helpers;
using CrossoutLogView.GUI.Models;
using MahApps.Metro.Controls;
using NLog;

namespace CrossoutLogView.GUI.Controls
{
    /// <summary>
    ///     Interaction logic for UserGamesControl.xaml
    /// </summary>
    public partial class UserGamesControl : ILogging
    {
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User),
            typeof(UserModel), typeof(UserGamesControl), new PropertyMetadata(OnUserPropertyChanged));

        public UserGamesControl()
        {
            InitializeComponent();
            var scroller = (Content as Grid).FindChild<ScrollableHeaderedControl>();
            var header = scroller.HeaderContent as Grid;
            DataGridGames = scroller.Content as PlayerGamesDataGrid;
            GameListFilter = header.FindChild<GameListFilter>();
            GamesChart = header.FindChild<Expander>().Content as PlayerGamesChart;
            GamesChart.OpenViewModel += (s, e) => OpenViewModel?.Invoke(s, e);
        }

        public PlayerGamesDataGrid DataGridGames { get; }

        public GameListFilter GameListFilter { get; }

        public PlayerGamesChart GamesChart { get; }


        /// <summary>
        ///     Gets or sets the <see cref="UserModel" /> used to generate the content of the <see cref="UserGamesControl" />.
        /// </summary>
        public UserModel User
        {
            get => GetValue(UserProperty) as UserModel;
            set => SetValue(UserProperty, value);
        }

        public event OpenModelViewerEventHandler OpenViewModel;

        private static void OnUserPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is UserGamesControl cntr && e.NewValue is UserModel newValue)
            {
                if (newValue.Participations != null)
                    newValue.Participations.Sort(new PlayerGameModelStartTimeDescending());
                cntr.DataContext = newValue;
                cntr.RefreshGamesFilter();
            }
        }

        private void RefreshGamesFilter(object sender, ValueChangedEventArgs<GameFilter> e)
        {
            RefreshGamesFilter();
        }

        private void RefreshGamesFilter()
        {
            if (User != null)
            {
                User.FilterParticipations = GameListFilter.Filter;
                GamesChart.ItemsSource = User.ParticipationsFiltered;
                DataGridGames.ItemsSource = User.ParticipationsFiltered;
            }
        }

        private void OpenUsersClick(object sender, RoutedEventArgs e)
        {
            DataGridGames.OpenAllGamesUsers();
        }

        private void MultiFlagComboBox_SelectedValueChanged(object sender, RoutedPropertyChangedEventArgs<NamedEnum> e)
        {
            GamesChart.Dimensions = (Dimensions)e.NewValue.Value;
        }

        private void UserGameControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGamesFilter();
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}