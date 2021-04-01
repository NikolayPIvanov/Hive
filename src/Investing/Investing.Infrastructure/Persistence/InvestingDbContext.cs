using Hive.Common.Application.Interfaces;
using Hive.Investing.Application.Interfaces;
using Hive.Investing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hive.Investing.Infrastructure.Persistence
{
    public class InvestingDbContext : DbContext, IInvestingDbContext
    {
        private readonly IDateTimeService _dateTimeService;
        private const string DefaultSchema = "investing";
        
        public InvestingDbContext(
            DbContextOptions<InvestingDbContext> options,
            IDateTimeService dateTimeService) : base(options)
        {
            _dateTimeService = dateTimeService;
        }

        public static string Schema => DefaultSchema;
        
        public DbSet<Investor> Investors { get; set; }
        public DbSet<Investment> Investments { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
    }
}