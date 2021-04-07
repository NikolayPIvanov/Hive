namespace Hive.Domain.Enums
{
    public enum OrderState
    {
        Validation,
        OrderValid,
        UserBalanceValid,
        Invalid,
        Pending,
        Canceled,
        Accepted,
        Declined,
        InProgress,
        Completed
    }
}