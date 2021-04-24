namespace Hive.Identity.Contracts
{
    public enum IdentityType
    {
        Admin = 0,
        Buyer,
        Seller,
        Investor
    }

    public static class IdentityTypeStrings
    {
        public const string Admin = "Admin";
        public const string Buyer = "Buyer";
        public const string Seller = "Seller";
        public const string Investor = "Investor";
    }
}