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
    public class SubsidyApplicationController : ControllerBase
    {
        private readonly ISubsidyApplicationService _subsidyApplicationService;

        public SubsidyApplicationController(ISubsidyApplicationService subsidyApplicationService)
        {
            _subsidyApplicationService = subsidyApplicationService;
        }
        [Authorize(Roles = "Farmer")]
        [HttpPost]
        public async Task<IActionResult> SubmitApplication(SubsidyApplicationInsertDTO subsidyApplicationInsertDTO)
        {
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }
            SubsidyApplicationResponseDTO result =  await _subsidyApplicationService.SubmitApplicationAsync(userId, subsidyApplicationInsertDTO);

            return StatusCode(201, new
            {   result,
                message = "application submitted successfully"
            });
        }

        [HttpPost("{applicationId:int}/evaluate")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> EvaluateApplication(int applicationId)
        {
            SubsidyApplicationResponseDTO responseDTO = await _subsidyApplicationService.EvaluateApplicationAsync(applicationId);
            return Ok(responseDTO);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("farmer/{farmerId}")]
        public async Task<IActionResult> GetApplicationsByFarmerId(int farmerId)
        {
            var applications = await _subsidyApplicationService.GetApplicationsByFarmerIdAsync(farmerId);
            return Ok(applications);
        }

        [Authorize(Roles = "Farmer")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyApplications()
        {
            string? userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var applications = await _subsidyApplicationService.GetMyApplicationsAsync(userId);
            return Ok(applications);
        }

    }
}
