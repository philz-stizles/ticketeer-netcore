/*using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Ticketeer.Application.Settings;

namespace Ticketeer.Infrastructure.Services.Storage
{
    public class CloudinaryService
    {
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings,
            Microsoft.Extensions.Logging.ILogger<CloudinaryService> logger)
        {
            _cloudinarySettings = cloudinarySettings;

            Account acc = new Account(
                _cloudinarySettings.Value.CloudName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ExecutionResponse<FileUploadResponseDto>> UploadFileAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500)
                            .Height(500)
                            .Crop("fill")
                            .Gravity("face")
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }

            if (uploadResult.Error != null) return new ExecutionResponse<FileUploadResponseDto>
            {
                Status = false,
                Message = uploadResult.Error.Message,
            };

            return new ExecutionResponse<FileUploadResponseDto>
            {
                Status = true,
                Data = new FileUploadResponseDto
                {
                    Url = uploadResult.Url.ToString(),
                    PublicId = uploadResult.PublicId
                }
            };
        }
    }
}
*/