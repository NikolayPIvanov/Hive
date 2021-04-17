namespace Hive.Domain.Enums
{
    public enum OrderState
    {
        Validation = 0,
        OrderDataValid,
        UserBalanceValid,
        Invalid,
        Canceled,
        Accepted,
        Declined,
        InProgress,
        Completed
    }
}