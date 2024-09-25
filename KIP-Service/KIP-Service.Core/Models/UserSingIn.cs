namespace KIP_Service.Core.Models
{
    public class UserSingIn(Guid id, Guid userId, DateTime singInDate)
    {
        public Guid Id { get; } = id;
        public Guid UserId { get; } = userId;
        public DateTime SingInDate { get; } = singInDate;
    }
}
