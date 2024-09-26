namespace KIP_Service.Core.Models
{
    public class QueryInfo<ResultType>(Guid queryId, int percent, ResultType? result)
    {
        public Guid Id { get; } = queryId;
        public int Percent { get; } = percent;
        public ResultType? Result { get; } = result;
    }
}
