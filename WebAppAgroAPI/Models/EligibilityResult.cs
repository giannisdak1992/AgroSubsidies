namespace WebAppAgroAPI.Models
{
    public class EligibilityResult
    {
        public bool IsEligible { get; set; }

        public List<string> Reasons { get; set; } = new();
    }
}
