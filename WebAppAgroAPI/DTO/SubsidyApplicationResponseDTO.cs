using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.DTO
{
    public class SubsidyApplicationResponseDTO
    {
        public int Id { get; set; }
        public int FarmerId { get; set; }
        public SubsidyType SubsidyType { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; }

        public DateTime? DecisionDate { get; set; }

        public string? RejectionReason { get; set; }


    }
}
