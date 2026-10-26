namespace WebAppAgroAPI.Data
{
    public class ApplicationParcel : BaseEntity
    {
        public int SubsidyApplicationId { get; set; }

        public int ParcelId { get; set; }

        public virtual SubsidyApplication SubsidyApplication { get; set; } = null!;

        public virtual Parcel Parcel { get; set; } = null!;
    }
}
