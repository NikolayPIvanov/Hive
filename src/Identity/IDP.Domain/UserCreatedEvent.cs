using System;

namespace IDP.Domain
{
    public enum UserType
    {
        Seller,
        Buyer,
        Investor
    }
    public class UserCreatedEvent
    {
        public Guid Id { get; set; }
        
        public UserType Type { get; set; }
    }
}