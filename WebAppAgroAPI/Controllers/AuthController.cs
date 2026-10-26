using Microsoft.AspNetCore.Mvc;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.DTO;
using WebAppAgroAPI.Service;

namespace WebAppAgroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IFarmerService _farmerService;
        public AuthController(IUserService userService, IJwtService jwtService, IFarmerService farmerService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _farmerService = farmerService;
        }

        [HttpPost("login")] //   api/auth/login //

        public async Task<IActionResult> Login(UserLoginDTO credentials)
        {
            User? user = await _userService.VerifyAndGetUserAsync(credentials);

            if (user == null)
            {
                return Unauthorized();
            }

            string token = _jwtService.GenerateToken(user);

            return Ok( new { token }); // { "token" : ".........." } //
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(FarmerSignupDTO farmerSignupDTO)
        {
            FarmerResponseDTO farmer = await _farmerService.RegisterFarmerAsync(farmerSignupDTO);

            return StatusCode(201, new
            {
                message = "Farmer registered successfully"
            });
        }
    }
}
