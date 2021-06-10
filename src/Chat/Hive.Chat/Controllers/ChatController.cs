using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Chat.Data;
using Hive.Chat.Hubs;
using Hive.Chat.Models;
using Hive.Identity.Contracts.IntegrationEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hive.Chat.Controllers
{
    public class IdentifierModel
    {
        public string UserId { get; set; }
    }

    public class RoomCreateModel
    {
        public string ParticipantOneUI { get; set; }
        public string ParticipantTwoUI { get; set; }
    }
    
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> hubContext;
        private readonly IChatContext _context;

        public ChatController(IHubContext<ChatHub> hubContext, IChatContext context)
        {
            this.hubContext = hubContext;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<UserIdentifier>> GetUserUniqueIdentifier([FromQuery] string userId)
        {
            var filter = new FilterDefinitionBuilder<UserIdentifier>().Eq(x => x.UserId, userId);
            var identifier = await (await _context.UserIdentifiers.FindAsync(filter)).FirstOrDefaultAsync();

            if (identifier != null)
            {
                return Ok(identifier);
            }

            return NotFound();
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> SetIdentifierForUser([FromBody] IdentifierModel model)
        {
            var filter = Builders<UserIdentifier>.Filter.Eq(x => x.UserId, model.UserId);
            var identifier = await (await _context.UserIdentifiers.FindAsync(filter)).FirstOrDefaultAsync();

            if (identifier == null)
            {
                var id = new UserIdentifier {UserId = model.UserId, UniqueIdentifier = Guid.NewGuid()};
                await _context.UserIdentifiers.InsertOneAsync(id);
                return CreatedAtAction(nameof(GetUserUniqueIdentifier), new {userId = id.UserId}, id.UniqueIdentifier);
            }

            return BadRequest();
        }

        
        [HttpGet("rooms/participants/{identifier}")]
        public async Task<IActionResult> GetRoomsForUser([FromRoute] string identifier)
        {
            var filter = Builders<Room>.Filter
                .Eq(x => x.ParticipantOne, identifier);
            filter |= Builders<Room>.Filter.Eq(x => x.ParticipantTwo, identifier);

            var cursor =  await _context.Rooms.FindAsync(filter);
            var list = await cursor.ToListAsync();

            return Ok(list);
        }
        
        [HttpGet("rooms/{id}")]
        public async Task<IActionResult> GetRoomById([FromRoute] string id)
        {
            var filter = Builders<Room>.Filter
                .Eq(x => x.Id, id);

            var cursor =  await _context.Rooms.FindAsync(filter);
            var room = await cursor.FirstOrDefaultAsync();
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }
        
        [HttpPost("rooms")]
        public async Task<ActionResult<Room>> CreateRoom([FromBody] RoomCreateModel model)
        {
            var one = model.ParticipantOneUI.ToString();
            var two = model.ParticipantTwoUI.ToString();
            
            var filter = Builders<Room>.Filter.Where(x =>
                (x.ParticipantOne == one && x.ParticipantTwo == two) ||
                (x.ParticipantTwo == two && x.ParticipantTwo == one));
            
            var dbRoom = await (await _context.Rooms.FindAsync(filter)).FirstOrDefaultAsync();

            if (dbRoom != null) return BadRequest();
            var room = new Room()
            {
                ParticipantOne = one,
                ParticipantTwo = two,
                Messages = new List<Message>()
            };
            await _context.Rooms.InsertOneAsync(room);
                
            return CreatedAtAction(nameof(GetRoomById), new { id = room.Id}, room);
        }

        [HttpPost("rooms/{id}/messages")]
        public async Task<IActionResult> SendMessage([FromRoute] string id, [FromBody] MessageCreate message)
        {
            //additional business logic
            var msg = new Message()
            {
                Text = message.Text,
                SenderIdentifier = message.SenderIdentifier,
                DateTime = DateTime.UtcNow
            };
            var filter = Builders<Room>.Filter.Eq(x => x.Id, id);
            var update = Builders<Room>.Update.Push(x => x.Messages, msg);
            var room = await _context.Rooms.FindOneAndUpdateAsync(filter, update);

            var participantToFind = room.ParticipantOne == message.SenderIdentifier.ToString()
                ? room.ParticipantTwo
                : room.ParticipantOne;

            var userFilter = Builders<UserIdentifier>.Filter.Eq(x => x.UniqueIdentifier, Guid.Parse(participantToFind));
            var participant = await (await _context.UserIdentifiers.FindAsync(userFilter)).FirstAsync();

            var response = new MessageSaved {RoomId = room.Id, Text = msg.Text, DateTime = msg.DateTime, SenderIdentifier = msg.SenderIdentifier };
            // await this.hubContext.Clients.User(participant.UserId).SendAsync("messageReceivedFromApi", response);
            
            await this.hubContext.Clients.All.SendAsync("messageReceivedFromApi", response);

            return Ok();
        }
    }

    public class MessageSaved
    {
        public string RoomId { get; set; }

        public Guid SenderIdentifier { get; set; }
        public string Text { get; set; }

        public DateTime DateTime { get; set; }
    }
    
    public class MessageCreate
    {
        public Guid SenderIdentifier { get; set; }
        
        public string Text { get; set; }
    }
}