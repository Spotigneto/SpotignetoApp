using Backend.Entities;

namespace Backend.Repositories.IRepositories
{
    public interface IAsUtenteArtistaRepository
    {
        Task<IEnumerable<AsUtenteArtistaEntity>> GetAllAsync();
        Task<AsUtenteArtistaEntity?> GetByIdAsync(long id);
        Task<IEnumerable<AsUtenteArtistaEntity>> GetByUtenteIdAsync(string utenteId);
        Task<IEnumerable<AsUtenteArtistaEntity>> GetByArtistaIdAsync(string artistaId);
        Task<AsUtenteArtistaEntity?> GetByUtenteAndArtistaAsync(string utenteId, string artistaId);
        Task<AsUtenteArtistaEntity> CreateAsync(AsUtenteArtistaEntity entity);
        Task<AsUtenteArtistaEntity> UpdateAsync(AsUtenteArtistaEntity entity);
        Task<bool> DeleteAsync(long id);
        Task<bool> DeleteByUtenteAndArtistaAsync(string utenteId, string artistaId);
        Task<bool> ExistsAsync(string utenteId, string artistaId);
    }
}