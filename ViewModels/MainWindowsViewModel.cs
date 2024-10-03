using CatanClient.Commands;
using CatanClient.UIHelpers;
using CatanClient.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CatanClient.ViewModels
{
    internal class MainWindowsViewModel : ViewModelBase
    {
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        
        public ICommand ShowRegisterViewCommand { get; }
        public ICommand ShowLoginViewCommand { get; }
        public ICommand ShowVerifyAccountViewCommand { get; }
        public ICommand OcultVerifyAccountViewCommand { get; }

        public MainWindowsViewModel()
        {
            CurrentView = new Views.VerifyAccountView();

            ShowRegisterViewCommand = new RelayCommand(async () => await ShowRegisterView());
            ShowLoginViewCommand = new RelayCommand(async () => await ShowLoginView());
            ShowVerifyAccountViewCommand = new RelayCommand(async () => await ShowVerifyAccountView());
            OcultVerifyAccountViewCommand = new RelayCommand(async () => await OcultVerifyAccountView());

            Mediator.Register("ShowVerifyAccountView", args => ShowVerifyAccountViewCommand.Execute(null));
        }


        private async Task ShowRegisterView()
        {
            if (_currentView is Views.LoginView loginView)
            {
                var animatedGrid = loginView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)animatedGrid.FindResource("SlideOutFromTopAnimation");
                    storyboard.Begin(animatedGrid);  
                    await Task.Delay(1000);  
                }
            }

            CurrentView = new Views.RegisterView(); 
        }
        private async Task ShowLoginView()
        {
            if (_currentView is Views.RegisterView registerView)
            {
                var animatedGrid = registerView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)registerView.FindResource("SlideOutFromTopAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(1000);
                }

                  
            }
            CurrentView = new Views.LoginView();
        }

        private async Task OcultVerifyAccountView()
        {
            if (_currentView is Views.VerifyAccountView verifyAccountView)
            {
                var animatedGrid = verifyAccountView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)verifyAccountView.FindResource("SlideOutFromRightAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(820);
                }


            }
            CurrentView = new Views.RegisterView();
        }

        private async Task ShowVerifyAccountView()
        {
            if (_currentView is Views.RegisterView registerView)
            {
                var animatedGrid = registerView.FindName("animatedGrid") as Grid;
                if (animatedGrid != null)
                {
                    var storyboard = (Storyboard)registerView.FindResource("SlideOutFromTopAnimation");
                    storyboard.Begin(animatedGrid);
                    await Task.Delay(900);
                }


            }
            CurrentView = new Views.VerifyAccountView();
        }

    }
}
