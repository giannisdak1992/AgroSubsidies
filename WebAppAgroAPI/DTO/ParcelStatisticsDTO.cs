using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.DTO
{
    public class ParcelStatisticsDTO
    {
        public int RegionalUnitId {get; set;}

        public string RegionalUnitName { get; set; } = null!;

        public CropType CropType {get; set;}

        public int ParcelCount { get; set;}

        public decimal TotalArea {get; set;}
    }
}
