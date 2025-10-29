using Backend.Models;

namespace Backend.Services
{
    public interface IArtistaService
    {
        Task<List<ArtistaModel>> GetAllAsync();
        Task<ArtistaModel?> GetByIdAsync(string id);
        Task<ArtistaModel?> GetByNameAsync(string name);
        Task<ArtistaModel> CreateAsync(ArtistaModel model);
        Task<bool> UpdateAsync(string id, ArtistaModel model);
        Task<bool> DeleteAsync(string id);
    }
}