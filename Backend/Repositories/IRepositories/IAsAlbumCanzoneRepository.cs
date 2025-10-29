using Backend.Entities;

namespace Backend.Repositories
{
    public interface IAsAlbumCanzoneRepository
    {
        Task<List<AsAlbumCanzoneEntity>> GetAllAsync();
        Task<AsAlbumCanzoneEntity?> GetByIdAsync(long id);
        Task<List<AsAlbumCanzoneEntity>> GetByAlbumIdAsync(long albumId);
        Task<List<AsAlbumCanzoneEntity>> GetByCanzoneIdAsync(long canzoneId);
        Task<AsAlbumCanzoneEntity> AddAsync(AsAlbumCanzoneEntity entity);
        Task<bool> UpdateAsync(AsAlbumCanzoneEntity entity);
        Task<bool> DeleteAsync(long id);
    }
}