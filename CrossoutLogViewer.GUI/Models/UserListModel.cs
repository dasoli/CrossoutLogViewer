using System;
using System.Collections.ObjectModel;
using CrossoutLogView.GUI.Core;

namespace CrossoutLogView.GUI.Models
{
    public class UserListModel : ViewModelBase
    {
        private string _filterUserName;

        public UserListModel(ObservableCollection<UserModel> users)
        {
            Users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public string FilterUserName
        {
            get => _filterUserName;
            set => Set(ref _filterUserName, value?.TrimStart());
        }

        public ObservableCollection<UserModel> Users { get; }
    }
}