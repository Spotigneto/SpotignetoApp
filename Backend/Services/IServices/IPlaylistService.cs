using Backend.Models;

namespace Backend.Services
{
    public interface IPlaylistService
    {
        Task<List<PlaylistModel>> GetAllAsync();
        Task<PlaylistModel?> GetByIdAsync(long id);
        Task<PlaylistModel?> GetByNameAsync(string name);
        Task<PlaylistModel> CreateAsync(PlaylistModel model);
        Task<bool> UpdateAsync(long id, PlaylistModel model);
        Task<bool> DeleteAsync(long id);
    }
}