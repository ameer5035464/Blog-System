using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.DtoModels.AccountDtos;
using BlogSystem.BLL.GlobalExceptions.ExceptionModels;
using BlogSystem.BLL.helpers;
using BlogSystem.DAL.Contracts;
using BlogSystem.DAL.Entities;
using BlogSystem.DAL.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogSystem.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPhotoService photoService,
            IOptions<JwtSettings> jwtOptions,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _photoService = photoService;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<UserDto> LoginAsync(UserLoginDto userLogin)
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email) ?? throw new UnauthorizedException("this email is not exist!");

            if (user.IsBlocked)
            {
                if (user.BlockPeriod.HasValue && user.BlockPeriod > DateTime.UtcNow)
                {
                    throw new UnauthorizedException($"Your accout is blocked until {user.BlockPeriod}");
                }
                user.IsBlocked = false;
                user.BlockPeriod = null;
            }

            var signInUser = await _signInManager.PasswordSignInAsync(user, userLogin.Password, isPersistent: userLogin.RememberMe, lockoutOnFailure: false);

            if (signInUser.Succeeded)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = userLogin.Email,
                    Token = await CreateJwtToken(user)
                };
                return userDto;
            }
            else if (signInUser.IsLockedOut)
            {
                throw new UnauthorizedException("your account is locked");
            }
            else if (signInUser.IsNotAllowed)
            {
                throw new UnauthorizedException("your account is not allowed to signin yet");
            }
            else if (signInUser.RequiresTwoFactor)
            {
                throw new UnauthorizedException("your account is not verfied yet");
            }
            throw new UnauthorizedException("invalid Login");
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto userRegister)
        {
            var user = await _userManager.FindByEmailAsync(userRegister.Email);

            if (user == null)
            {
                var image = await _photoService.UploadImageAsync(userRegister.Image!);

                var newUser = new ApplicationUser
                {
                    Email = userRegister.Email,
                    PicturePublicId = image?.PublicId,
                    PictureUrl = image?.Url.ToString(),
                    UserName = $"{userRegister.FirstName}{userRegister.LastName}",
                    PhoneNumber = userRegister.PhoneNumber
                };

                var createUser = await _userManager.CreateAsync(newUser!, userRegister.Password);

                if (createUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "Editor");

                    var userDto = new UserDto
                    {
                        Id = newUser.Id,
                        Email = newUser.Email,
                        Token = await CreateJwtToken(newUser)
                    };
                    newUser.Token = userDto.Token;

                    return userDto;
                }
                else
                {
                    var errors = createUser.Errors.Select(err => err.Description).ToArray();

                    throw new RegisterAccountException("failed to regiser") { Errors = errors };
                }

            }

            throw new CustomConflictException("account already registerd");
        }

        public async Task<UserDto> EditUserProfile(UserEditDto editDto, string currentUserId)
        {
            var user = await _userManager.FindByIdAsync(currentUserId) ?? throw new CustomBadRequest("No user with this Id");

            if (user.Id == editDto.Id || await _userManager.IsInRoleAsync(user, "Admin"))
            {
                if (user.Id != editDto.Id)
                {
                    var updateUser = await _userManager.FindByIdAsync(editDto.Id) ?? throw new CustomBadRequest("No user with this Id");

                    updateUser.Id = editDto.Id;

                    if (!string.IsNullOrEmpty(editDto.Email))
                    {
                        updateUser.Email = editDto.Email;
                    }
                    if (!string.IsNullOrEmpty(editDto.PhoneNumber))
                    {
                        updateUser.PhoneNumber = editDto.PhoneNumber;
                    }
                    if (!string.IsNullOrEmpty(editDto.FirstName) && !string.IsNullOrEmpty(editDto.LastName))
                    {
                        updateUser.UserName = $"{editDto.FirstName}_{editDto.LastName}";
                    }

                    var result = await _userManager.UpdateAsync(updateUser);

                    if (result.Succeeded)
                    {
                        var returnDto = new UserDto
                        {
                            Id = editDto.Id,
                            UserName = updateUser.UserName!,
                            Email = updateUser.Email!,
                            PhoneNumber = updateUser.PhoneNumber!
                        };

                        return returnDto;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(editDto.Email))
                    {
                        user.Email = editDto.Email;
                    }
                    if (!string.IsNullOrEmpty(editDto.PhoneNumber))
                    {
                        user.PhoneNumber = editDto.PhoneNumber;
                    }
                    if (!string.IsNullOrEmpty(editDto.FirstName) && !string.IsNullOrEmpty(editDto.LastName))
                    {
                        user.UserName = $"{editDto.FirstName}_{editDto.LastName}";
                    }

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        var returnDto = new UserDto
                        {
                            Id = editDto.Id,
                            UserName = user.UserName!,
                            Email = user.Email!,
                            PhoneNumber = user.PhoneNumber!
                        };

                        return returnDto;
                    }
                    if (!result.Succeeded)
                    {
                        var errors = result.Errors.Select(err => err.Description).ToList();
                        var returnDto = new UserDto
                        {
                            Errors = errors
                        };

                        return returnDto;
                    }
                }
            }
            throw new UnauthorizedException("you cannot edit other's profile");
        }

        public async Task<string> EditProfileImage(string currentUserId, IFormFile image, string? deletePhoto)
        {
            var user = await _userManager.FindByIdAsync(currentUserId);
            var oldPhotoPublicId = user!.PicturePublicId;

            if (deletePhoto != "DeletePhoto" || oldPhotoPublicId == null)
            {
                var updatePhoto = await _photoService.UploadImageAsync(image);

                user!.PictureUrl = updatePhoto!.Url.ToString();
                user!.PicturePublicId = updatePhoto!.PublicId;

            }
            else
            {
                user!.PictureUrl = null;
                user!.PicturePublicId = null;
            }
            if (oldPhotoPublicId != null)
                await _photoService.DeleteImageAsync(oldPhotoPublicId!);

            var updateUser = await _userManager.UpdateAsync(user);

            if (updateUser.Succeeded)
            {
                return user.PictureUrl == null ? "Photo Deleted Succefully" : "Photo Updated Succefully";
            }
            throw new CustomBadRequest("something wrong happen");
        }

        public async Task<string> BlockUser(string userId, int blockPeriod, string blockStatus)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new CustomBadRequest("no user with this id");

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                if (blockStatus == "Block")
                {
                    user.IsBlocked = true;
                    user.BlockPeriod = DateTime.UtcNow.AddDays(blockPeriod);

                    await _userManager.UpdateAsync(user);
                    return "Blocked";
                }
                else if (blockStatus == "UnBlock")
                {
                    user.IsBlocked = false;
                    user.BlockPeriod = null;

                    await _userManager.UpdateAsync(user);
                    return "UnBlocked";
                }
                throw new CustomBadRequest("invalid Block Action");
            }
            else
                throw new UnauthorizedException("Cant Block user with Admin Role!");
        }

        public async Task<ResultDto> ChangePassword(ChangePasswordDto changePassword, string currentUserId)
        {
            var user = await _userManager.FindByIdAsync(currentUserId);
            var result = await _userManager.ChangePasswordAsync(user!, changePassword.OldPassword, changePassword.NewPassword);
            var getResult = new ResultDto();

            if (result.Succeeded)
            {
                getResult.Success = "Password Changed Succefully";
                return getResult;
            }

            var errors = result.Errors.Select(err => err.Description).ToList();
            getResult.Errors = errors;
            return getResult;
        }

        public async Task<ResultDto> DeleteAccount(string currentUserId, string toDeleteUserId)
        {
            var user = await _userManager.FindByIdAsync(currentUserId);

            if (currentUserId == toDeleteUserId || await _userManager.IsInRoleAsync(user!, "Admin"))
            {
                var posts = await _unitOfWork.GetRepository<BlogPost>().GetAllAsync();
                var filterPosts = posts.Where(P => P.AuthorId == toDeleteUserId).ToList();
                var deleteUser = await _userManager.FindByIdAsync(toDeleteUserId);
                var result = await _userManager.DeleteAsync(deleteUser!);
                var getResult = new ResultDto();

                if (result.Succeeded)
                {
                    foreach (var item in filterPosts)
                    {
                        item.Status = Status.Archived;
                        _unitOfWork.GetRepository<BlogPost>().Update(item);
                    }
                    await _unitOfWork.CompleteAsync();
                    await _unitOfWork.DisposeAsync();

                    getResult.Success = "User Deleted";
                    return getResult;
                }
                else
                {
                    var errors = result.Errors.Select(err => err.Description).ToList();
                    getResult.Errors = errors;
                    return getResult;
                }

            }
            throw new UnauthorizedException("you are not valid to delete this accout");
        }

        #region CreateJwtToken
        public async Task<string> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesAsClaims = new List<Claim>();

            foreach (var role in userRoles)
            {
                rolesAsClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.GivenName,user.UserName!)
            }
            .Union(rolesAsClaims)
            .Union(userClaims);

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var Securitytoken = new JwtSecurityToken(
                audience: _jwtSettings.Audience,
                issuer: _jwtSettings.Issuer,
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddHours(_jwtSettings.Expire)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(Securitytoken);

            return token;
        }
        #endregion


    }
}