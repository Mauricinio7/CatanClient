using Autofac;
using CatanClient.AccountService;
using CatanClient.GameService;
using CatanClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatanClient.Views
{
    /// <summary>
    /// Lógica de interacción para GameLobbyView.xaml
    /// </summary>
    public partial class GameLobbyView : UserControl
    {

        public GameLobbyView(GameDto gameDto)
        {
            InitializeComponent();
            ChatService.GameDto game = new ChatService.GameDto
            {
                Name = gameDto.Name,
                Id = gameDto.Id,
                MaxNumberPlayers = gameDto.MaxNumberPlayers,
                AccessKey = gameDto.AccessKey,
                IdAdminGame = gameDto.IdAdminGame
            };

            this.DataContext = App.Container.Resolve<GameLobbyViewModel>(
                new TypedParameter(typeof(ChatService.GameDto), game)
            );
        }

        

    }
}

