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
using CatanClient.UIHelpers;
using System.Linq.Expressions;
using CatanClient.AccountService;
using System.ServiceModel;
using System.Security.Principal;

namespace CatanClient.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private AccountDto accountDto;

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
            if (!isValidEmail() || !isValidPassword())
            {
                //TODO invalid valors exception
                MessageBox.Show("Se han ingresado datos invalidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                //TODO null valors exception
                MessageBox.Show("Se han dejado campos vacios.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //TODO quit hardcode

                switch (isValidUser())
                {
                    case AccountService.AuthenticationStatus.Verified:
                        ShowMainMenu(parameter);
                        break;
                    case AccountService.AuthenticationStatus.NotVerified:
                        ShowVerifyAccountView(accountDto);
                        break;
                    case AccountService.AuthenticationStatus.Incorrect:
                        MessageBox.Show("Usuario no existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }

               
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

        public AuthenticationStatus isValidUser()
        {

            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://192.168.1.127:8181/AccountService");
            var channelFactory = new ChannelFactory<IAccountEndPoint>(binding, endpoint);
            IAccountEndPoint client = channelFactory.CreateChannel();
            OperationResultProfileDto result;

            try
            {
                accountDto = new AccountDto
                {
                    //TODO phoneNumber
                    Name = "",
                    Email = Username,
                    PhoneNumber = "",
                    Password = Password,
                    PicturePath = "",
                    PreferredLanguage = ""
                };


                result = client.LogInAsync(accountDto).Result;
                MessageBox.Show(result.IsSuccess + " " + result.MessageResponse);
                ((IClientChannel)client).Close();
                channelFactory.Close();

                return result.Status;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return AccountService.AuthenticationStatus.Incorrect;
            }
            finally
            {
               // ((IClientChannel)client).Close();
               // channelFactory.Close();


            }
        }

        internal void ShowMainMenu(object parameter)
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
                            Mediator.Notify("ShowMainMenuBackgroundView", null);
                            Mediator.Notify("ShowMainMenuView", null);
                            Storyboard fadeInStoryboard = ventanaActual.FindResource("FadeInAnimation") as Storyboard;
                            if (fadeInStoryboard != null)
                            {
                                fadeInStoryboard.Completed += (sender, args) =>
                                {
                                    ventanaActual.IsEnabled = true;
                                };
                                fadeInStoryboard.Begin(ventanaActual);
                                ventanaActual.IsEnabled = true;
                            }
                        };

                        fadeOutStoryboard.Begin(ventanaActual);
                    }
                }
            }
        }

        internal void ShowVerifyAccountView(AccountDto account)
        {
            MessageBox.Show("Usuario no verificado.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            Mediator.Notify("ShowVerifyAccountView", account);
        }

    }
}
