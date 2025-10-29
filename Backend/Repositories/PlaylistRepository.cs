using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly SpotigneteDbContext _context;

        public PlaylistRepository(SpotigneteDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlaylistEntity>> GetAllAsync()
        {
            return await _context.Playlists.AsNoTracking().ToListAsync();
        }

        public async Task<PlaylistEntity?> GetByIdAsync(long id)
        {
            return await _context.Playlists.AsNoTracking()
                .FirstOrDefaultAsync(p => p.PlId == id);
        }

        public async Task<PlaylistEntity?> GetByNameAsync(string name)
        {
            return await _context.Playlists.AsNoTracking()
                .FirstOrDefaultAsync(p => p.PlNome == name);
        }

        public async Task<PlaylistEntity> AddAsync(PlaylistEntity entity)
        {
            _context.Playlists.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(PlaylistEntity entity)
        {
            var existing = await _context.Playlists.FindAsync(entity.PlId);
            if (existing == null) return false;

            existing.PlNome = entity.PlNome;
            existing.PlPrivata = entity.PlPrivata;

            _context.Playlists.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Playlists.FindAsync(id);
            if (entity == null) return false;

            _context.Playlists.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}