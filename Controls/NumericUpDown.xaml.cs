using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimationManager.Controls
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "Part_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "Part_DecreaseButton", Type = typeof(RepeatButton))]
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        protected TextBox TextBox;
        protected RepeatButton IncreaseButton;
        protected RepeatButton DecreaseButton;

        private readonly RoutedUICommand _minorIncreaseValueCommand = new RoutedUICommand("MinorIncreaseValue", "MinorIncreaseValue", typeof(NumericUpDown));
        private readonly RoutedUICommand _minorDecreaseValueCommand = new RoutedUICommand("MinorDecreaseValue", "MinorDecreaseValue", typeof(NumericUpDown));
        private readonly RoutedUICommand _updateValueStringCommand = new RoutedUICommand("UpdateValueString", "UpdateValueString", typeof(NumericUpDown));

        /*static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }*/

        public NumericUpDown()
        {
            InitializeComponent();
        }

        #region MinValue
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(-1000000, new PropertyChangedCallback(OnMinValueChanged), new CoerceValueCallback(CoerceMinValue)));

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        private static void OnMinValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (NumericUpDown)obj;
            var minValue = (int)args.NewValue;
            if (minValue < control.MinValue)
            {
                control.MinValue = minValue;
            }
        }

        private static object CoerceMinValue(DependencyObject obj, object baseValue)
        {
            var minValue = (int)baseValue;
            return minValue;
        }
        #endregion

        #region MaxValue
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(1000000, new PropertyChangedCallback(OnMaxValueChanged), new CoerceValueCallback(CoerceMaxValue)));

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        private static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (NumericUpDown)obj;
            var maxValue = (int)args.NewValue;
            if (maxValue < control.MinValue)
            {
                control.MinValue = maxValue;
            }
        }

        private static object CoerceMaxValue(DependencyObject obj, object baseValue)
        {
            var maxValue = (int)baseValue;
            return maxValue;
        }
        #endregion

        #region Value
        /// <summary>
        /// Identifies the Value dependency property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueChanged), new CoerceValueCallback(CoerceValue)));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static object CoerceValue(DependencyObject obj, object baseValue)
        {
            var control = (NumericUpDown)obj;
            var newValue = Math.Max(control.MinValue, Math.Min(control.MaxValue, (int)baseValue));

            if (control.TextBox == null)
                return 0;

            // Update Text Box as value is changed
            control.TextBox.Text = newValue.ToString(CultureInfo.CurrentCulture);

            return newValue;
        }

        /// <summary>
        /// Identifies the ValueChanged routed event
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<int>), typeof(NumericUpDown));

        /// <summary>
        /// Occurs when the Value property changes
        /// </summary>
        public event RoutedPropertyChangedEventHandler<int> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<int> args)
        {
            RaiseEvent(args);
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var control = (NumericUpDown)obj;
            RoutedPropertyChangedEventArgs<int> e = new RoutedPropertyChangedEventArgs<int>(
                (int)args.OldValue, (int)args.NewValue, ValueChangedEvent);
            control.OnValueChanged(e);
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AttachToVisualTree();
            AttachCommands();
        }

        private void AttachToVisualTree()
        {
            AttachTextBox();
            AttachIncreaseButton();
            AttachDecreaseButton();
        }

        private void AttachTextBox()
        {
            TextBox textBox = PART_TextBox;
            if (textBox != null)
            {
                TextBox = textBox;
                TextBox.LostFocus += TextBoxOnLostFocus;
                TextBox.InputBindings.Add(new KeyBinding(_minorIncreaseValueCommand, new KeyGesture(Key.Up)));
                TextBox.InputBindings.Add(new KeyBinding(_minorDecreaseValueCommand, new KeyGesture(Key.Down)));
                TextBox.InputBindings.Add(new KeyBinding(_updateValueStringCommand, new KeyGesture(Key.Enter)));
            }
        }

        private void AttachCommands()
        {
            CommandBindings.Add(new CommandBinding(_minorIncreaseValueCommand, (a, b) => IncreaseValue()));
            CommandBindings.Add(new CommandBinding(_minorDecreaseValueCommand, (a, b) => DecreaseValue()));
            CommandBindings.Add(new CommandBinding(_updateValueStringCommand, (a, b) => { Value = ParseStringToInt(TextBox.Text); }));
            //CommandManager.RegisterClassInputBinding(typeof(TextBox), new KeyBinding(_updateValueStringCommand, new KeyGesture(Key.Enter)));
        }

        private void IncreaseValue()
        {
            var value = ParseStringToInt(TextBox.Text);
            value++;
            Value = value;
        }

        private void DecreaseValue()
        {
            var value = ParseStringToInt(TextBox.Text);
            value--;
            Value = value;
        }

        private void AttachIncreaseButton()
        {
            RepeatButton increaseButton = PART_IncreaseButton;
            if (increaseButton != null)
            {
                IncreaseButton = increaseButton;
                IncreaseButton.Focusable = false;
                IncreaseButton.Command = _minorIncreaseValueCommand;
                IncreaseButton.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
            }
        }

        private void AttachDecreaseButton()
        {
            RepeatButton decreaseButton = PART_DecreaseButton;
            if (decreaseButton != null)
            {
                DecreaseButton = decreaseButton;
                DecreaseButton.Focusable = false;
                DecreaseButton.Command = _minorDecreaseValueCommand;
                DecreaseButton.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
            }
        }

        private void RemoveFocus()
        {
            Focusable = true;
            Focus();
            Focusable = false;
        }

        private int ParseStringToInt(string source)
        {
            int value;
            int.TryParse(source, out value);
            return value;
        }

        private void TextBoxOnLostFocus(object sender, RoutedEventArgs args)
        {
            Value = ParseStringToInt(TextBox.Text);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs args)
        {
            Regex regex = new Regex("[^0-9]+");
            args.Handled = regex.IsMatch(args.Text);
        }
    }
}
