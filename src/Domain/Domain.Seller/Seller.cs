using System;

namespace Domain.Seller
{
    // Seller account is created upon registration as seller
    public class Seller
    {
        public Seller(Guid id)
        {
            Id = id;
        }
        
        // Should be the same as user id in user management domain
        public Guid Id { get; set; }
        
    }
}