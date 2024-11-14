using Autofac;
using CatanClient.UIHelpers;
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
    /// Lógica de interacción para KickPlayerWindow.xaml
    /// </summary>
    public partial class KickPlayerWindow : Window
    {
        public KickPlayerWindow(ChatService.GameDto game)
        {
            InitializeComponent();
            this.DataContext = App.Container.Resolve<KickPlayerWindowViewModel>(
                new TypedParameter(typeof(ChatService.GameDto), game)
            );
            Mediator.Register(Utilities.CLOSE_KICK_PLAYER, _ => this.Close());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
