namespace KIP_Service.Core.Models
{
    public class QueryCache<T>(string queryName, DateTime createdDate,  T queryDetails)
    {
        public string QueryName { get; set; } = queryName;
        public DateTime CreatedDate { get; set; } = createdDate;
        public T QueryDetails { get; set; } = queryDetails;
    }
}
