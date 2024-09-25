namespace KIP_Service.Contracts
{
    public record UserStatisticResponse(
        Guid UserId,
        int CountSingIn);
}
