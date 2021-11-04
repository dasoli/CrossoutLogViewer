using System;
using System.Windows;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.Events;
using CrossoutLogView.GUI.Models;
using CrossoutLogView.GUI.WindowsAuxilary;
using NLog;

namespace CrossoutLogView.GUI.Navigation
{
    /// <summary>
    ///     Interaction logic for UserListPage.xaml
    /// </summary>
    public partial class UserListPage : ILogging
    {
        private readonly NavigationWindow nav;
        private readonly UserListModel userListViewModel;

        public UserListPage(NavigationWindow nav, UserListModel userList)
        {
            this.nav = nav ?? throw new ArgumentNullException(nameof(nav));
            InitializeComponent();
            DataContext = userList;
            userListViewModel = userList ?? throw new ArgumentNullException(nameof(userList));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UsersListControl.ItemsSource = userListViewModel.Users;
            UsersListControl.FilterUserName = userListViewModel.FilterUserName;
        }

        private void UsersListControl_OpenViewModel(object sender, OpenModelViewerEventArgs e)
        {
            if (e.ViewModel is UserModel viewModel) nav.Navigate(new UserPage(nav, viewModel));
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}