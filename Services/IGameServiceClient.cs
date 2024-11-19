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
        Task<OperationResultGameDto> CreateRoomClientAsync(GameDto game, ProfileDto profile);

        Task<OperationResultGameDto> JoinRoomClientAsync(string code, ProfileDto profile);

        Task<OperationResultGameDto> JoinRoomAsGuestClientAsync(string code, GuestAccountDto profile);

        OperationResultListOfPlayersInGame GetPlayerList(GameDto game);

        Task<bool> LeftRoomClientAsync(GameDto game, ProfileDto profile);

        Task<bool> ExpelPlayerAsync(ExpelPlayerDto expelPlayer, int idPlayer, GameDto game);

        Task<bool> LeftRoomGuestClientAsync(GameDto game, GuestAccountDto guest);
        Task<bool> StartGameAsync(PlayerGameplayDto playerGameplayDto, GameDto gameClientDto);
        Task<bool> CancelStartGameAsync(PlayerGameplayDto playerGameplayDto, GameDto gameClientDto);
        Task<bool> ThrowDiceAsync(PlayerGameplayDto playerGameplayDto, GameDto gameClientDto);

    }
}
