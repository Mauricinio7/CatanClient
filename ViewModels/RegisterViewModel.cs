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


        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public ICommand RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(RegisterUser);
        }

        private void RegisterUser()
        {
            //TODO Validate data and enter it into the database

            // Validar si algún campo está vacío
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Todos los campos deben estar llenos.", "Error de Registro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar el correo
            if (!Email.Contains("@") || !Email.Contains("."))
            {
                MessageBox.Show("El correo debe contener una arroba (@) y un punto (.)", "Error de Registro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validar la contraseña (al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial)
            if (Password.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres.", "Error de Registro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool hasUpperChar = Password.Any(char.IsUpper);
            bool hasLowerChar = Password.Any(char.IsLower);
            bool hasDigit = Password.Any(char.IsDigit);
            bool hasSpecialChar = Password.Any(ch => !char.IsLetterOrDigit(ch));

            if (!hasUpperChar || !hasLowerChar || !hasDigit || !hasSpecialChar)
            {
                MessageBox.Show("La contraseña debe tener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.", "Error de Registro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (serverConexion())
            {
                // Si todo es correcto, muestra el mensaje de bienvenida
                MessageBox.Show($"Bienvenido, {Username}!\nContraseña: {Password} \nCorreo: {Email}", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        public bool serverConexion()
        {
            //TODO server conexion
            return true;
        }
    }
}
