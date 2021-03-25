// using Hive.Application.Common.Interfaces;
// using Hive.Domain.Common;
// using Hive.Domain.Entities;
// using Hive.Infrastructure.Identity;
// using IdentityServer4.EntityFramework.Options;
// using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Options;
// using System.Linq;
// using System.Reflection;
// using System.Threading;
// using System.Threading.Tasks;
// using Hive.Domain.Entities.Accounts;
// using Hive.Domain.Entities.Categories;
// using Hive.Domain.Entities.Gigs;
// using Hive.Domain.Entities.Investments;
// using Hive.Domain.Entities.Orders;
//
// namespace Hive.Infrastructure.Persistence
// {
//     public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
//     {
//         private readonly ICurrentUserService _currentUserService;
//         private readonly IDateTime _dateTime;
//         private readonly IDomainEventService _domainEventService;
//
//         public ApplicationDbContext(
//             DbContextOptions<ApplicationDbContext> options,
//             IOptions<OperationalStoreOptions> operationalStoreOptions,
//             ICurrentUserService currentUserService,
//             IDomainEventService domainEventService,
//             IDateTime dateTime) : base(options, operationalStoreOptions)
//         {
//             _currentUserService = currentUserService;
//             _domainEventService = domainEventService;
//             _dateTime = dateTime;
//         }
//
//         public DbSet<Gig> Gigs { get; set; }
//         
//         public DbSet<GigQuestion> GigQuestions { get; set; }
//         
//         public DbSet<Category> Categories { get; set; }
//         
//         // Accounts
//         public DbSet<Seller> Sellers { get; set; }
//         
//         public DbSet<UserProfile> Profiles { get; set; }
//         
//         public DbSet<Package> Packages { get; set; }
//         
//         public DbSet<Order> Orders { get; set; }
//         
//         public DbSet<Requirement> Requirements { get; set; }
//         public DbSet<Review> Reviews { get; set; }
//         
//         
//         public DbSet<Plan> Plans { get; set; }
//         public DbSet<Investor> Investors { get; set; }
//         public DbSet<Investment> Investments { get; set; }
//         
//         
//
//         public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
//         {
//             foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
//             {
//                 switch (entry.State)
//                 {
//                     case EntityState.Added:
//                         entry.Entity.CreatedBy = _currentUserService.UserId;
//                         entry.Entity.Created = _dateTime.Now;
//                         break;
//
//                     case EntityState.Modified:
//                         entry.Entity.LastModifiedBy = _currentUserService.UserId;
//                         entry.Entity.LastModified = _dateTime.Now;
//                         break;
//                 }
//             }
//
//             var result = await base.SaveChangesAsync(cancellationToken);
//
//             await DispatchEvents();
//
//             return result;
//         }
//
//         protected override void OnModelCreating(ModelBuilder builder)
//         {
//             builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
//
//             base.OnModelCreating(builder);
//         }
//
//         private async Task DispatchEvents()
//         {
//             while (true)
//             {
//                 var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
//                     .Select(x => x.Entity.DomainEvents)
//                     .SelectMany(x => x)
//                     .Where(domainEvent => !domainEvent.IsPublished)
//                     .FirstOrDefault();
//                 if (domainEventEntity == null) break;
//
//                 domainEventEntity.IsPublished = true;
//                 await _domainEventService.Publish(domainEventEntity);
//             }
//         }
//     }
// }
