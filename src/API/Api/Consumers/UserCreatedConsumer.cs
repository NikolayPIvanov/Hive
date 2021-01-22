using System;
using System.Threading.Tasks;
using IDP.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;
using Seller;

namespace Api.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedConsumer> _logger;
        private readonly ISellerRepository _sellerRepository;

        public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger, ISellerRepository sellerRepository)
        {
            _logger = logger;
            _sellerRepository = sellerRepository;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            await _sellerRepository.CreateSellerAccount(context.Message.Id);
        }
    }
}