using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using CrossoutLogView.Common;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.Events;
using CrossoutLogView.GUI.Models;
using NLog;

namespace CrossoutLogView.GUI.Controls
{
    /// <summary>
    ///     Interaction logic for UsersListControl.xaml
    /// </summary>
    public partial class UsersListControl : ILogging
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
            typeof(ObservableCollection<UserModel>), typeof(UsersListControl),
            new PropertyMetadata(OnItemsSourcePropertyChanged));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(UserModel), typeof(UsersListControl),
                new PropertyMetadata(OnSelectedItemPropertyChanged));

        private readonly UsersListControlModel viewModel;

        public UsersListControl()
        {
            InitializeComponent();
            DataContext = viewModel = new UsersListControlModel();
            viewModel.PropertyChanged += OnPropertyChanged;
        }

        public ObservableCollection<UserModel> ItemsSource
        {
            get => GetValue(ItemsSourceProperty) as ObservableCollection<UserModel>;
            set => SetValue(ItemsSourceProperty, value);
        }

        public UserModel SelectedItem
        {
            get => GetValue(SelectedItemProperty) as UserModel;
            set => SetValue(SelectedItemProperty, value);
        }

        public string FilterUserName
        {
            get => viewModel.FilterUserName;
            set => viewModel.FilterUserName = value;
        }

        public event OpenModelViewerEventHandler OpenViewModel;

        private static void OnItemsSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is UsersListControl ulc)
            {
                if (e.OldValue != null && e.OldValue is ObservableCollection<UserModel> oldValue)
                    oldValue.CollectionChanged -= ulc.ItemsSource_CollectionChanged;
                if (e.NewValue != null && e.NewValue is ObservableCollection<UserModel> newValue)
                {
                    newValue.Sort(new UserModelParticipationCountDescending());
                    newValue.CollectionChanged += ulc.ItemsSource_CollectionChanged;
                    var userListView = (CollectionView)CollectionViewSource.GetDefaultView(newValue);
                    userListView.Filter = ulc.UserListFilter;
                }
            }
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is UsersListControl ulc)
                if (e.NewValue != null && e.NewValue is UserModel model)
                    ulc.PlayerGamesChart.ItemsSource = model.Participations;
        }

        private void UserOpenUserDoubleClick(object sender, OpenModelViewerEventArgs e)
        {
            OpenViewModel?.Invoke(this, e);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(UserListModel.FilterUserName):
                    CollectionViewSource.GetDefaultView(ItemsSource).Refresh();
                    CollectionViewSource.GetDefaultView(UserListViewUsers.ItemsSource).Refresh();
                    break;
                default:
                    return;
            }
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(UserListViewUsers.ItemsSource).Refresh();
        }

        private bool UserListFilter(object obj)
        {
            if (string.IsNullOrEmpty(viewModel.FilterUserName)) return true;
            if (!(obj is UserModel ul)) return false;
            var values = ul.Name.TrimEnd().Split(' ', '-', '_');
            for (var i = 0; i < viewModel.FiltersUserName.Length; i++)
                if (UserListFilter(values, viewModel.FiltersUserName[i]))
                    return true;
            return false;
        }

        private static bool UserListFilter(string[] values, string match)
        {
            for (var i = 0; i < values.Length; i++)
                if (values[i].Contains(match, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            return false;
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}