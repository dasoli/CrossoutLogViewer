using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace CrossoutLogView.GUI.Helpers
{
    public class BindingTrigger : INotifyPropertyChanged
    {
        public BindingTrigger()
        {
            Binding = new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(Value))
            };
        }

        public Binding Binding { get; }

        public object Value { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}