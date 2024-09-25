namespace KIP_Service.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public List<UserSingInEntity>? userSingIns { get; set; }
    }
}
