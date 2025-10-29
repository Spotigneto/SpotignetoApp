namespace Backend.Services
{
    public interface IFileService
    {
        Task<string> SaveMp3FileAsync(IFormFile file, long canzoneId);
        Task<bool> DeleteMp3FileAsync(string filePath);
        Task<FileStream?> GetMp3FileStreamAsync(string filePath);
        bool ValidateMp3File(IFormFile file);
        string GetAudioDirectory();
    }
}