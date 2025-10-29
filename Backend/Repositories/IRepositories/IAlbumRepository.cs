using Backend.Entities;

namespace Backend.Repositories
{
    public interface IAlbumRepository
    {
        Task<List<AlbumEntity>> GetAllAsync();
        Task<AlbumEntity?> GetByIdAsync(string id);
        Task<AlbumEntity?> GetByNameAsync(string name);
        Task<AlbumEntity> AddAsync(AlbumEntity entity);
        Task<bool> UpdateAsync(AlbumEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}