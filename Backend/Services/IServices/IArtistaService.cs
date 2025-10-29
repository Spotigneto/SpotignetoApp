using Backend.Models;

namespace Backend.Services
{
    public interface IArtistaService
    {
        Task<List<ArtistaModel>> GetAllAsync();
        Task<ArtistaModel?> GetByIdAsync(long id);
        Task<ArtistaModel?> GetByNameAsync(string name);
        Task<ArtistaModel> CreateAsync(ArtistaModel model);
        Task<bool> UpdateAsync(long id, ArtistaModel model);
        Task<bool> DeleteAsync(long id);
    }
}