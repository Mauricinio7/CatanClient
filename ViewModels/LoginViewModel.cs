using CatanClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

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
            // Inicializar el comando
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        // Método ejecutado al presionar el botón de login
        private void ExecuteLogin(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                MessageBox.Show("No se ha ingresado ningún nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("No se ha ingresado ninguna contraseña.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show($"Bienvenido, {Username}!\nContraseña: {Password}", "Login Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
