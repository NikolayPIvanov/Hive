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
        
        [HttpGet("uuids")]
        public async Task<ActionResult<List<UserIdentifier>>> GetUuids([FromQuery] string userId)
        {
            var filter = new FilterDefinitionBuilder<UserIdentifier>().Where(x => x.UserId != userId);
            var identifiers = await (await _context.UserIdentifiers.FindAsync(filter)).ToListAsync();

            return identifiers;
        }
        
        [HttpPost]
        public async Task<ActionResult<UserIdentifier>> SetIdentifierForUser([FromBody] IdentifierModel model)
        {
            var filter = Builders<UserIdentifier>.Filter.Eq(x => x.UserId, model.UserId);
            var identifier = await (await _context.UserIdentifiers.FindAsync(filter)).FirstOrDefaultAsync();

            if (identifier == null)
            {
                var uuid = new UserIdentifier {UserId = model.UserId, UniqueIdentifier = Guid.NewGuid().ToString()};
                await _context.UserIdentifiers.InsertOneAsync(uuid);
                return CreatedAtAction(nameof(GetUserUniqueIdentifier), new {userId = uuid.UserId}, uuid);
            }

            return BadRequest();
        }

        
        [HttpGet("rooms/participants/{identifier}")]
        public async Task<IActionResult> GetRoomsForUser([FromRoute] string identifier)
        {
            var filter = Builders<Room>.Filter
                .Eq(x => x.ParticipantOne.UniqueIdentifier, identifier);
            filter |= Builders<Room>.Filter.Eq(x => x.ParticipantTwo.UniqueIdentifier, identifier);

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
            
            var filter = Builders<Room>.Filter.Where(x => x.ParticipantOne.UniqueIdentifier == one && x.ParticipantTwo.UniqueIdentifier == two);
            filter |= Builders<Room>.Filter.Where(x =>
                x.ParticipantTwo.UniqueIdentifier == two && x.ParticipantTwo.UniqueIdentifier == one);

            var dbRoom = await (await _context.Rooms.FindAsync(filter)).FirstOrDefaultAsync();

            if (dbRoom != null) return BadRequest();

            var participantOneFilter =
                Builders<UserIdentifier>.Filter.Where(ui => ui.UniqueIdentifier == model.ParticipantOneUI);
            var participantOne =
                await (await _context.UserIdentifiers.FindAsync(participantOneFilter)).FirstOrDefaultAsync();
            
            var participantTwoFilter =
                Builders<UserIdentifier>.Filter.Where(ui => ui.UniqueIdentifier == model.ParticipantTwoUI);
            var participantTwo =
                await (await _context.UserIdentifiers.FindAsync(participantTwoFilter)).FirstOrDefaultAsync();
            
            var room = new Room()
            {
                ParticipantOne = participantOne,
                ParticipantTwo = participantTwo,
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

            var participantToFind = room.ParticipantOne.UniqueIdentifier == message.SenderIdentifier.ToString()
                ? room.ParticipantTwo
                : room.ParticipantOne;

            var userFilter =
                Builders<UserIdentifier>.Filter.Eq(x => x.UniqueIdentifier, participantToFind.UniqueIdentifier);
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