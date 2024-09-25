namespace KIP_Service.Contracts
{
    public record QueryInfoResponse<T>(
        Guid Query,
        int Percent,
        T? result);
}
