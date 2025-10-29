using Backend.Entities;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class ArtistaService : IArtistaService
    {
        private readonly IArtistaRepository _repo;

        public ArtistaService(IArtistaRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ArtistaModel>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(MapToModel).ToList();
        }

        public async Task<ArtistaModel?> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<ArtistaModel?> GetByNameAsync(string name)
        {
            var entity = await _repo.GetByNameAsync(name);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<ArtistaModel> CreateAsync(ArtistaModel model)
        {
            var entity = new ArtistaEntity { ArNome = model.Nome };
            var saved = await _repo.AddAsync(entity);
            return MapToModel(saved);
        }

        public async Task<bool> UpdateAsync(string id, ArtistaModel model)
        {
            var entity = new ArtistaEntity
            {
                ArId = id,
                ArNome = model.Nome
            };
            return await _repo.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repo.DeleteAsync(id);
        }

        private static ArtistaModel MapToModel(ArtistaEntity e)
        {
            return new ArtistaModel
            {
                Id = e.ArId,
                Nome = e.ArNome
            };
        }
    }
}