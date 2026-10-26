using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.Data
{
    public class Parcel : BaseEntity
    {
        public int Id { get; set; }


        public string ParcelCode { get; set; } = null!;

        public decimal Area { get; set; }

        public CropType CropType { get; set; }

        public OwnershipType OwnershipType { get; set; }

        public int FarmerId { get; set; }

        public virtual Farmer Farmer { get; set; } = null!;

        public int RegionalUnitId { get; set; }

        public virtual RegionalUnit RegionalUnit { get; set; } = null!;

        public virtual ICollection<ApplicationParcel> ApplicationParcels { get; set; }  = new HashSet<ApplicationParcel>();

    }
}
