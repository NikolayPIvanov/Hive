using System.Collections.Generic;
using Hive.Domain.Common;
using Hive.Domain.Entities.Categories;

namespace Hive.Domain.Entities.Accounts
{
    public class Plan : AuditableEntity
    {
        public Plan()
        {
            InvolvedCategories = new();
            Investments = new();
            IsReleased = false;
        }
        
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsReleased { get; set; }
        
        public int SellerId { get; set; }

        public Seller Seller { get; set; }

        public List<Category> InvolvedCategories { get; private set; }

        public List<InvestmentContract> Investments { get; private set; }
        
    }
}