using CatanClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using CatanClient.Views;
using System.Windows.Media.Animation;

namespace CatanClient.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }


        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }


        //TODO Exeptions and quit all hard code
        private void ExecuteLogin(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                //TODO quit hardcode
                if (isValidUser())
                {
                    MessageBoxResult result = MessageBox.Show($"Bienvenido, {Username}!\nContraseña: {Password}", "Login Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (result == MessageBoxResult.OK)
                    {
                        if (parameter is Window ventanaActual)
                        {
                            ventanaActual.IsEnabled = false;
                            Storyboard fadeOutStoryboard = ventanaActual.FindResource("FadeOutAnimation") as Storyboard;
                            if (fadeOutStoryboard != null)
                            {
                                fadeOutStoryboard.Completed += (s, e) =>
                                {
                                    //TODO  
                                    
                                };

                                fadeOutStoryboard.Begin(ventanaActual);
                            }
                        }
                    }
                }
            }
            //TODO invalid valors exception
            else if (!isValidEmail() || !isValidPassword())
            {
                MessageBox.Show("Se han ingresado datos invalidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //TODO null valors exception
                MessageBox.Show("Se han dejado campos vacios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public bool isValidEmail()
        {
            return true;
        }
        public bool isValidPassword()
        {
            return true;
        }

        public bool isValidUser()
        {
            //TODO validate user with the server
            return true;
        }
    }
}
