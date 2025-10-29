using Backend.Entities;

namespace Backend.Repositories
{
    public interface IPlaylistRepository
    {
        Task<List<PlaylistEntity>> GetAllAsync();
        Task<PlaylistEntity?> GetByIdAsync(string id);
        Task<PlaylistEntity?> GetByNameAsync(string name);
        Task<PlaylistEntity> AddAsync(PlaylistEntity entity);
        Task<bool> UpdateAsync(PlaylistEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}