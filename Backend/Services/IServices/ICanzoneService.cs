using Backend.Models;

namespace Backend.Services
{
    public interface ICanzoneService
    {
        Task<List<CanzoneModel>> GetAllAsync();
        Task<CanzoneModel?> GetByIdAsync(long id);
        Task<CanzoneModel?> GetByNameAsync(string name);
        Task<CanzoneModel> CreateAsync(CanzoneModel model);
        Task<bool> UpdateAsync(long id, CanzoneModel model);
        Task<bool> DeleteAsync(long id);
    }
}