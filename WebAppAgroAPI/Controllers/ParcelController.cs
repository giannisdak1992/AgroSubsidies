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
    public class ParcelController : ControllerBase
    {
        private readonly IParcelService _parcelService;

        public ParcelController (IParcelService parcelService)
        {
            _parcelService = parcelService;
        }

        [Authorize(Roles = "Farmer")]
        [HttpPost]
        public async Task<IActionResult> AddParcel(ParcelInsertDTO parcelInsertDTO)
        {
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId)) // if string userIdClaim cannot be converted  to int
            {
                return Unauthorized();
            }
            await _parcelService.AddParcelAsync(userId, parcelInsertDTO);

            return StatusCode(201, new
            {
                message = "Parcel created successfully"
            });
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("farmer/{farmerId}")] //api/parcel/farmer/1
        public async Task<IActionResult> GetParcelsByFarmerId(int farmerId)
        {
            var parcels = await _parcelService.GetParcelsByFarmerIdAsync(farmerId);
            return Ok(parcels);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("{parcelId}")] //api/parcel/1
        public async Task<IActionResult> GetParcelWithDetails(int parcelId)
        {
           var parcel = await _parcelService.GetParcelWithDetailsAsync(parcelId);
            return Ok(parcel);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> GetAllParcels ( [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] ParcelFiltersDTO parcelFiltersDTO)
        {
            var parcels = await _parcelService.GetAllParcelsFilteredPaginatedAsync(pageNumber, pageSize, parcelFiltersDTO);
            return Ok(parcels);
        }

        [Authorize(Roles = "Farmer")]
        [HttpGet("my/{parcelId}")]
        public async Task<IActionResult> GetMyParcelWithDetails(int parcelId)
        {
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId)) // if string userIdClaim cannot be converted  to int
            {
                return Unauthorized();
            }

            var parcelDetails =  await _parcelService.GetMyParcelWithDetailsAsync(userId, parcelId);
            return Ok(parcelDetails);
        }

        [Authorize(Roles = "Farmer")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyParcels()
        {
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }
            var parcels = await _parcelService.GetMyParcelsAsync(userId);
            return Ok(parcels);
        }
        [Authorize(Roles = "Employee")]
        [HttpGet("statistics")]
        public async Task<IActionResult> GetParcelStatistics()
        {
            var statistics = await _parcelService.GetParcelStatisticsAsync();
            return Ok(statistics);
        }
    }
}
