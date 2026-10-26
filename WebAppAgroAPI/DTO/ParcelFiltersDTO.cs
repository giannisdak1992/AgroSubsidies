using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.DTO
{
    public class ParcelFiltersDTO
    {
        public CropType? CropType { get; set; }

        public OwnershipType? OwnershipType { get; set; }

        public int? RegionalUnitId { get; set; }
    }
}
