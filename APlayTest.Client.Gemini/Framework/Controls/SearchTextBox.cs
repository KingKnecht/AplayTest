using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace APlayTest.Client.Gemini.Framework.Controls
{
    public class SearchTextBox : TextBox
    {
        private string _originalLabelText;


        public static DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(SearchTextBox));

        public static DependencyProperty LabelTextColorProperty =
            DependencyProperty.Register(
                "LabelTextColor",
                typeof(Brush),
                typeof(SearchTextBox));

        private static DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "HasText",
                typeof(bool),
                typeof(SearchTextBox),
                new PropertyMetadata());
        public static DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        public static readonly DependencyProperty SearchOptionsPopupProperty = DependencyProperty.Register(
            "SearchOptionsPopup", typeof(Popup), typeof(SearchTextBox), new PropertyMetadata(default(Popup)));

        public Popup SearchOptionsPopup
        {
            get { return (Popup)GetValue(SearchOptionsPopupProperty); }
            set { SetValue(SearchOptionsPopupProperty, value); }
        }

        static SearchTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SearchTextBox),
                new FrameworkPropertyMetadata(typeof(SearchTextBox)));
        }


        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            HasText = Text.Length != 0;
        }



        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public Brush LabelTextColor
        {
            get { return (Brush)GetValue(LabelTextColorProperty); }
            set { SetValue(LabelTextColorProperty, value); }
        }

        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextPropertyKey, value); }
        }


        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (!HasText)
                LabelText = _originalLabelText;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (!HasText)
            {
                _originalLabelText = LabelText;
                LabelText = "";
            }

        }

        #region OptionClick Routed Event

        public static readonly RoutedEvent OptionClickEvent = EventManager.RegisterRoutedEvent(
            "OptionClick",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SearchTextBox));

        public event RoutedEventHandler OptionClick
        {
            add { AddHandler(OptionClickEvent, value); }
            remove { RemoveHandler(OptionClickEvent, value); }
        }

        protected virtual void OnOptionClick()
        {
            RaiseEvent(new RoutedEventArgs(OptionClickEvent, this));
        }

        #endregion


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var optionsIconBorder = GetTemplateChild("PART_OptionsIconBorder") as Border;
            if (optionsIconBorder != null)
            {
                optionsIconBorder.MouseLeftButtonDown += new MouseButtonEventHandler(OptionsMouseLeftButtonDown);
                optionsIconBorder.MouseLeftButtonUp += new MouseButtonEventHandler(OptionsMouseLeftButtonUp);
            }

            var clearIconBorder = GetTemplateChild("PART_ClearIconBorder") as Border;
            if (clearIconBorder != null)
            {
                clearIconBorder.MouseLeftButtonDown += new MouseButtonEventHandler(ClearMouseLeftButtonDown);
                clearIconBorder.MouseLeftButtonUp += new MouseButtonEventHandler(ClearMouseLeftButtonUp);
            }
        }

        private void ClearMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsMouseLeftButtonDown) return;

            Text = String.Empty;

            IsMouseLeftButtonDown = false;
        }

        private void ClearMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsMouseLeftButtonDown = true;
        }

        private void OptionsMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsMouseLeftButtonDown) return;

            OnOptionClick();

            if (SearchOptionsPopup != null)
            {
                SearchOptionsPopup.IsOpen = true;
            }

            IsMouseLeftButtonDown = false;
        }

        private void OptionsMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsMouseLeftButtonDown = true;
        }

        public bool IsMouseLeftButtonDown { get; set; }
    }
}
