using NLog;

namespace CrossoutLogView.GUI.Core
{
    internal interface ILogging
    {
        internal Logger Logger { get; }
    }
}