namespace KIP_Service.Core.Models
{
    public class QueryInfo<T>(Guid queryId, int percent, T? result)
    {
        public Guid Id { get; } = queryId;
        public int Percent { get; } = percent;
        public T? Result { get; } = result;
    }
}
