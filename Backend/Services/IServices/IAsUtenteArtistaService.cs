using Backend.Models;

namespace Backend.Services.IServices
{
    public interface IAsUtenteArtistaService
    {
        Task<IEnumerable<AsUtenteArtistaModel>> GetAllAsync();
        Task<AsUtenteArtistaModel?> GetByIdAsync(long id);
        Task<IEnumerable<AsUtenteArtistaModel>> GetByUtenteIdAsync(string utenteId);
        Task<IEnumerable<AsUtenteArtistaModel>> GetByArtistaIdAsync(string artistaId);
        Task<AsUtenteArtistaModel?> GetByUtenteAndArtistaAsync(string utenteId, string artistaId);
        Task<AsUtenteArtistaModel> CreateAsync(AsUtenteArtistaModel model);
        Task<AsUtenteArtistaModel> UpdateAsync(AsUtenteArtistaModel model);
        Task<bool> DeleteAsync(long id);
        Task<bool> DeleteByUtenteAndArtistaAsync(string utenteId, string artistaId);
        Task<bool> ExistsAsync(string utenteId, string artistaId);
        Task<bool> FollowArtistaAsync(string utenteId, string artistaId);
        Task<bool> UnfollowArtistaAsync(string utenteId, string artistaId);
        Task<bool> IsFollowingAsync(string utenteId, string artistaId);
    }
}