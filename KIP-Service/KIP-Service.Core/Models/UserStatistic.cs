namespace KIP_Service.Core.Models
{
    public class UserStatistic(Guid id, int countSignIn)
    {
        public Guid Id { get; } = id;
        public int CountSignIn { get; } = countSignIn;
    }
}
