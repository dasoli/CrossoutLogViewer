using System;
using System.Linq;
using CrossoutLogView.GUI.Core;

namespace CrossoutLogView.GUI.Models
{
    public class UsersListControlModel : ViewModelBase
    {
        private string[] _filtersUserName;
        private string _userName;

        public string FilterUserName
        {
            get => _userName;
            set
            {
                var val = value?.TrimStart();
                if (string.IsNullOrWhiteSpace(val))
                    FiltersUserName = Array.Empty<string>();
                else
                    FiltersUserName = val.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim())
                        .ToArray();
                Set(ref _userName, val);
            }
        }

        public string[] FiltersUserName
        {
            get => _filtersUserName;
            set => Set(ref _filtersUserName, value);
        }
    }
}