using System.Windows.Threading;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.Models;

namespace CrossoutLogView.GUI
{
    public class LiveTrackingWindowViewModel : WindowViewModelBase
    {
        private bool initialized = false;

        public LiveTrackingWindowViewModel()
        {
        }

        public LiveTrackingWindowViewModel(Dispatcher dispatcher)
        {
            WindowDispatcher = dispatcher;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            SettingsWindowViewModel.ApplyColors();
        }

        protected override void UpdateCollections()
        {
        }
    }
}