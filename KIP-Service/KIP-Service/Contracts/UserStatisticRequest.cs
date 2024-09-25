namespace KIP_Service.Contracts
{
    public record UserStatisticRequest(
        Guid UserId,
        DateTime From,
        DateTime To);
}
