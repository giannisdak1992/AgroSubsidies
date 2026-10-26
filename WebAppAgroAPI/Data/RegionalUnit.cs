namespace WebAppAgroAPI.Data
{
    public class RegionalUnit : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Parcel> Parcels { get; set; }
            = new HashSet<Parcel>();
    }
}
