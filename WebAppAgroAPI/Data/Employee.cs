namespace WebAppAgroAPI.Data
{
    public class Employee : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
