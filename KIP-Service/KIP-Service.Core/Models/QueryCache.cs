namespace KIP_Service.Core.Models
{
    public class QueryCache<DetailsType, ResultType>(
        Guid id,
        string queryName,
        DateTime createdDate,
        DetailsType queryDetails,
        ResultType? result = default,
        bool isCompleted = false)
    {
        public Guid Id { get; set; } = id;
        public string QueryName { get; set; } = queryName;
        public DateTime CreatedDate { get; set; } = createdDate;
        public DetailsType QueryDetails { get; set; } = queryDetails;
        public ResultType? Result { get; set; } = result;
        public bool IsCompleted { get; set; } = isCompleted;
    }
}
