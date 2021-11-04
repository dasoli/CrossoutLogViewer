using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.Events;
using CrossoutLogView.GUI.Helpers;
using CrossoutLogView.GUI.Models;
using NLog;

namespace CrossoutLogView.GUI.Controls
{
    /// <summary>
    ///     Interaction logic for UserListView.xaml
    /// </summary>
    public partial class UserDataGrid : ILogging
    {
        public UserDataGrid()
        {
            InitializeComponent();
            foreach (var column in Columns)
            {
                column.CanUserSort = true;
                column.IsReadOnly = true;
            }
        }

        public event OpenModelViewerEventHandler OpenViewModel;

        private void OpenUserMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridHelper.GetSourceCellElement(e) is DataGridCell dgc && dgc.DataContext is UserModel u)
            {
                OpenViewModel?.Invoke(e.OriginalSource, new OpenModelViewerEventArgs(u, e));
                e.Handled = true;
            }
        }

        private void OpenUserClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItem is UserModel u)
            {
                OpenViewModel?.Invoke(e.OriginalSource, new OpenModelViewerEventArgs(u, e));
                e.Handled = true;
            }
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}