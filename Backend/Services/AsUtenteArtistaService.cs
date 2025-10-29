using Backend.Entities;
using Backend.Models;
using Backend.Repositories.IRepositories;
using Backend.Services.IServices;

namespace Backend.Services
{
    public class AsUtenteArtistaService : IAsUtenteArtistaService
    {
        private readonly IAsUtenteArtistaRepository _repository;

        public AsUtenteArtistaService(IAsUtenteArtistaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AsUtenteArtistaModel>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToModel);
        }

        public async Task<AsUtenteArtistaModel?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? MapToModel(entity) : null;
        }

        public async Task<IEnumerable<AsUtenteArtistaModel>> GetByUtenteIdAsync(long utenteId)
        {
            var entities = await _repository.GetByUtenteIdAsync(utenteId);
            return entities.Select(MapToModel);
        }

        public async Task<IEnumerable<AsUtenteArtistaModel>> GetByArtistaIdAsync(long artistaId)
        {
            var entities = await _repository.GetByArtistaIdAsync(artistaId);
            return entities.Select(MapToModel);
        }

        public async Task<AsUtenteArtistaModel?> GetByUtenteAndArtistaAsync(long utenteId, long artistaId)
        {
            var entity = await _repository.GetByUtenteAndArtistaAsync(utenteId, artistaId);
            return entity != null ? MapToModel(entity) : null;
        }

        public async Task<AsUtenteArtistaModel> CreateAsync(AsUtenteArtistaModel model)
        {
            var entity = MapToEntity(model);
            var createdEntity = await _repository.CreateAsync(entity);
            return MapToModel(createdEntity);
        }

        public async Task<AsUtenteArtistaModel> UpdateAsync(AsUtenteArtistaModel model)
        {
            var entity = MapToEntity(model);
            var updatedEntity = await _repository.UpdateAsync(entity);
            return MapToModel(updatedEntity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> DeleteByUtenteAndArtistaAsync(long utenteId, long artistaId)
        {
            return await _repository.DeleteByUtenteAndArtistaAsync(utenteId, artistaId);
        }

        public async Task<bool> ExistsAsync(long utenteId, long artistaId)
        {
            return await _repository.ExistsAsync(utenteId, artistaId);
        }

        public async Task<bool> FollowArtistaAsync(long utenteId, long artistaId)
        {
            // Verifica se la relazione esiste già
            if (await _repository.ExistsAsync(utenteId, artistaId))
                return false; // Già seguito

            var entity = new AsUtenteArtistaEntity
            {
                AsuaUtenteFk = utenteId,
                AsuaArtistaFk = artistaId
            };

            await _repository.CreateAsync(entity);
            return true;
        }

        public async Task<bool> UnfollowArtistaAsync(long utenteId, long artistaId)
        {
            return await _repository.DeleteByUtenteAndArtistaAsync(utenteId, artistaId);
        }

        public async Task<bool> IsFollowingAsync(long utenteId, long artistaId)
        {
            return await _repository.ExistsAsync(utenteId, artistaId);
        }

        private static AsUtenteArtistaModel MapToModel(AsUtenteArtistaEntity entity)
        {
            return new AsUtenteArtistaModel
            {
                Id = entity.AsuaId,
                UtenteId = entity.AsuaUtenteFk,
                ArtistaId = entity.AsuaArtistaFk
            };
        }

        private static AsUtenteArtistaEntity MapToEntity(AsUtenteArtistaModel model)
        {
            return new AsUtenteArtistaEntity
            {
                AsuaId = model.Id,
                AsuaUtenteFk = model.UtenteId,
                AsuaArtistaFk = model.ArtistaId
            };
        }
    }
}