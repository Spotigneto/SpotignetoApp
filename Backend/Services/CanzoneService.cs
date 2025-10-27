using Backend.Entities;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class CanzoneService : ICanzoneService
    {
        private readonly ICanzoneRepository _repository;

        public CanzoneService(ICanzoneRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CanzoneModel>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.Select(MapToModel).ToList();
        }

        public async Task<CanzoneModel?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<CanzoneModel?> GetByNameAsync(string name)
        {
            var entity = await _repository.GetByNameAsync(name);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<CanzoneModel> CreateAsync(CanzoneModel model)
        {
            var entity = MapToEntity(model);
            var created = await _repository.AddAsync(entity);
            return MapToModel(created);
        }

        public async Task<bool> UpdateAsync(long id, CanzoneModel model)
        {
            var entity = MapToEntity(model);
            entity.CaId = id;
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static CanzoneModel MapToModel(CanzoneEntity e)
        {
            return new CanzoneModel
            {
                Id = e.CaId,
                Nome = e.CaNome,
                File = e.CaFile,
                GenereId = e.CaGenere,
                SottogenereId = e.CaSottogenere,
                Durata = e.CaDurata,
                Secondi = e.CaSecondi
            };
        }

        private static CanzoneEntity MapToEntity(CanzoneModel m)
        {
            return new CanzoneEntity
            {
                CaId = m.Id,
                CaNome = m.Nome,
                CaFile = m.File,
                CaGenere = m.GenereId,
                CaSottogenere = m.SottogenereId,
                CaDurata = m.Durata,
                CaSecondi = m.Secondi
            };
        }
    }
}