﻿using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hive.Identity.Contracts.IntegrationEvents;
using Hive.UserProfile.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hive.UserProfile.Application.IntegrationEvents.EventHandlers.Identity
{
    using Domain.Entities;
    
    public class UserCreatedIntegrationEventHandler : ICapSubscribe
    {
        private readonly IUserProfileDbContext _dbContext;
        private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

        public UserCreatedIntegrationEventHandler(IUserProfileDbContext dbContext, ILogger<UserCreatedIntegrationEventHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [CapSubscribe(nameof(UserCreatedIntegrationEvent), Group = "hive.profiles.users.created")]
        public async Task Handle(UserCreatedIntegrationEvent @event)
        {
            var profileExist = await _dbContext.UserProfiles.AnyAsync(u => u.UserId == @event.UserId);
            if (profileExist)
            {
                _logger.LogWarning("User profile for user with id: {@Id} has already been registered", @event.UserId);
                return;
            }

            var profile = new UserProfile(@event.UserId)
            {
                CreatedBy = @event.UserId
            };

            _dbContext.UserProfiles.Add(profile);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("User profile for user with id: {@Id} was successfully registered", @event.UserId);
        }
    }
}