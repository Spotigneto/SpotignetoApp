using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class NavigateService : INavigateService
    {
        private readonly SpotigneteDbContext _context;
        public NavigateService(SpotigneteDbContext context) { _context = context; }

        public async Task<(IReadOnlyList<ItemModel> songs, IReadOnlyList<ItemModel> playlists, IReadOnlyList<ItemModel> artists)>
            SearchFiltriAsync(string? q, string? genereId, string? sottoGenereId, bool? playlistPrivata)
        {
            var qp = (q ?? "").Trim();
            var songsQuery = _context.Canzoni.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(qp))
                songsQuery = songsQuery.Where(s => s.CaNome.StartsWith(qp));

            if (genereId != null)
                songsQuery = songsQuery.Where(s => s.CaGenere == genereId);

            if (sottoGenereId != null)
                songsQuery = songsQuery.Where(s => s.CaSottogenere == sottoGenereId);

            var songs = await songsQuery.OrderBy(s => s.CaNome).Take(50)
                .Select(s => new ItemModel { Id = s.CaId, Nome = s.CaNome }).ToListAsync();

            var playlistsQuery = _context.Playlists.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(qp))
                playlistsQuery = playlistsQuery.Where(p => p.PlNome.StartsWith(qp));
            if (playlistPrivata.HasValue)
                playlistsQuery = playlistsQuery.Where(p => p.PlPrivata == playlistPrivata.Value);
            var playlists = await playlistsQuery.OrderBy(p => p.PlNome).Take(50)
                .Select(p => new ItemModel { Id = p.PlId, Nome = p.PlNome }).ToListAsync();

            var artistsQuery = _context.Artisti.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(qp))
                artistsQuery = artistsQuery.Where(a => a.ArNome.StartsWith(qp));
            var artists = await artistsQuery.OrderBy(a => a.ArNome).Take(50)
                .Select(a => new ItemModel { Id = a.ArId, Nome = a.ArNome }).ToListAsync();

            return (songs, playlists, artists);
        }

        public async Task<(IReadOnlyList<ItemModel> songs, IReadOnlyList<ItemModel> playlists, IReadOnlyList<ItemModel> artists)>
            LettereAsync(string q)
        {
            var qp = q.Trim();

            var songs = await _context.Canzoni.AsNoTracking()
                .Where(s => s.CaNome.StartsWith(qp))
                .OrderBy(s => s.CaNome).Take(50)
                .Select(s => new ItemModel { Id = s.CaId, Nome = s.CaNome }).ToListAsync();

            var playlists = await _context.Playlists.AsNoTracking()
                .Where(p => p.PlNome.StartsWith(qp))
                .OrderBy(p => p.PlNome).Take(50)
                .Select(p => new ItemModel { Id = p.PlId, Nome = p.PlNome }).ToListAsync();

            var artists = await _context.Artisti.AsNoTracking()
                .Where(a => a.ArNome.StartsWith(qp))
                .OrderBy(a => a.ArNome).Take(50)
                .Select(a => new ItemModel { Id = a.ArId, Nome = a.ArNome }).ToListAsync();

            return (songs, playlists, artists);
        }
    }
}