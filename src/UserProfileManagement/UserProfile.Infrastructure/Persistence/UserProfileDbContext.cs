﻿using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;

namespace Hive.UserProfile.Infrastructure.Persistence
{
    using Domain.Entities;
    using Application.Interfaces;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    public class UserProfileDbContext : DbContext, IUserProfileDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;
        private const string DefaultSchema = "up";
        
        public UserProfileDbContext(
            DbContextOptions<UserProfileDbContext> options,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public static string Schema => DefaultSchema;
        
        public DbSet<UserProfile> UserProfiles { get; set; }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTimeService.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTimeService.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            
            return result;
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(DefaultSchema);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}