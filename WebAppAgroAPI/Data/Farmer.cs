namespace WebAppAgroAPI.Data
{
    public class Farmer : BaseEntity
    {
        public int Id { get; set; }

        public string TaxIdentificationNumber { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<Parcel> Parcels { get; set; } = new HashSet<Parcel>();

        public virtual ICollection<SubsidyApplication> SubsidyApplications { get; set; } = new HashSet<SubsidyApplication>();
    }
}
