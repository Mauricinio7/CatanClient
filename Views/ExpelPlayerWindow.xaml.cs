using Autofac;
using CatanClient.GameService;
using CatanClient.Services;
using CatanClient.ViewModels;
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
using System.Windows.Shapes;

namespace CatanClient.Views
{
    /// <summary>
    /// Lógica de interacción para ExpelPlayerWindow.xaml
    /// </summary>
    public partial class ExpelPlayerWindow : Window
    {
        public ExpelPlayerWindow(ProfileService.ProfileDto profile, GameDto game)
        {
            InitializeComponent();
            this.DataContext = App.Container.Resolve<ExpelPlayerWindowViewModel>(
            new TypedParameter(typeof(ProfileService.ProfileDto), profile),
            new TypedParameter(typeof(GameDto), game)
        );
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
