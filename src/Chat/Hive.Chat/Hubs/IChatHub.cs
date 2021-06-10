using System;
using System.Threading.Tasks;
using Hive.Chat.Models;

namespace Hive.Chat.Hubs
{
    
    public interface IChatHub
    {
        Task MessageReceivedFromHub(Message message);

        Task NewUserConnected(string message);
    }
}