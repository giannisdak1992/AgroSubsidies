using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.Data
{
    public class SubsidyApplication : BaseEntity
    {
        public int Id { get; set; }

        public int FarmerId { get; set; }

        public SubsidyType SubsidyType { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; }
            = ApplicationStatus.UnderReview;

        public DateTime? DecisionDate{ get; set; } 

        public string? RejectionReason { get; set; }

        public virtual Farmer Farmer { get; set; } = null!;

        public virtual ICollection<ApplicationParcel> ApplicationParcels { get; set; }
            = new HashSet<ApplicationParcel>();

    }
}
