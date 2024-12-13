using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.DtoModels.AccountDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSystem.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AccountController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDto userRegisterDto)
        {
            var user = await _serviceManager.AccountService.RegisterAsync(userRegisterDto);
            return Ok(new { Email = user.Email, Token = user.Token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var user = await _serviceManager.AccountService.LoginAsync(userLoginDto);

            return Ok(new { Token = user.Token });
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangeUserPassword(ChangePasswordDto changePassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var password = await _serviceManager.AccountService.ChangePassword(changePassword, userId!);

            return password.Success == null ? BadRequest(new { Errors = password.Errors })
                                            : Ok(new { message = password.Success });
        }

        [Authorize]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateUserProfile(UserEditDto userEditDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var user = await _serviceManager.AccountService.EditUserProfile(userEditDto, currentUserId!);

            return user.Errors == null ? Ok(new { UserName = user.UserName, Email = user.Email, PhoneNumber = user.PhoneNumber })
                                       : BadRequest(new { Errors = user.Errors });

        }

        [Authorize]
        [HttpPost("UpdateProfilePhoto")]
        public async Task<IActionResult> UpdateUserProfilePhoto(IFormFile image, string? deletePhoto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.PrimarySid);
            var photo = await _serviceManager.AccountService.EditProfileImage(currentUserId!, image, deletePhoto);

            return Ok(new { message = $"{photo}" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Block/{userId}/{blockStatus}")]
        public async Task<IActionResult> BlockUser(string userId, int? blockPeriod, string blockStatus)
        {
            var result = await _serviceManager.AccountService.BlockUser(userId, blockPeriod ?? 5, blockStatus);

            return Ok(new { message = $"Account {result} Successfully!" });
        }

        [Authorize(Roles = "Admin,Editor")]
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.PrimarySid);

            var result = await _serviceManager.AccountService.DeleteAccount(currentUser!, userId);

            return result.Success == null ? BadRequest(new { Errors = result.Errors })
                                          : Ok(new { message = result.Success });
        }

    }
}
