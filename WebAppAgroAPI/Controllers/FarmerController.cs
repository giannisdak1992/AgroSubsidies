using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppAgroAPI.DTO;
using WebAppAgroAPI.Service;

namespace WebAppAgroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FarmerController : ControllerBase
    {
        private readonly IFarmerService _farmerService;

        public FarmerController(IFarmerService farmerService)
        {
            _farmerService = farmerService;
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("{farmerId}")] //api/farmer/1
        public async Task<IActionResult> GetFarmerWithUser(int farmerId)
        {
            var farmer = await _farmerService.GetFarmerWithUserAsync(farmerId);
            return Ok(farmer);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("taxid/{taxId}")]
        public async Task<IActionResult> GetFarmerByTaxId(string taxId)
        {
            var farmer = await _farmerService.GetFarmerByTaxIdAsync(taxId);
            return Ok(farmer);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> GetAllFarmers([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] FarmerFiltersDTO farmerFiltersDTO)
        {
            var farmers = await _farmerService.GetAllFarmersFilteredPaginatedAsync(pageNumber, pageSize, farmerFiltersDTO);
            return Ok(farmers);
        }

        [Authorize(Roles = "Farmer")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyFarmer()
        {
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId)) // if string userIdClaim cannot be converted  to int
            {
                return Unauthorized();
            }
            var farmer = await _farmerService.GetFarmerByUserIdAsync(userId);
            return Ok(farmer);
        }
    }
}
