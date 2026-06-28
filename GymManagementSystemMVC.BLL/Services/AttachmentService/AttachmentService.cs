using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GymManagementSystemMVC.BLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AttachmentService> _logger;

        public AttachmentService(IWebHostEnvironment env, ILogger<AttachmentService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public bool Delete(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName))
                return false;

            var filePath = GetSafePath(fileName, folderName);
            if (!File.Exists(filePath))
                return false;

            File.Delete(filePath);
            return true;
        }

        public (Stream stream, string ContentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName))
                return null;

            var filePath = GetSafePath(fileName, folderName);
            if (!File.Exists(filePath))
                return null;

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            return (File.OpenRead(filePath), contentType);
        }

        public async Task<string?> UploadAsync(IFormFile? file, string folderName, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0)
                return null;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Rejected upload with extension {Extension}", extension);
                return null;
            }

            if (file.Length > MaxFileSize)
            {
                _logger.LogWarning("Rejected upload {FileName} because it exceeded the max size", file.FileName);
                return null;
            }

            var directory = GetSafeDirectory(folderName);
            Directory.CreateDirectory(directory);

            var savedFileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(directory, savedFileName);

            await using var output = File.Create(filePath);
            await file.CopyToAsync(output, ct);

            return savedFileName;
        }

        private string GetSafeDirectory(string folderName)
        {
            var safeFolderName = Path.GetFileName(folderName);
            return Path.Combine(_env.WebRootPath, "images", safeFolderName);
        }

        private string GetSafePath(string fileName, string folderName)
        {
            var safeFileName = Path.GetFileName(fileName);
            return Path.Combine(GetSafeDirectory(folderName), safeFileName);
        }
    }
}
