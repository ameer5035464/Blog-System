using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BlogSystem.BLL.Contracts
{
    public interface IPhotoService
    {
        Task<ImageUploadResult?> UploadImageAsync(IFormFile image);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
