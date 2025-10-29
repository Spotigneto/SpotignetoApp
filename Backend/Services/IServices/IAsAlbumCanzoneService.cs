using Backend.Models;

namespace Backend.Services
{
    public interface IAsAlbumCanzoneService
    {
        Task<List<AsAlbumCanzoneModel>> GetAllAsync();
        Task<AsAlbumCanzoneModel?> GetByIdAsync(long id);
        Task<List<AsAlbumCanzoneModel>> GetByAlbumIdAsync(string albumId);
        Task<List<AsAlbumCanzoneModel>> GetByCanzoneIdAsync(string canzoneId);
        Task<AsAlbumCanzoneModel> CreateAsync(AsAlbumCanzoneModel model);
        Task<bool> UpdateAsync(long id, AsAlbumCanzoneModel model);
        Task<bool> DeleteAsync(long id);
    }
}