namespace WebAppAgroAPI.DTO
{
    public class FarmerResponseDTO
    {
        public int Id { get; set; }

        public required string Username { get; set; } 
        public required string Firstname { get; set; }

        public required string Lastname { get; set; } 

        public required string Email { get; set; } 

        public required string TaxIdentificationNumber { get; set; } 

        public DateOnly DateOfBirth { get; set; }

        public required string PhoneNumber { get; set; } 
    }
}
