using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebMusic.Web.Services
{
    /// <summary>
    /// Service xử lý upload file
    /// </summary>
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileUploadService> _logger;
        private readonly string _uploadsPath;

        // Các định dạng file được phép
        private readonly string[] _allowedAudioExtensions = { ".mp3", ".wav", ".m4a", ".aac", ".ogg", ".flac" };
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

        // Kích thước file tối đa (50MB cho audio, 10MB cho image)
        private const long MaxAudioSize = 50 * 1024 * 1024; // 50MB
        private const long MaxImageSize = 10 * 1024 * 1024;  // 10MB

        public FileUploadService(IWebHostEnvironment environment, ILogger<FileUploadService> logger)
        {
            _environment = environment;
            _logger = logger;
            _uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
        }

        public async Task<string> UploadAudioAsync(IFormFile file, int userId)
        {
            try
            {
                // Validate file
                var validation = ValidateAudioFile(file);
                if (!validation.IsValid)
                {
                    throw new ArgumentException(validation.ErrorMessage);
                }

                // Tạo thư mục nếu chưa có
                var audioPath = Path.Combine(_uploadsPath, "audio", userId.ToString());
                Directory.CreateDirectory(audioPath);

                // Tạo tên file unique
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(audioPath, fileName);

                // Upload file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Trả về URL
                var relativePath = Path.Combine("uploads", "audio", userId.ToString(), fileName).Replace("\\", "/");
                _logger.LogInformation("Audio file uploaded successfully: {FilePath}", relativePath);
                return $"/{relativePath}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading audio file for user {UserId}", userId);
                throw;
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file, int userId, string folder = "general")
        {
            try
            {
                // Validate file
                var validation = ValidateImageFile(file);
                if (!validation.IsValid)
                {
                    throw new ArgumentException(validation.ErrorMessage);
                }

                // Tạo thư mục nếu chưa có
                var imagePath = Path.Combine(_uploadsPath, "images", folder, userId.ToString());
                Directory.CreateDirectory(imagePath);

                // Tạo tên file unique
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(imagePath, fileName);

                // Upload file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Trả về URL
                var relativePath = Path.Combine("uploads", "images", folder, userId.ToString(), fileName).Replace("\\", "/");
                _logger.LogInformation("Image file uploaded successfully: {FilePath}", relativePath);
                return $"/{relativePath}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image file for user {UserId} in folder {Folder}", userId, folder);
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                    return false;

                // Loại bỏ "/" đầu nếu có
                var relativePath = fileUrl.TrimStart('/');
                var fullPath = Path.Combine(_environment.WebRootPath, relativePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation("File deleted successfully: {FilePath}", fullPath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FileUrl}", fileUrl);
                return false;
            }
        }

        public (bool IsValid, string ErrorMessage) ValidateAudioFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, "File không được để trống");

            if (file.Length > MaxAudioSize)
                return (false, $"File quá lớn. Kích thước tối đa: {GetFileSizeString(MaxAudioSize)}");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedAudioExtensions.Contains(extension))
                return (false, $"Định dạng file không được hỗ trợ. Các định dạng được phép: {string.Join(", ", _allowedAudioExtensions)}");

            return (true, string.Empty);
        }

        public (bool IsValid, string ErrorMessage) ValidateImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, "File không được để trống");

            if (file.Length > MaxImageSize)
                return (false, $"File quá lớn. Kích thước tối đa: {GetFileSizeString(MaxImageSize)}");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedImageExtensions.Contains(extension))
                return (false, $"Định dạng file không được hỗ trợ. Các định dạng được phép: {string.Join(", ", _allowedImageExtensions)}");

            return (true, string.Empty);
        }

        public string GetFileSizeString(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
