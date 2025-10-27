using Backend.Entities;

namespace Backend.Repositories
{
    public interface ICanzoneRepository
    {
        Task<List<CanzoneEntity>> GetAllAsync();
        Task<CanzoneEntity?> GetByIdAsync(long id);
        Task<CanzoneEntity?> GetByNameAsync(string name);
        Task<CanzoneEntity> AddAsync(CanzoneEntity entity);
        Task<bool> UpdateAsync(CanzoneEntity entity);
        Task<bool> DeleteAsync(long id);
    }
}