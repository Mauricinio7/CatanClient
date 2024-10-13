using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace CatanClient.Views
{
    /// <summary>
    /// Lógica de interacción para GameLobbyView.xaml
    /// </summary>
    public partial class GameLobbyView : UserControl
    {

        public GameLobbyView()
        {
            InitializeComponent();
            // Asignar el DataContext (solo si es necesario)
            this.DataContext = new GameLobbyViewModel();
        }

        // Definir el conversor dentro del code-behind
        

    }
}

