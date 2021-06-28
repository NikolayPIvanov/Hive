using System.Threading.Tasks;
using Hive.Chat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Hive.Chat.Hubs
{
    public interface IChatHub
    {
        Task MessageReceivedFromHub(Message message);

        Task NewUserConnected(string message);
    }
    
    public class ChatHub : Hub<IChatHub>
    {
        public async Task BroadcastAsync(Message message)
        {
            await Clients.All.MessageReceivedFromHub(message);
        }
        
        public override async Task OnConnectedAsync()
        {
            await Clients.All.NewUserConnected("a new user connected");
        }
    }
}