using CatanClient.GameService;
using CatanClient.UIHelpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatanClient.Services
{
    public interface IGameServiceClient
    {

        OperationResultGameDto CreateRoomClient(GameDto game, ProfileDto profile);
        

        OperationResultGameDto JoinRoomClient(string code, ProfileDto profile);

        OperationResultGameDto JoinRoomAsGuestClient(string code, GuestAccountDto profile);

        OperationResultListOfPlayersInGame GetPlayerList(GameDto game);

        bool LeftRoomClient(GameDto game, ProfileDto profile);

        bool ExpelPlayer(ExpelPlayerDto expelPlayer, int idPlayer, GameService.GameDto game);


        }
}
