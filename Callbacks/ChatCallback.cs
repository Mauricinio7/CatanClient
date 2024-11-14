using CatanClient.ChatService;
using CatanClient.UIHelpers;
using CatanClient.ViewModels;
using System;   
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatanClient.Callbacks
{
    internal class ChatCallback : IChatServiceEndpointCallback
    {
        public void ReceiveMessage(string name, string message)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                ChatMessage chatMessage = new ChatMessage { Content = message, Name = name, IsUserMessage = false };
                Mediator.Notify(Utilities.RECIVE_MESSAGE, chatMessage);
            });
        }

        public void ReceiveMessageJoinPlayerToChat(string name)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                ChatMessage systemMessage = new ChatMessage { Content = name + Utilities.MessagePlayerJoin(CultureInfo.CurrentCulture.Name), Name = Utilities.SYSTEM_NAME, IsUserMessage = false };
                Mediator.Notify(Utilities.RECIVE_MESSAGE, systemMessage);
                Mediator.Notify(Utilities.LOAD_PLAYER_LIST, null);
            });
        }

        public void ReceiveMessageLeftPlayerToChat(string name)
        {
            App.Current.Dispatcher.InvokeAsync(async () =>
            {
                ChatMessage systemMessage = new ChatMessage
                {
                    Content = name + Utilities.MessagePlayerLeft(CultureInfo.CurrentCulture.Name),
                    Name = Utilities.SYSTEM_NAME,
                    IsUserMessage = false
                };

                Mediator.Notify(Utilities.RECIVE_MESSAGE, systemMessage);
                await Task.Delay(3000);

                Mediator.Notify(Utilities.LOAD_PLAYER_LIST, null);
            });
        }
    }
}
