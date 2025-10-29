using Backend.Entities;

namespace Backend.Repositories
{
    public interface IArtistaRepository
    {
        Task<List<ArtistaEntity>> GetAllAsync();
        Task<ArtistaEntity?> GetByIdAsync(long id);
        Task<ArtistaEntity?> GetByNameAsync(string name);
        Task<ArtistaEntity> AddAsync(ArtistaEntity entity);
        Task<bool> UpdateAsync(ArtistaEntity entity);
        Task<bool> DeleteAsync(long id);
    }
}