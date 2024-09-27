using CatanClient.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class RegisterViewModel : ViewModelBase
    {
        private string _email;
        private string _password;
        private string _username;



        // Propiedad para Email
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        // Propiedad para Password
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        // Propiedad para Username
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        // Comando para el botón de registro
        public ICommand RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(RegisterUser);
        }

        private void RegisterUser()
        {
            MessageBox.Show($"Bienvenido, {Username}!\nContraseña: {Password} \nCorreo: {Email}", "Login Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
