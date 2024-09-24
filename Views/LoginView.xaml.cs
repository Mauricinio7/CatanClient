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

namespace CatanClient.Views
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string placeholderText = textBox.Tag as string;

            if (textBox.Text == placeholderText)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.White;

                //TODO quit hardcode
                if (ShowPasswordCheckBox.IsChecked == false && textBox.Tag == PasswordTextBox.Tag)
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
                textBox.Foreground = Brushes.LightGray;

            }
        }


        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordTextBox.Foreground = Brushes.White;
            }
            else
            {
                PasswordTextBox.Text = PasswordTextBox.Tag as string;
                PasswordTextBox.Foreground = Brushes.LightGray;
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



        private void Button_Click(object sender, RoutedEventArgs e)
        {


        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (TextBoxMail.Text != TextBoxMail.PlaceholderTag && !NullOrWhiteSpacePassword() && !string.IsNullOrWhiteSpace(TextBoxMail.Text))
            {
                string Password;

                if (ShowPasswordCheckBox.IsChecked == false)
                {
                    Password = PasswordBox.Password;
                }
                else
                {
                    Password = PasswordTextBox.Text;
                }

                Console.WriteLine("Correo: " + TextBoxMail.Text);
                Console.WriteLine("Contraseña: " + Password);
            }
            else
            {
                Console.WriteLine("No se ha insertado un correo o contraseña");
            }
        }

        private bool NullOrWhiteSpacePassword()
        {
            return string.IsNullOrWhiteSpace(PasswordBox.Password) && (string.IsNullOrWhiteSpace(PasswordTextBox.Text) || PasswordTextBox.Text == PasswordTextBox.Tag as string);
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Text = PasswordTextBox.Tag as string;
                PasswordTextBox.Foreground = Brushes.LightGray;
                PasswordTextBox.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
