using System.ComponentModel.DataAnnotations;
using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.DTO
{
    public class SubsidyApplicationInsertDTO
    {
        [Required(ErrorMessage = "Subsidy type is required")]
        public SubsidyType? SubsidyType { get; set; }

        //starting with an emptyu dto ParcelIds = [], not null
        public List<int> ParcelIds { get; set; } = new();
    }
}
