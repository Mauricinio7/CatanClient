using Autofac;
using CatanClient.Gameplay.ViewModels;
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

namespace CatanClient.Gameplay.Views
{
    /// <summary>
    /// Lógica de interacción para TradeWindow.xaml
    /// </summary>
    public partial class TradeWindow : Window
    {
        public TradeWindow(GameService.GameDto game)
        {
            InitializeComponent();
            this.DataContext = App.Container.Resolve<TradeWindowViewModel>(
                new TypedParameter(typeof(GameService.GameDto), game)
            );
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
