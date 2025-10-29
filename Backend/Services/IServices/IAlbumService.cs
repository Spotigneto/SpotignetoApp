using Backend.Models;

namespace Backend.Services
{
    public interface IAlbumService
    {
        Task<List<AlbumModel>> GetAllAsync();
        Task<AlbumModel?> GetByIdAsync(long id);
        Task<AlbumModel?> GetByNameAsync(string name);
        Task<AlbumModel> CreateAsync(AlbumModel model);
        Task<bool> UpdateAsync(long id, AlbumModel model);
        Task<bool> DeleteAsync(long id);
    }
}