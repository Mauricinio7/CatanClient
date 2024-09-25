using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CatanClient.Controls
{
    public partial class ShowPasswordPlaceholderControl : UserControl
    {
        public ShowPasswordPlaceholderControl()
        {
            InitializeComponent();

            var widthBinding = new Binding("Width")
            {
                Source = PasswordTextBox,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            PasswordBox.SetBinding(WidthProperty, widthBinding);

            var heightBinding = new Binding("Height")
            {
                Source = PasswordTextBox,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            PasswordBox.SetBinding(HeightProperty, heightBinding);
        }

        public static readonly DependencyProperty TextBoxWidthProperty =
            DependencyProperty.Register("TextBoxWidth", typeof(double), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(200.0));

        public double TextBoxWidth
        {
            get { return (double)GetValue(TextBoxWidthProperty); }
            set { SetValue(TextBoxWidthProperty, value); }
        }

        public static readonly DependencyProperty TextBoxHeightProperty =
            DependencyProperty.Register("TextBoxHeight", typeof(double), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(44.0));

        public double TextBoxHeight
        {
            get { return (double)GetValue(TextBoxHeightProperty); }
            set { SetValue(TextBoxHeightProperty, value); }
        }

        public static readonly DependencyProperty ControlBackgroundProperty =
            DependencyProperty.Register("ControlBackground", typeof(Brush), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(Brushes.White));

        public Brush ControlBackground
        {
            get { return (Brush)GetValue(ControlBackgroundProperty); }
            set { SetValue(ControlBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PromptForegroundProperty =
            DependencyProperty.Register("PromptForeground", typeof(Brush), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(Brushes.LightGray));

        public Brush PromptForeground
        {
            get { return (Brush)GetValue(PromptForegroundProperty); }
            set { SetValue(PromptForegroundProperty, value); }
        }

        public static readonly DependencyProperty WritingForegroundProperty =
            DependencyProperty.Register("WritingForeground", typeof(Brush), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(Brushes.White));

        public Brush WritingForeground
        {
            get { return (Brush)GetValue(WritingForegroundProperty); }
            set { SetValue(WritingForegroundProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata("Password"));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty TagTextProperty =
            DependencyProperty.Register("TagText", typeof(string), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata("Password"));

        public string TagText
        {
            get { return (string)GetValue(TagTextProperty); }
            set { SetValue(TagTextProperty, value); }
        }

        public static readonly DependencyProperty ControlFontSizeProperty =
            DependencyProperty.Register("ControlFontSize", typeof(double), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(15.0));

        public double ControlFontSize
        {
            get { return (double)GetValue(ControlFontSizeProperty); }
            set { SetValue(ControlFontSizeProperty, value); }
        }

        public static readonly DependencyProperty ControlPaddingProperty =
            DependencyProperty.Register("ControlPadding", typeof(Thickness), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(new Thickness(15)));

        public Thickness ControlPadding
        {
            get { return (Thickness)GetValue(ControlPaddingProperty); }
            set { SetValue(ControlPaddingProperty, value); }
        }

        public static readonly DependencyProperty ControlBorderThicknessProperty =
            DependencyProperty.Register("ControlBorderThickness", typeof(Thickness), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(new Thickness(0)));

        public Thickness ControlBorderThickness
        {
            get { return (Thickness)GetValue(ControlBorderThicknessProperty); }
            set { SetValue(ControlBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty CheckBoxMarginProperty =
            DependencyProperty.Register("CheckBoxMargin", typeof(Thickness), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(new Thickness(0, 60, 0, 0)));

        public Thickness CheckBoxMargin
        {
            get { return (Thickness)GetValue(CheckBoxMarginProperty); }
            set { SetValue(CheckBoxMarginProperty, value); }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string placeholderText = textBox.Tag as string;

            if (textBox.Text == placeholderText)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = WritingForeground;

                if (ShowPasswordCheckBox.IsChecked == false)
                {
                    textBox.Visibility = Visibility.Collapsed;
                    PasswordBox.Visibility = Visibility.Visible;
                    PasswordBox.Focus();
                }

            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string placeholderText = textBox.Tag as string;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholderText;
                textBox.Foreground = PromptForeground;

            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Text = PasswordTextBox.Tag as string;
                PasswordTextBox.Foreground = PromptForeground;
                PasswordTextBox.Visibility = Visibility.Visible;
            }
        }


        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordTextBox.Foreground = WritingForeground;
            }
            else
            {
                PasswordTextBox.Text = PasswordTextBox.Tag as string;
                PasswordTextBox.Foreground = PromptForeground;
            }

            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Collapsed;
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text != PasswordTextBox.Tag as string)
            {
                PasswordBox.Password = PasswordTextBox.Text;
            }
            else
            {
                PasswordBox.Password = string.Empty;
            }

            if (PasswordTextBox.Text != PasswordTextBox.Tag as string)
            {
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
            }
        }
    }
}