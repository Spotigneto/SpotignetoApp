using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Services
{
    public class HomeService : IHomeService
    {
        private readonly SpotigneteDbContext _context;
        public HomeService(SpotigneteDbContext context) { _context = context; }

        public async Task<(IReadOnlyList<ItemModel> songs, IReadOnlyList<ItemModel> artists, IReadOnlyList<ItemModel> playlists)> RandomViewAsync(int songs = 6, int artists = 6, int playlists = 6)
        {

            var s = await _context.Canzone
                .OrderBy(c => Guid.NewGuid())
                .Take(songs)
                .Select(c => new ItemModel { Id = c.Id, Nome = c.Nome })
                .ToListAsync();

            var a = await _context.Artista
                .OrderBy(ar => Guid.NewGuid())
                .Take(artists)
                .Select(ar => new ItemModel { Id = ar.Id, Nome = ar.Nome })
                .ToListAsync();

            var p = await _context.Playlist
                .OrderBy(pl => Guid.NewGuid())
                .Take(playlists)
                .Select(pl => new ItemModel { Id = pl.Id, Nome = pl.Nome })
                .ToListAsync();

            return (s, a, p);
        }
    }
}
