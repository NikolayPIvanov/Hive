﻿using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Billing.Application.Interfaces;
using Hive.Billing.Domain.Entities;
using Hive.Common.Core;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Persistence
{
    public class BillingDbContext : DbContext, IBillingDbContext
    {
        private const string DefaultSchema = "billing";
        
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTimeService;
        
        public BillingDbContext(
            DbContextOptions<BillingDbContext> options,
            ICurrentUserService currentUserService,
            IMediator mediator,
            IDateTimeService dateTimeService) : base(options)
        {
            _currentUserService = currentUserService;
            _mediator = mediator;
            _dateTimeService = dateTimeService;
        }

        public string Schema => DefaultSchema;
        
        public DbSet<AccountHolder> AccountHolders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity> entry in ChangeTracker.Entries<Entity>())
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

            await _mediator.DispatchDomainEventsAsync(this);
            
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