using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CrossoutLogView.GUI.Core;
using NLog;

namespace CrossoutLogView.GUI.Controls
{
    /// <summary>
    ///     Interaction logic for ColorSelector.xaml
    /// </summary>
    public partial class ColorSelector : ILogging
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
            typeof(IEnumerable), typeof(ColorSelector), new PropertyMetadata(OnItemsSourceChanged));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ColorSelector),
                new PropertyMetadata(OnSelectedItemChanged));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
            typeof(string), typeof(ColorSelector), new PropertyMetadata(OnTextChanged));

        public static readonly DependencyProperty ComboBoxWidthProperty =
            DependencyProperty.Register(nameof(ComboBoxWidth), typeof(double), typeof(ColorSelector),
                new PropertyMetadata(OnComboBoxWidthChanged));

        public ColorSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets or sets a collection used to generate the selections of the <see cref="ColorSelector" />.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => GetValue(ItemsSourceProperty) as IEnumerable;
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        ///     Gets or sets the item in the currently selected or returns null if the selection is empty.
        /// </summary>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        ///     Gets or sets the text labeling the <see cref="ColorSelector" />.
        /// </summary>
        public string Text
        {
            get => GetValue(TextProperty) as string;
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Width property of the combobox.
        /// </summary>
        public double ComboBoxWidth
        {
            get => (double)GetValue(ComboBoxWidthProperty);
            set => SetValue(ComboBoxWidthProperty, value);
        }

        private static void OnItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorSelector colorSelector && e.NewValue is IEnumerable newValue && newValue != e.OldValue)
                colorSelector.ComboBoxColors.ItemsSource = newValue;
        }

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorSelector colorSelector && e.NewValue != e.OldValue)
                colorSelector.ComboBoxColors.SelectedItem = e.NewValue;
        }

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorSelector colorSelector && e.NewValue is string newValue &&
                newValue != e.OldValue as string)
                colorSelector.TextBoxText.Text = newValue;
        }

        private static void OnComboBoxWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ColorSelector colorSelector && e.NewValue is double newValue &&
                newValue != (double)e.OldValue)
            {
                if (double.IsNaN(newValue))
                    colorSelector.SetBinding(WidthProperty,
                        new Binding("ComboBoxWidth")
                        {
                            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(UserControl), 0)
                        });
                else
                    colorSelector.ComboBoxColors.Width = newValue;
            }
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}