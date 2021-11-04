using System;
using CrossoutLogView.GUI.Core;

namespace CrossoutLogView.GUI.Events
{
    public delegate void OpenModelViewerEventHandler(object sender, OpenModelViewerEventArgs e);

    public sealed class OpenModelViewerEventArgs : EventArgs
    {
        public OpenModelViewerEventArgs(ViewModelBase viewModel, EventArgs innerEvent = null)
        {
            ViewModel = viewModel;
            InnerEvent = innerEvent;
        }

        public ViewModelBase ViewModel { get; }
        public EventArgs InnerEvent { get; }
    }
}