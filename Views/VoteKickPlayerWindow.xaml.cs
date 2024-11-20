using Autofac;
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
    /// Lógica de interacción para VoteKickPlayerWindow.xaml
    /// </summary>
    public partial class VoteKickPlayerWindow : Window
    {
        public VoteKickPlayerWindow(ChatService.GameDto game)
        {
            InitializeComponent();
            this.DataContext = App.Container.Resolve<VoteKickPlayerWindowViewModel>(
                new TypedParameter(typeof(ChatService.GameDto), game)
            );
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
