namespace KIP_Service.Core.Models
{
    public class QueryCache<TDetails, TResult>(
        Guid id,
        string queryName,
        DateTime createdDate,
        TDetails queryDetails,
        TResult? result = default,
        bool isCompleted = false)
    {
        public Guid Id { get; set; } = id;
        public string QueryName { get; set; } = queryName;
        public DateTime CreatedDate { get; set; } = createdDate;
        public TDetails QueryDetails { get; set; } = queryDetails;
        public TResult? Result { get; set; } = result;
        public bool IsCompleted { get; set; } = isCompleted;
    }
}
