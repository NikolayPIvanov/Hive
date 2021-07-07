using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Chat.Data;
using Hive.Chat.Hubs;
using Hive.Chat.Models;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.Identity.Contracts.IntegrationEvents.Outbound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace Hive.Chat.Controllers
{
    public class ConsumerController: Controller
    {
        private readonly IChatContext _context;

        public ConsumerController(IChatContext context)
        {
            _context = context;
        }
        
        [NonAction]
        [CapSubscribe( nameof(UserCreatedIntegrationEvent))]
        public async Task Process(UserCreatedIntegrationEvent @event)
        {
            var filter = Builders<UserIdentifier>.Filter.Eq(x => x.UserId, @event.UserId);
            var identifier = await (await _context.UserIdentifiers.FindAsync(filter)).FirstOrDefaultAsync();

            if (identifier == null)
            {
                var id = new UserIdentifier {
                    UserId = @event.UserId, 
                    UniqueIdentifier = Guid.NewGuid().ToString(), 
                    GivenName = @event.GivenName, 
                    Surname = @event.Surname
                };
                await _context.UserIdentifiers.InsertOneAsync(id);
            }
        }
    }
}