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

        public string Text
        {
            get { return (PlaceholderText); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(PlaceholderTextBoxControl), new PropertyMetadata("Escribe aquí..."));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTagProperty =
            DependencyProperty.Register("PlaceholderTag", typeof(string), typeof(PlaceholderTextBoxControl), new PropertyMetadata(null));

        public string PlaceholderTag
        {
            get { return (string)GetValue(PlaceholderTagProperty); }
            set { SetValue(PlaceholderTagProperty, value); }
        }

        public static readonly DependencyProperty FontSizeValueProperty =
            DependencyProperty.Register("FontSizeValue", typeof(double), typeof(PlaceholderTextBoxControl), new PropertyMetadata(15.0));

        public double FontSizeValue
        {
            get { return (double)(GetValue(FontSizeValueProperty)); }
            set { SetValue(FontSizeValueProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorValueProperty =
            DependencyProperty.Register("BackgroundColorValue", typeof(Brush), typeof(PlaceholderTextBoxControl), new PropertyMetadata(Brushes.Black));

        public Brush BackgroundColorValue
        {
            get { return (Brush)(GetValue(BackgroundColorValueProperty)); }
            set { SetValue(BackgroundColorValueProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderForegroundColorValueProperty =
            DependencyProperty.Register("ForegroundColorValue", typeof(Brush), typeof(PlaceholderTextBoxControl), new PropertyMetadata(Brushes.LightGray));

        public Brush PlaceholderForegroundColorValue
        {
            get { return (Brush)(GetValue(PlaceholderForegroundColorValueProperty)); }
            set { SetValue(PlaceholderForegroundColorValueProperty, value); }
        }

        public static readonly DependencyProperty NormalForegroundColorValueProperty =
            DependencyProperty.Register("NormalForegroundColorValue", typeof(Brush), typeof(PlaceholderTextBoxControl), new PropertyMetadata(Brushes.White));

        public Brush NormalForegroundColorValue
        {
            get { return (Brush)(GetValue(NormalForegroundColorValueProperty)); }
            set { SetValue(NormalForegroundColorValueProperty, value); }
        }

        public static readonly DependencyProperty PaddingValueProperty =
            DependencyProperty.Register("PaddingValue", typeof(Thickness), typeof(PlaceholderTextBoxControl), new PropertyMetadata(new Thickness(15)));

        public Thickness PaddingValue
        {
            get { return (Thickness)(GetValue(PaddingValueProperty)); }
            set { SetValue(PaddingValueProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessValueProperty =
            DependencyProperty.Register("BorderThicknessValue", typeof(Thickness), typeof(PlaceholderTextBoxControl), new PropertyMetadata(new Thickness(0)));

        public Thickness BorderThicknessValue
        {
            get { return (Thickness)(GetValue(BorderThicknessValueProperty)); }
            set { SetValue(BorderThicknessValueProperty, value); }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string placeholderText = textBox.Tag as string;

            if (textBox.Text == placeholderText)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = NormalForegroundColorValue;
            }
        }


        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string placeholderText = textBox.Tag as string;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholderText;
                textBox.Foreground = PlaceholderForegroundColorValue;

            }
        }
    }
}
