using Backend.Entities;

namespace Backend.Repositories.IRepositories
{
    public interface IAsUtenteArtistaRepository
    {
        Task<IEnumerable<AsUtenteArtistaEntity>> GetAllAsync();
        Task<AsUtenteArtistaEntity?> GetByIdAsync(long id);
        Task<IEnumerable<AsUtenteArtistaEntity>> GetByUtenteIdAsync(long utenteId);
        Task<IEnumerable<AsUtenteArtistaEntity>> GetByArtistaIdAsync(long artistaId);
        Task<AsUtenteArtistaEntity?> GetByUtenteAndArtistaAsync(long utenteId, long artistaId);
        Task<AsUtenteArtistaEntity> CreateAsync(AsUtenteArtistaEntity entity);
        Task<AsUtenteArtistaEntity> UpdateAsync(AsUtenteArtistaEntity entity);
        Task<bool> DeleteAsync(long id);
        Task<bool> DeleteByUtenteAndArtistaAsync(long utenteId, long artistaId);
        Task<bool> ExistsAsync(long utenteId, long artistaId);
    }
}