namespace Backend.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly string _audioDirectory;

        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
            _audioDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio", "songs");
            
            // Ensure directory exists
            Directory.CreateDirectory(_audioDirectory);
        }

        public async Task<string> SaveMp3FileAsync(IFormFile file, long canzoneId)
        {
            try
            {
                if (!ValidateMp3File(file))
                {
                    throw new ArgumentException("Invalid MP3 file");
                }

                // Generate unique filename
                var fileName = $"{canzoneId}_{Guid.NewGuid()}.mp3";
                var filePath = Path.Combine(_audioDirectory, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation($"MP3 file saved: {fileName} for canzone {canzoneId}");
                
                // Return relative path for database storage
                return $"/audio/songs/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving MP3 file for canzone {canzoneId}");
                throw;
            }
        }

        public async Task<bool> DeleteMp3FileAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return false;

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
                
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    _logger.LogInformation($"MP3 file deleted: {filePath}");
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting MP3 file: {filePath}");
                return false;
            }
        }

        public async Task<FileStream?> GetMp3FileStreamAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return null;

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
                
                if (!File.Exists(fullPath))
                    return null;

                return await Task.FromResult(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting MP3 file stream: {filePath}");
                return null;
            }
        }

        public bool ValidateMp3File(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            // Check file extension
            if (!file.FileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                return false;

            // Check content type
            if (!file.ContentType.Equals("audio/mpeg", StringComparison.OrdinalIgnoreCase))
                return false;

            // Check file size (max 50MB)
            if (file.Length > 50 * 1024 * 1024)
                return false;

            return true;
        }

        public string GetAudioDirectory()
        {
            return _audioDirectory;
        }
    }
}