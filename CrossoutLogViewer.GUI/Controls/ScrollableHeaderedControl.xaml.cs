using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CrossoutLogView.GUI.Core;
using NLog;

namespace CrossoutLogView.GUI.Controls
{
    [DefaultProperty(nameof(Content))]
    /// <summary>
    /// Interaction logic for ScrollableHeaderControl.xaml
    /// </summary>
    public partial class ScrollableHeaderedControl : ILogging
    {
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register(nameof(HeaderContent), typeof(object), typeof(ScrollableHeaderedControl));

        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(ScrollableHeaderedControl));

        public ScrollableHeaderedControl()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets the header used to generate the content of the <see cref="ScrollableHeaderedControl" />.
        /// </summary>
        public object HeaderContent
        {
            get => GetValue(HeaderContentProperty);
            set => SetValue(HeaderContentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the content used to generate the content of the <see cref="ScrollableHeaderedControl" />.
        /// </summary>
        public new object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        private void ContentPresenter_Content_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var args = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            args.RoutedEvent = MouseWheelEvent;
            ScrollViewerMain.RaiseEvent(args);
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}