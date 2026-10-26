using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.Models
{
    public class ParcelStatisticsResult
    {
        public int RegionalUnitId { get; set; }

        public string RegionalUnitName { get; set; } = null!;

        public CropType CropType { get; set; }

        public int ParcelCount { get; set; }

        public decimal TotalArea { get; set; } 


    }
}
