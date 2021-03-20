using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Accounts;
using Hive.Domain.Entities.Categories;

namespace Hive.Domain.Entities.Investments
{
    public class PlanCategory
    {
        public int CategoryId { get; set; }

        public int PlanId { get; set; }
    }
    
    public class Plan : AuditableEntity
    {
        public Plan()
        {
            Categories = new();
            Investments = new();
            IsReleased = false;
        }
        
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsReleased { get; set; }
        
        public int SellerId { get; set; }

        public Seller Seller { get; set; }

        public List<PlanCategory> Categories { get; private set; }

        public List<Investment> Investments { get; private set; }
        
    }
}