using Backend.Entities;

namespace Backend.Repositories
{
    public interface IAsAlbumCanzoneRepository
    {
        Task<List<AsAlbumCanzoneEntity>> GetAllAsync();
        Task<AsAlbumCanzoneEntity?> GetByIdAsync(long id);
        Task<List<AsAlbumCanzoneEntity>> GetByAlbumIdAsync(string albumId);
        Task<List<AsAlbumCanzoneEntity>> GetByCanzoneIdAsync(string canzoneId);
        Task<AsAlbumCanzoneEntity> AddAsync(AsAlbumCanzoneEntity entity);
        Task<bool> UpdateAsync(AsAlbumCanzoneEntity entity);
        Task<bool> DeleteAsync(long id);
    }
}