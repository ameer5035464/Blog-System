using BlogSystem.BLL.DtoModels.AccountDtos;
using Microsoft.AspNetCore.Http;

namespace BlogSystem.BLL.Contracts
{
    public interface IAccountService
    {
        Task<UserDto> LoginAsync(UserLoginDto userLogin);
        Task<UserDto> RegisterAsync(UserRegisterDto userRegister);
        Task<ResultDto> ChangePassword(ChangePasswordDto changePassword, string currentUserId);
        Task<UserDto> EditUserProfile(UserEditDto editDto, string currentUserId);
        Task<string> EditProfileImage(string currentUserId, IFormFile image, string? deletePhoto);
        Task<string> BlockUser(string userId, int blockPeriod, string blockStatus);
        Task<ResultDto> DeleteAccount(string currentUserId, string toDeleteUserId);
    }
}
