using System;
using System.Windows;
using CrossoutLogView.GUI.Helpers;

namespace CrossoutLogView.GUI.Events
{
    public delegate void SessionClickEventHandler(object sender, SessionClickEventArgs e);

    public class SessionClickEventArgs : RoutedEventArgs
    {
        public SessionClickEventArgs(SessionTimes session, DateTime day)
        {
            Session = session;
            Day = day;
        }

        public SessionTimes Session { get; set; }
        public DateTime Day { get; set; }
    }
}