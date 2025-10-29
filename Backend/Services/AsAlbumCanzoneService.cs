using Backend.Entities;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class AsAlbumCanzoneService : IAsAlbumCanzoneService
    {
        private readonly IAsAlbumCanzoneRepository _repository;

        public AsAlbumCanzoneService(IAsAlbumCanzoneRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AsAlbumCanzoneModel>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.Select(MapToModel).ToList();
        }

        public async Task<AsAlbumCanzoneModel?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<List<AsAlbumCanzoneModel>> GetByAlbumIdAsync(long albumId)
        {
            var items = await _repository.GetByAlbumIdAsync(albumId);
            return items.Select(MapToModel).ToList();
        }

        public async Task<List<AsAlbumCanzoneModel>> GetByCanzoneIdAsync(long canzoneId)
        {
            var items = await _repository.GetByCanzoneIdAsync(canzoneId);
            return items.Select(MapToModel).ToList();
        }

        public async Task<AsAlbumCanzoneModel> CreateAsync(AsAlbumCanzoneModel model)
        {
            var entity = MapToEntity(model);
            var created = await _repository.AddAsync(entity);
            return MapToModel(created);
        }

        public async Task<bool> UpdateAsync(long id, AsAlbumCanzoneModel model)
        {
            var entity = MapToEntity(model);
            entity.AsalcId = id;
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static AsAlbumCanzoneModel MapToModel(AsAlbumCanzoneEntity e)
        {
            return new AsAlbumCanzoneModel
            {
                Id = e.AsalcId,
                CanzoneId = e.AsalcCanzoneFk,
                AlbumId = e.AsalcAlbumFk
            };
        }

        private static AsAlbumCanzoneEntity MapToEntity(AsAlbumCanzoneModel m)
        {
            return new AsAlbumCanzoneEntity
            {
                AsalcId = m.Id,
                AsalcCanzoneFk = m.CanzoneId,
                AsalcAlbumFk = m.AlbumId
            };
        }
    }
}