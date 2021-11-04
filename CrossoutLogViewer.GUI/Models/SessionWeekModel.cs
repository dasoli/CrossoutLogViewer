using System;
using System.Collections.ObjectModel;
using CrossoutLogView.GUI.Core;

namespace CrossoutLogView.GUI.Models
{
    public class SessionWeekModel : ViewModelBase
    {
        private ObservableCollection<DateTime> _days;

        public ObservableCollection<DateTime> Days
        {
            get => _days;
            set => Set(ref _days, value);
        }
    }
}