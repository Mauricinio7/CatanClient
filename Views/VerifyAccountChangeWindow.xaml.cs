using Autofac;
using CatanClient.AccountService;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
    /// Lógica de interacción para VerifyAccountChangeWindow.xaml
    /// </summary>
    public partial class VerifyAccountChangeWindow : Window
    {
        public VerifyAccountChangeWindow(AccountDto account)
        {
            InitializeComponent();
            this.DataContext = App.Container.Resolve<VerifyAccountChangeWindowViewModel>(
                new TypedParameter(typeof(AccountDto), account)
            );
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
