﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CrossoutLogView.Database.Data;
using CrossoutLogView.GUI.Core;
using NLog;

namespace CrossoutLogView.GUI.Controls
{
    /// <summary>
    ///     Interaction logic for MultiFlagComboBox.xaml
    /// </summary>
    public partial class MultiFlagComboBox : ILogging
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(MultiFlagComboBox));

        protected static readonly DependencyPropertyKey ItemsSourcePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ItemsSource), typeof(ReadOnlyObservableCollection<NamedEnum>),
                typeof(MultiFlagComboBox), new PropertyMetadata(ItemsSourcePropertyChanged));

        public static readonly DependencyProperty ItemsSourceProperty = ItemsSourcePropertyKey.DependencyProperty;

        protected static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(NamedEnum), typeof(MultiFlagComboBox),
                new PropertyMetadata(SelectedValuePropertyChanged));

        private bool lockCheckedValueUpdate;

        public MultiFlagComboBox()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => GetValue(TitleProperty) as string;
            set => SetValue(TitleProperty, value);
        }

        public Type ItemsType { get; protected set; }

        public ReadOnlyObservableCollection<NamedEnum> ItemsSource
        {
            get => GetValue(ItemsSourceProperty) as ReadOnlyObservableCollection<NamedEnum>;
            protected set => SetValue(ItemsSourcePropertyKey, value);
        }

        public NamedEnum SelectedItem
        {
            get => GetValue(SelectedItemProperty) as NamedEnum;
            set => SetValue(SelectedItemProperty, value);
        }

        public object SelectedValue
        {
            get => SelectedItem.Value;
            set => SelectedItem = new NamedEnum(value, "Selected", true);
        }

        public event RoutedPropertyChangedEventHandler<NamedEnum> SelectedValueChanged;

        public void LoadEnumValues<T>() where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new ArgumentException("T must be an enumerated type.", nameof(T));
            ItemsType = type;
            ItemsSource =
                new ReadOnlyObservableCollection<NamedEnum>(
                    new ObservableCollection<NamedEnum>(NamedEnum.FromType<T>()));
            SelectedItem = new NamedEnum(Settings.Current.Dimensions, "Selected Value", true);
        }

        private void CheckValue(NamedEnum namedEnum)
        {
            var checkedValue = (int)namedEnum.Value;
            var currentValue = (int)SelectedItem.Value;
            currentValue |= checkedValue;
            SelectedValue = Enum.Parse(ItemsType, currentValue.ToString());
        }

        private void UncheckValue(NamedEnum namedEnum)
        {
            var checkedValue = (int)namedEnum.Value;
            var currentValue = (int)SelectedItem.Value;
            currentValue &= ~checkedValue;
            SelectedValue = Enum.Parse(ItemsType, currentValue.ToString());
        }

        private void ApplyValue(NamedEnum namedEnum)
        {
            if (namedEnum == null) return;
            var mask = (int)namedEnum.Value;
            for (var i = 0; i < ItemsSource.Count; i++)
            {
                var value = (int)ItemsSource[i].Value;
                ItemsSource[i].IsChecked = (value & mask) == value;
            }
        }

        private void InvalidateSelectedValue()
        {
            lockCheckedValueUpdate = true;
            ApplyValue(SelectedItem);
            lockCheckedValueUpdate = false;
        }

        private void Expander_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(Expander);
            if (pos.X <= Expander.ActualWidth && pos.Y <= Expander.ActualHeight)
                if (Expander.IsExpanded)
                {
                    Expander.IsExpanded = false;
                    e.Handled = true;
                }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!lockCheckedValueUpdate && sender is FrameworkElement fe && fe.DataContext is NamedEnum ne)
            {
                lockCheckedValueUpdate = true;
                if (ne.IsChecked)
                    CheckValue(ne);
                else
                    UncheckValue(ne);
                lockCheckedValueUpdate = false;
            }
        }

        private static void SelectedValuePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is MultiFlagComboBox cmb)
            {
                cmb.InvalidateSelectedValue();
                cmb.SelectedValueChanged?.Invoke(cmb,
                    new RoutedPropertyChangedEventArgs<NamedEnum>(e.OldValue as NamedEnum, e.NewValue as NamedEnum));
            }
        }

        private static void ItemsSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is MultiFlagComboBox cmb) cmb.InvalidateSelectedValue();
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }

    public class NamedEnum : ViewModelBase
    {
        private bool _isChecked;

        public NamedEnum([NotNull] object value, string name, bool isChecked = false)
        {
            Value = value;
            Name = name;
            IsChecked = isChecked;
        }

        [NotNull] public object Value { get; }

        public string Name { get; }

        public bool IsChecked
        {
            get => _isChecked;
            set => Set(ref _isChecked, value);
        }

        public static NamedEnum[] FromType<T>() where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new ArgumentException("T must be an enumerated type.", nameof(T));
            var values = Enum.GetValues(type).Cast<T>().ToArray();
            var names = Enum.GetNames(type);
            var result = new NamedEnum[values.Length];
            for (var i = 0; i < values.Length; i++) result[i] = new NamedEnum(values[i], names[i]);
            return result;
        }
    }
}