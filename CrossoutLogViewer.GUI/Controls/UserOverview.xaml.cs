using System;
using System.Linq;
using System.Windows;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.Helpers;
using CrossoutLogView.GUI.Models;
using NLog;

namespace CrossoutLogView.GUI.Controls
{
    /// <summary>
    ///     Interaction logic for UserOverview.xaml
    /// </summary>
    public partial class UserOverview : ILogging
    {
        public static readonly DependencyProperty GameStatGroupVisibilityProperty =
            DependencyProperty.Register(nameof(GameStatGroupVisibility), typeof(Visibility), typeof(UserOverview));

        public static readonly DependencyProperty DamageGroupVisibilityProperty =
            DependencyProperty.Register(nameof(DamageGroupVisibility), typeof(Visibility), typeof(UserOverview));

        public static readonly DependencyProperty StatDisplayVisibilityProperty =
            DependencyProperty.Register(nameof(StatDisplayVisibility), typeof(Visibility), typeof(UserOverview));

        public UserOverview()
        {
            InitializeComponent();
            ComboBoxDisplayMode.ItemsSource = Enum.GetValues(typeof(DisplayMode)).Cast<DisplayMode>();
            DataContextChanged += OnDataContextChanged;
        }

        public Visibility GameStatGroupVisibility { get; set; }

        public Visibility DamageGroupVisibility { get; set; }

        public Visibility StatDisplayVisibility { get; set; }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is UserModel umNew && e.OldValue is UserModel umOld)
                umNew.StatDisplayMode = umOld.StatDisplayMode;
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}