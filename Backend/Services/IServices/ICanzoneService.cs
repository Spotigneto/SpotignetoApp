using Backend.Models;

namespace Backend.Services
{
    public interface ICanzoneService
    {
        Task<List<CanzoneModel>> GetAllAsync();
        Task<CanzoneModel?> GetByIdAsync(string id);
        Task<CanzoneModel?> GetByNameAsync(string name);
        Task<CanzoneModel> CreateAsync(CanzoneModel model);
        Task<bool> UpdateAsync(string id, CanzoneModel model);
        Task<bool> DeleteAsync(string id);
    }
}