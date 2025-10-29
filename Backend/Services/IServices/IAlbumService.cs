using Backend.Models;

namespace Backend.Services
{
    public interface IAlbumService
    {
        Task<List<AlbumModel>> GetAllAsync();
        Task<AlbumModel?> GetByIdAsync(string id);
        Task<AlbumModel?> GetByNameAsync(string name);
        Task<AlbumModel> CreateAsync(AlbumModel model);
        Task<bool> UpdateAsync(string id, AlbumModel model);
        Task<bool> DeleteAsync(string id);
    }
}