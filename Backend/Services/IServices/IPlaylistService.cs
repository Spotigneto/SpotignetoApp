using Backend.Models;

namespace Backend.Services
{
    public interface IPlaylistService
    {
        Task<List<PlaylistModel>> GetAllAsync();
        Task<PlaylistModel?> GetByIdAsync(string id);
        Task<PlaylistModel?> GetByNameAsync(string name);
        Task<PlaylistModel> CreateAsync(PlaylistModel model);
        Task<bool> UpdateAsync(string id, PlaylistModel model);
        Task<bool> DeleteAsync(string id);
    }
}