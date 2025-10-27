using Backend.Entities;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _repo;

        public AlbumService(IAlbumRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AlbumModel>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(MapToModel).ToList();
        }

        public async Task<AlbumModel?> GetByIdAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<AlbumModel?> GetByNameAsync(string name)
        {
            var entity = await _repo.GetByNameAsync(name);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<AlbumModel> CreateAsync(AlbumModel model)
        {
            var entity = new AlbumEntity
            {
                AlNome = model.Nome,
                AlPubblica = model.Pubblica,
                AlReleaseDate = model.ReleaseDate
            };
            var saved = await _repo.AddAsync(entity);
            return MapToModel(saved);
        }

        public async Task<bool> UpdateAsync(long id, AlbumModel model)
        {
            var entity = new AlbumEntity
            {
                AlId = id,
                AlNome = model.Nome,
                AlPubblica = model.Pubblica,
                AlReleaseDate = model.ReleaseDate
            };
            return await _repo.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repo.DeleteAsync(id);
        }

        private static AlbumModel MapToModel(AlbumEntity e)
        {
            return new AlbumModel
            {
                Id = e.AlId,
                Nome = e.AlNome,
                Pubblica = e.AlPubblica,
                ReleaseDate = e.AlReleaseDate
            };
        }
    }
}