using Microsoft.AspNetCore.Http;

namespace WebMusic.Web.Services
{
    /// <summary>
    /// Service interface cho xử lý upload file
    /// </summary>
    public interface IFileUploadService
    {
        /// <summary>
        /// Upload file audio (mp3, wav, etc.)
        /// </summary>
        /// <param name="file">File audio</param>
        /// <param name="userId">ID người dùng</param>
        /// <returns>URL của file đã upload</returns>
        Task<string> UploadAudioAsync(IFormFile file, int userId);

        /// <summary>
        /// Upload file hình ảnh (jpg, png, gif, etc.)
        /// </summary>
        /// <param name="file">File hình ảnh</param>
        /// <param name="userId">ID người dùng</param>
        /// <param name="folder">Thư mục con (songs, albums, playlists)</param>
        /// <returns>URL của file đã upload</returns>
        Task<string> UploadImageAsync(IFormFile file, int userId, string folder = "general");

        /// <summary>
        /// Xóa file
        /// </summary>
        /// <param name="fileUrl">URL của file cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteFileAsync(string fileUrl);

        /// <summary>
        /// Validate file audio
        /// </summary>
        /// <param name="file">File cần validate</param>
        /// <returns>Kết quả validation</returns>
        (bool IsValid, string ErrorMessage) ValidateAudioFile(IFormFile file);

        /// <summary>
        /// Validate file hình ảnh
        /// </summary>
        /// <param name="file">File cần validate</param>
        /// <returns>Kết quả validation</returns>
        (bool IsValid, string ErrorMessage) ValidateImageFile(IFormFile file);

        /// <summary>
        /// Lấy kích thước file dưới dạng chuỗi đọc được
        /// </summary>
        /// <param name="bytes">Kích thước file tính bằng bytes</param>
        /// <returns>Chuỗi kích thước file</returns>
        string GetFileSizeString(long bytes);
    }
}
