using Backend.Entities;

namespace Backend.Repositories
{
    public interface IProfileRepository
    {
        Task<List<ProfileEntity>> GetAllAsync();
        Task<ProfileEntity?> GetByIdAsync(long id);
        Task<ProfileEntity> AddAsync(ProfileEntity entity);
        Task<bool> UpdateAsync(ProfileEntity entity);
        Task<bool> DeleteAsync(long id);
    }
}