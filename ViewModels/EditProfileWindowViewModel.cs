using CatanClient.ProfileService;
using CatanClient.Commands;
using CatanClient.Services;
using CatanClient.UIHelpers;
using CatanClient.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatanClient.ViewModels
{
    internal class EditProfileWindowViewModel : ViewModelBase
    {

        private string newValue;
        private ProfileDto profile;

        public string Field { get; }


        public string PromptText => $"Ingrese su nuevo {Field}:";
        public string NewValue
        {
            get => newValue;
            set
            {
                newValue = value;
                OnPropertyChanged(nameof(NewValue));
            }
        }

        public ProfileDto Profile { get => profile; set => profile = value; }

        public ICommand SaveCommand { get; }
        private readonly ServiceManager serviceManager;

        public EditProfileWindowViewModel(string field, ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            Field = field;
            AccountService.ProfileDto profileDto = serviceManager.ProfileSingleton.Profile;
            profileDto.PreferredLanguage = CultureInfo.CurrentCulture.Name;

            ProfileService.ProfileDto profile = new ProfileDto();
            profile.Id = profileDto.Id;
            profile.Name = profileDto.Name;
            profile.PicturePath = profileDto.PicturePath;
            profile.PreferredLanguage = CultureInfo.CurrentCulture.Name;

            Profile = profile;

            SaveCommand = new RelayCommand(OnSave);
        }


        private void OnSave()
        {
            switch (Field)
            {
                case "Username":
                    SaveUsername(newValue);

                    break;
                case "Email":
                    SaveEmail(NewValue);
                    break;
                case "Phone":
                    SavePhone(NewValue);
                    break;
            }
        }

        public void SaveUsername(string username)
        {
            OperationResultProfileDto result = ChangeName(Profile, username);
            if (result.IsSuccess)
            {
                MessageBox.Show("Username saved: " + username);
                serviceManager.ProfileSingleton.SetName(username);
            }
            else
            {
                MessageBox.Show("No se pudo cambiar el nombre");
            }
            
            
        }

        public void SaveEmail(string email)
        {
            AccountService.AccountDto account = new AccountService.AccountDto();
            account.Email = email;
            account.PhoneNumber = String.Empty;
            account.Password = String.Empty;
            account.Id = Profile.Id;
            account.PreferredLanguage = CultureInfo.CurrentCulture.Name;
           

            AccountService.OperationResultChangeRegisterEmailOrPhone result;

            result = serviceManager.AccountServiceClient.ChangeEmail(account);

            if (result.IsSuccess)
            {
                MessageBox.Show("Email saved: " + email);
                ShowVerify(account);
            }
            else
            {
                MessageBox.Show("No se pudo guardar el nuevo correo");
                MessageBox.Show(result.MessageResponse);
                MessageBox.Show(result.StatusChangeAccountRegister.ToString());
            }

            
        }

        public void SavePhone(string phone)
        {
            MessageBox.Show("Phone saved: " + phone);
            ShowVerify(null);
        }

        public void ShowVerify(AccountService.AccountDto account)
        {
            var verifyWindow = new VerifyAccountChangeWindow(account);
                
            verifyWindow.ShowDialog();
        }

        public OperationResultProfileDto ChangeName(ProfileDto profile, string newName)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(Utilities.IP_PROFILE_SERVICE);
            ChannelFactory<IProfileServiceEndpoint> channelFactory = new ChannelFactory<IProfileServiceEndpoint>(binding, endpoint);
            IProfileServiceEndpoint client = channelFactory.CreateChannel();
            OperationResultProfileDto result;

            profile.Name = newName;

            try
            {
                result = client.ChangeProfileName(profile);
            }
            catch (Exception ex)
            {
                result = new OperationResultProfileDto
                {
                    IsSuccess = false,
                    MessageResponse = ex.Message,
                };

                Log.Information(ex.Message);
                MessageBox.Show(Utilities.MessageServerLostConnection(CultureInfo.CurrentCulture.Name), Utilities.TittleServerLostConnection(CultureInfo.CurrentCulture.Name), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                ((IClientChannel)client).Close();
                channelFactory.Close();
            }
            return result;
        }



    }
}
