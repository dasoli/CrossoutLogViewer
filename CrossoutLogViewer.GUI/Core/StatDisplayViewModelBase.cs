using CrossoutLogView.GUI.Events;
using CrossoutLogView.GUI.Helpers;

namespace CrossoutLogView.GUI.Core
{
    public class StatDisplayViewModeBase : CollectionViewModelBase
    {
        private static bool lockUpdate;
        private DisplayMode _statDisplayMode = DisplayMode.GameAvg;

        public StatDisplayViewModeBase()
        {
            StatDisplayModeChanged += OnStatDisplayModeChanged;
        }

        public DisplayMode StatDisplayMode
        {
            get => _statDisplayMode;
            set
            {
                var oldValue = _statDisplayMode;
                Set(ref _statDisplayMode, value);
                if (!lockUpdate)
                {
                    lockUpdate = true;
                    StatDisplayModeChanged?.Invoke(this, new ValueChangedEventArgs<DisplayMode>(oldValue, value));
                    lockUpdate = false;
                }

                UpdateProperties();
            }
        }

        private static event ValueChangedEventHandler<DisplayMode> StatDisplayModeChanged;

        private void OnStatDisplayModeChanged(object sender, ValueChangedEventArgs<DisplayMode> e)
        {
            StatDisplayMode = e.NewValue;
        }

        protected override void UpdateCollections()
        {
        }

        public virtual void UpdateProperties()
        {
        }
    }
}