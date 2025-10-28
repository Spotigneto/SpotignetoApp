using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class NavigateService : INavigateService
    {
        private readonly SpotigneteDbContext _context;
        public NavigateService(SpotigneteDbContext context) { _context = context; }

        public async Task<(IReadOnlyList<ItemModel> songs, IReadOnlyList<ItemModel> playlists, IReadOnlyList<ItemModel> artists)> SearchFiltriAsync(string? q, long? genereId, long? sottoGenereId, bool? playlistPrivata)
        {
            var qp = (q ?? "").Trim();

            var songsQuery = _context.Set<Canzone>().AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(qp)) songsQuery = songsQuery.Where(s => EF.Functions.Like(s.Nome, qp + "%"));
            if (genereId.HasValue) songsQuery = songsQuery.Where(s => s.GenereFk == genereId.Value);
            if (sottoGenereId.HasValue) songsQuery = songsQuery.Where(s => s.SottogenereFk == sottoGenereId.Value);
            var songs = await songsQuery.OrderBy(s => s.Nome).Take(50)
                .Select(s => new ItemModel { Id = s.Id, Nome = s.Nome }).ToListAsync();

            var playlistsQuery = _context.Set<Playlist>().AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(qp)) playlistsQuery = playlistsQuery.Where(p => EF.Functions.Like(p.Nome, qp + "%"));
            if (playlistPrivata.HasValue) playlistsQuery = playlistsQuery.Where(p => p.Privata == playlistPrivata.Value);
            var playlists = await playlistsQuery.OrderBy(p => p.Nome).Take(50)
                .Select(p => new ItemModel { Id = p.Id, Nome = p.Nome }).ToListAsync();

            var artistsQuery = _context.Set<Artista>().AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(qp)) artistsQuery = artistsQuery.Where(a => EF.Functions.Like(a.Nome, qp + "%"));
            var artists = await artistsQuery.OrderBy(a => a.Nome).Take(50)
                .Select(a => new ItemModel { Id = a.Id, Nome = a.Nome }).ToListAsync();

            return (songs, playlists, artists);
        }

        public async Task<(IReadOnlyList<ItemModel> songs, IReadOnlyList<ItemModel> playlists, IReadOnlyList<ItemModel> artists)> LettereAsync(string q)
        {
            var qp = q.Trim();

            var songs = await _context.Set<Canzone>().AsNoTracking()
                .Where(s => EF.Functions.Like(s.Nome, qp + "%"))
                .OrderBy(s => s.Nome).Take(50)
                .Select(s => new ItemModel { Id = s.Id, Nome = s.Nome }).ToListAsync();

            var playlists = await _context.Set<Playlist>().AsNoTracking()
                .Where(p => EF.Functions.Like(p.Nome, qp + "%"))
                .OrderBy(p => p.Nome).Take(50)
                .Select(p => new ItemModel { Id = p.Id, Nome = p.Nome }).ToListAsync();

            var artists = await _context.Set<Artista>().AsNoTracking()
                .Where(a => EF.Functions.Like(a.Nome, qp + "%"))
                .OrderBy(a => a.Nome).Take(50)
                .Select(a => new ItemModel { Id = a.Id, Nome = a.Nome }).ToListAsync();

            return (songs, playlists, artists);
        }
    }
}
