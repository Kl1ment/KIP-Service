namespace KIP_Service.Core.Models
{
    public class QueryInfo<TResult>(Guid queryId, int percent, TResult? result)
    {
        public Guid Id { get; } = queryId;
        public int Percent { get; } = percent;
        public TResult? Result { get; } = result;
    }
}
