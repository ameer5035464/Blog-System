using BlogSystem.BLL.Contracts;
using BlogSystem.BLL.GlobalExceptions.ExceptionModels;
using BlogSystem.BLL.helpers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BlogSystem.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
            {
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret,
                Cloud = config.Value.CloudName
            };
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult?> UploadImageAsync(IFormFile image)
        {
            if (image.Length > 0)
            {
                using var stream = image.OpenReadStream();
                var Imageparams = new ImageUploadParams
                {
                    File = new FileDescription(image.FileName, stream)
                };
                var result = await _cloudinary.UploadAsync(Imageparams);

                if (result.PublicId is null)
                    throw new CustomBadRequest("failed to Upload Image");

                return result;

            }
            throw new CustomBadRequest("no image selected");
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Result != "ok")
                throw new CustomBadRequest("Failed to delete image");

            return result;
            
        }
    }
}
