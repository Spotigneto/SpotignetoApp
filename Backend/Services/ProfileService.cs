using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;         // per AsNoTracking, ToListAsync, ecc.
using Backend.Data;
using Backend.Models;

namespace Backend.Services
{
    public class ProfileService : IProfileService
    {
        private readonly SpotigneteDbContext _context;
        public ProfileService(SpotigneteDbContext context) => _context = context;

        public async Task<(IReadOnlyList<AlbumItemModel> albums, IReadOnlyList<ItemModel> playlists)> SearchFiltriAsync(
            string? q,
            bool? albumPubblica,
            DateTime? albumFrom,
            DateTime? albumTo,
            bool? playlistPrivata)
        {
            var qp = (q ?? string.Empty).Trim();

            // ------- ALBUM -------
            // Supponiamo che la tua entity si chiami "Album" con proprietà:
            // Id (long), Nome (string), Pubblica (bool), ReleaseDate (DateTime?)
            var albumsQuery = _context.Set<Album>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(qp))
                albumsQuery = albumsQuery.Where(a => a.Nome.StartsWith(qp));

            if (albumPubblica.HasValue)
                albumsQuery = albumsQuery.Where(a => a.Pubblica == albumPubblica.Value);

            if (albumFrom.HasValue)
                albumsQuery = albumsQuery.Where(a => a.ReleaseDate >= albumFrom.Value);

            if (albumTo.HasValue)
                albumsQuery = albumsQuery.Where(a => a.ReleaseDate <= albumTo.Value);

            var albums = await albumsQuery
                .OrderBy(a => a.Nome)
                .Take(50)
                .Select(a => new AlbumItemModel
                {
                    Id = a.Id,
                    Nome = a.Nome,
                    Pubblica = a.Pubblica,
                    ReleaseDate = a.ReleaseDate
                })
                .ToListAsync();

            // ------- PLAYLIST -------
            // Supponiamo che la tua entity si chiami "Playlist" con proprietà:
            // Id (long), Nome (string), Privata (bool)
            var playlistsQuery = _context.Set<Playlist>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(qp))
                playlistsQuery = playlistsQuery.Where(p => p.Nome.StartsWith(qp));

            if (playlistPrivata.HasValue)
                playlistsQuery = playlistsQuery.Where(p => p.Privata == playlistPrivata.Value);

            var playlists = await playlistsQuery
                .OrderBy(p => p.Nome)
                .Take(50)
                .Select(p => new ItemModel
                {
                    Id = p.Id,
                    Nome = p.Nome
                })
                .ToListAsync();

            return (albums, playlists);
        }
    }

    // ====== ESEMPI DI ENTITY (rimuovi se li hai già definiti altrove) ======
    // Metti questi in un file separato o cancellali se le tue entity esistono già.
    // Sono qui solo per mostrare le proprietà attese.
    public class Album
    {
        public long Id { get; set; }
        public string Nome { get; set; } = default!;
        public bool Pubblica { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }

    public class Playlist
    {
        public long Id { get; set; }
        public string Nome { get; set; } = default!;
        public bool Privata { get; set; }
    }
}
