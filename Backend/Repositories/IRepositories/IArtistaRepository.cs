using Backend.Entities;

namespace Backend.Repositories
{
    public interface IArtistaRepository
    {
        Task<List<ArtistaEntity>> GetAllAsync();
        Task<ArtistaEntity?> GetByIdAsync(string id);
        Task<ArtistaEntity?> GetByNameAsync(string name);
        Task<ArtistaEntity> AddAsync(ArtistaEntity entity);
        Task<bool> UpdateAsync(ArtistaEntity entity);
        Task<bool> DeleteAsync(string id);
    }
}