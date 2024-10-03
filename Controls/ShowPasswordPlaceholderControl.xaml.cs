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

     public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(string.Empty));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
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

        public static readonly DependencyProperty BackgroundColorValueProperty =
            DependencyProperty.Register("BackgroundColorValue", typeof(Brush), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(Brushes.White));

        public Brush BackgroundColorValue
        {
            get { return (Brush)GetValue(BackgroundColorValueProperty); }
            set { SetValue(BackgroundColorValueProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderForegroundColorValueProperty =
            DependencyProperty.Register("PlaceholderForegroundColorValue", typeof(Brush), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(Brushes.LightGray));

        public Brush PlaceholderForegroundColorValue
        {
            get { return (Brush)GetValue(PlaceholderForegroundColorValueProperty); }
            set { SetValue(PlaceholderForegroundColorValueProperty, value); }
        }

        public static readonly DependencyProperty WritingForegroundColorValueProperty =
            DependencyProperty.Register("WritingForegroundColorValue", typeof(Brush), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(Brushes.White));

        public Brush WritingForegroundColorValue
        {
            get { return (Brush)GetValue(WritingForegroundColorValueProperty); }
            set { SetValue(WritingForegroundColorValueProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata("Password"));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty FontSizeValueProperty =
            DependencyProperty.Register("FontSizeValue", typeof(double), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(15.0));

        public double FontSizeValue
        {
            get { return (double)GetValue(FontSizeValueProperty); }
            set { SetValue(FontSizeValueProperty, value); }
        }

        public static readonly DependencyProperty PaddingValueProperty =
            DependencyProperty.Register("PaddingValue", typeof(Thickness), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(new Thickness(15)));

        public Thickness PaddingValue
        {
            get { return (Thickness)GetValue(PaddingValueProperty); }
            set { SetValue(PaddingValueProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessValueProperty =
            DependencyProperty.Register("BorderThicknessValue", typeof(Thickness), typeof(ShowPasswordPlaceholderControl), new PropertyMetadata(new Thickness(0)));

        public Thickness BorderThicknessValue
        {
            get { return (Thickness)GetValue(BorderThicknessValueProperty); }
            set { SetValue(BorderThicknessValueProperty, value); }
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

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                PasswordTextBox.Text = string.Empty;
                PlaceholderLabel.Content = string.Empty;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                PlaceholderLabel.Content = PlaceholderText;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text != PasswordBox.Password)
            {
                PasswordTextBox.Text = PasswordBox.Password; 
            }
        }

        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Collapsed;
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Visibility = Visibility.Collapsed;
            PasswordBox.Visibility = Visibility.Visible;

            if (PasswordBox.Password != PasswordTextBox.Text)
            {
                PasswordBox.Password = PasswordTextBox.Text; 
            }
        }
    }
}