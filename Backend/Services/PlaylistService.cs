using Backend.Entities;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _repo;

        public PlaylistService(IPlaylistRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<PlaylistModel>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(MapToModel).ToList();
        }

        public async Task<PlaylistModel?> GetByIdAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<PlaylistModel?> GetByNameAsync(string name)
        {
            var entity = await _repo.GetByNameAsync(name);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<PlaylistModel> CreateAsync(PlaylistModel model)
        {
            var entity = new PlaylistEntity
            {
                PlNome = model.Nome,
                PlPrivata = model.Privata
            };
            var saved = await _repo.AddAsync(entity);
            return MapToModel(saved);
        }

        public async Task<bool> UpdateAsync(long id, PlaylistModel model)
        {
            var entity = new PlaylistEntity
            {
                PlId = id,
                PlNome = model.Nome,
                PlPrivata = model.Privata
            };
            return await _repo.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repo.DeleteAsync(id);
        }

        private static PlaylistModel MapToModel(PlaylistEntity e)
        {
            return new PlaylistModel
            {
                Id = e.PlId,
                Nome = e.PlNome,
                Privata = e.PlPrivata
            };
        }
    }
}