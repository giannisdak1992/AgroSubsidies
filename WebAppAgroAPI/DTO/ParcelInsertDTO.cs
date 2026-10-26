using System.ComponentModel.DataAnnotations;
using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.DTO
{
    public class ParcelInsertDTO
    {
        [Required(ErrorMessage = "Parcel code is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Parcel code must be between 2 and 50 characters")]
        public string? ParcelCode { get; set; }


        [Required(ErrorMessage = "Acres must be declared and be a positive number")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Area must be greater than zero")]
        public decimal Area { get; set; }


        [Range(1, int.MaxValue,
            ErrorMessage = "Regional Unit is required")]
        public int RegionalUnitId { get; set; }

        [Required(ErrorMessage = "Crop type is required")]
        public CropType CropType { get; set; }

        [Required(ErrorMessage = "Ownership type is required")]
        public OwnershipType OwnershipType { get; set; }
    }
}
