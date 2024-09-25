namespace KIP_Service.DataAccess.Entities
{
    public class UserSingInEntity
    {
        public Guid Id { get; set; }
        public DateTime SingInDate { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;
    }
}
