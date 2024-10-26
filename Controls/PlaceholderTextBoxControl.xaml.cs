using CatanClient.UIHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatanClient.Controls
{
    /// <summary>
    /// Lógica de interacción para PlaceholderTextBoxControl.xaml
    /// </summary>
    public partial class PlaceholderTextBoxControl : UserControl
    {
        public PlaceholderTextBoxControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.Register(Utilities.LABEL_STYLE, typeof(Style), typeof(PlaceholderTextBoxControl), new PropertyMetadata(null));

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(Utilities.TEXT, typeof(string), typeof(PlaceholderTextBoxControl),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(Utilities.PLACEHOLDER_TEXT, typeof(string), typeof(PlaceholderTextBoxControl), new PropertyMetadata("Write here..."));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }


        public static readonly DependencyProperty FontSizeValueProperty =
            DependencyProperty.Register(Utilities.FONT_SIZE_VALUE, typeof(double), typeof(PlaceholderTextBoxControl), new PropertyMetadata(15.0));

        public double FontSizeValue
        {
            get { return (double)(GetValue(FontSizeValueProperty)); }
            set { SetValue(FontSizeValueProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorValueProperty =
            DependencyProperty.Register(Utilities.BACKGROUND_COLOR_VALUE, typeof(Brush), typeof(PlaceholderTextBoxControl), new PropertyMetadata(Brushes.Black));

        public Brush BackgroundColorValue
        {
            get { return (Brush)(GetValue(BackgroundColorValueProperty)); }
            set { SetValue(BackgroundColorValueProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderForegroundColorValueProperty =
            DependencyProperty.Register(Utilities.PLACEHOLDER_FOREGROUND_COLOR_VALUE, typeof(Brush), typeof(PlaceholderTextBoxControl), new PropertyMetadata(Brushes.LightGray));

        public Brush PlaceholderForegroundColorValue
        {
            get { return (Brush)(GetValue(PlaceholderForegroundColorValueProperty)); }
            set { SetValue(PlaceholderForegroundColorValueProperty, value); }
        }

        public static readonly DependencyProperty WritingForegroundColorValueProperty =
            DependencyProperty.Register(Utilities.WRITING_FOREGROUND_COLOR_VALUE, typeof(Brush), typeof(PlaceholderTextBoxControl), new PropertyMetadata(Brushes.White));

        public Brush WritingForegroundColorValue
        {
            get { return (Brush)(GetValue(WritingForegroundColorValueProperty)); }
            set { SetValue(WritingForegroundColorValueProperty, value); }
        }

        public static readonly DependencyProperty PaddingValueProperty =
            DependencyProperty.Register(Utilities.PADDING_VALUE, typeof(Thickness), typeof(PlaceholderTextBoxControl), new PropertyMetadata(new Thickness(15)));

        public Thickness PaddingValue
        {
            get { return (Thickness)(GetValue(PaddingValueProperty)); }
            set { SetValue(PaddingValueProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessValueProperty =
            DependencyProperty.Register(Utilities.BORDER_THICKNESS_VALUE, typeof(Thickness), typeof(PlaceholderTextBoxControl), new PropertyMetadata(new Thickness(0)));

        public Thickness BorderThicknessValue
        {
            get { return (Thickness)(GetValue(BorderThicknessValueProperty)); }
            set { SetValue(BorderThicknessValueProperty, value); }
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = string.Empty;
                PlaceholderLabel.Content = string.Empty;
            }
        }


        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                PlaceholderLabel.Content = PlaceholderText;
            }
        }
    }
}
