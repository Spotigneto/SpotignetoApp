using Backend.Models;

namespace Backend.Services.IServices
{
    public interface IAsUtenteArtistaService
    {
        Task<IEnumerable<AsUtenteArtistaModel>> GetAllAsync();
        Task<AsUtenteArtistaModel?> GetByIdAsync(long id);
        Task<IEnumerable<AsUtenteArtistaModel>> GetByUtenteIdAsync(long utenteId);
        Task<IEnumerable<AsUtenteArtistaModel>> GetByArtistaIdAsync(long artistaId);
        Task<AsUtenteArtistaModel?> GetByUtenteAndArtistaAsync(long utenteId, long artistaId);
        Task<AsUtenteArtistaModel> CreateAsync(AsUtenteArtistaModel model);
        Task<AsUtenteArtistaModel> UpdateAsync(AsUtenteArtistaModel model);
        Task<bool> DeleteAsync(long id);
        Task<bool> DeleteByUtenteAndArtistaAsync(long utenteId, long artistaId);
        Task<bool> ExistsAsync(long utenteId, long artistaId);
        Task<bool> FollowArtistaAsync(long utenteId, long artistaId);
        Task<bool> UnfollowArtistaAsync(long utenteId, long artistaId);
        Task<bool> IsFollowingAsync(long utenteId, long artistaId);
    }
}