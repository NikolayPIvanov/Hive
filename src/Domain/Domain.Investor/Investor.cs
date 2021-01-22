using System;

namespace Domain.Investor
{
    // Investor account is created upon registration as investor
    public class Investor
    {
        // Should be the same as user id in user management domain
        public Guid Id { get; set; }
    }
}