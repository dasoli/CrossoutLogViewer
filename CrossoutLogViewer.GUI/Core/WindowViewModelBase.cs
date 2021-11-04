using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using CrossoutLogView.Database.Data;
using CrossoutLogView.Database.Events;

namespace CrossoutLogView.GUI.Core
{
    public class WindowViewModelBase : CollectionViewModelBase
    {
        private bool _colorWindowTitlebar = Settings.Current.ColorWindowTitlebar;

        public WindowViewModelBase()
        {
            Settings.SettingsPropertyChanged += Settings_SettingsPropertyChanged;
            StartUpInitialize();
        }

        public bool IsInitialized { get; private set; }

        public Dispatcher WindowDispatcher { get; internal set; }

        public bool ColorWindowTitlebar
        {
            get => _colorWindowTitlebar;
            set
            {
                Set(ref _colorWindowTitlebar, value);
                Settings.Current.ColorWindowTitlebar = value;
            }
        }

        public event EventHandler Initialized;

        private void Settings_SettingsPropertyChanged(Settings sender, SettingsChangedEventArgs e)
        {
            if (sender != null && e != null && e.Name == nameof(Settings.ColorWindowTitlebar))
            {
                _colorWindowTitlebar = (bool)e.NewValue;
                OnPropertyChanged(nameof(ColorWindowTitlebar));
            }
        }

        private async void StartUpInitialize()
        {
            if (IsInitialized) return;
            await Task.Run(OnInitialize);
            IsInitialized = true;
            Initialized?.Invoke(this, new EventArgs());
        }

        protected virtual void OnInitialize()
        {
            UpdateCollectionsSafe();
        }

        protected override void UpdateCollections()
        {
        }
    }
}