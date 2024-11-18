using Autofac;
using CatanClient.ChatService;
using CatanClient.Gameplay.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatanClient.Gameplay.Views
{
    /// <summary>
    /// Lógica de interacción para GameFrameView.xaml
    /// </summary>
    public partial class GameFrameView : UserControl
    {
        public GameFrameView(GameDto game)
        {
            InitializeComponent();
            this.DataContext = App.Container.Resolve<GameFrameViewModel>(
               new TypedParameter(typeof(ChatService.GameDto), game)
           );
        }


    }
}
