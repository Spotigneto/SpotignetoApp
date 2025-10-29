using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Entities;
using Backend.Repositories;

namespace Backend.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _repo;
        private readonly SpotigneteDbContext _context;

        public ProfileService(IProfileRepository repo, SpotigneteDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<ProfileModel>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(MapToModel).ToList();
        }

        public async Task<ProfileModel?> GetByIdAsync(string id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity == null ? null : MapToModel(entity);
        }

        public async Task<ProfileModel> CreateAsync(ProfileModel model)
        {
            var entity = new ProfileEntity { Nome = model.Nome };
            var saved = await _repo.AddAsync(entity);
            return MapToModel(saved);
        }

        public async Task<bool> UpdateAsync(string id, ProfileModel model)
        {
            var entity = new ProfileEntity
            {
                Id = id,
                Nome = model.Nome
            };
            return await _repo.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repo.DeleteAsync(id);
        }

        private static ProfileModel MapToModel(ProfileEntity e)
        {
            return new ProfileModel { Id = e.Id, Nome = e.Nome };
        }

        public async Task<(IReadOnlyList<AlbumItemModel> albums, IReadOnlyList<ItemModel> playlists)> SearchFiltriAsync(
            string? q, bool? albumPubblica, DateTime? albumFrom, DateTime? albumTo, bool? playlistPrivata)
        {
            var qp = (q ?? string.Empty).Trim();

            var albumsQuery = _context.Albums.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(qp))
                albumsQuery = albumsQuery.Where(a => a.AlNome.StartsWith(qp));
            var albums = await albumsQuery.OrderBy(a => a.AlNome).Take(50)
                .Select(a => new AlbumItemModel { Id = a.AlId, Nome = a.AlNome, Pubblica = a.AlPubblica, ReleaseDate = a.AlReleaseDate })
                .ToListAsync();

            var playlistsQuery = _context.Playlists.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(qp))
                playlistsQuery = playlistsQuery.Where(p => p.PlNome.StartsWith(qp));
            var playlists = await playlistsQuery.OrderBy(p => p.PlNome).Take(50)
                .Select(p => new ItemModel { Id = p.PlId, Nome = p.PlNome })
                .ToListAsync();

            return (albums, playlists);
        }
    }
}