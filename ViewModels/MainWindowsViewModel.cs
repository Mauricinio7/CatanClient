using CatanClient.Commands;
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

        public MainWindowsViewModel()
        {
            CurrentView = new Views.LoginView();

            ShowRegisterViewCommand = new RelayCommand(async () => await ShowRegisterView());
            ShowLoginViewCommand = new RelayCommand(async () => await ShowLoginView());
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

    }
}
