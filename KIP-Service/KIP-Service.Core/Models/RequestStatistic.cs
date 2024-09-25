namespace KIP_Service.Core.Models
{
    public class RequestStatistic(Guid userId, DateTime from, DateTime to)
    {
        public Guid UserId { get; } = userId;
        public DateTime From { get; } = from;
        public DateTime To { get; } = to;
    }
}
