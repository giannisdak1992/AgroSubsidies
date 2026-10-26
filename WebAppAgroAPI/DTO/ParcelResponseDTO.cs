using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.DTO
{
    public class ParcelResponseDTO
    {
        public int Id { get ; set; }
        public string ParcelCode { get; set; } = null!;

        public decimal Area { get; set; }

        public CropType CropType { get; set; }

        public OwnershipType OwnershipType { get; set; }

        public int RegionalUnitId { get; set; }

        public string RegionalUnitName { get; set; } = null!;

        
    }
}
