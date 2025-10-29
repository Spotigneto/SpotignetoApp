using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly SpotigneteDbContext _context;

        public AlbumRepository(SpotigneteDbContext context)
        {
            _context = context;
        }

        public async Task<List<AlbumEntity>> GetAllAsync()
        {
            return await _context.Albums.AsNoTracking().ToListAsync();
        }

        public async Task<AlbumEntity?> GetByIdAsync(string id)
        {
            return await _context.Albums.AsNoTracking()
                .FirstOrDefaultAsync(a => a.AlId == id);
        }

        public async Task<AlbumEntity?> GetByNameAsync(string name)
        {
            return await _context.Albums.AsNoTracking()
                .FirstOrDefaultAsync(a => a.AlNome == name);
        }

        public async Task<AlbumEntity> AddAsync(AlbumEntity entity)
        {
            _context.Albums.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(AlbumEntity entity)
        {
            var existing = await _context.Albums.FindAsync(entity.AlId);
            if (existing == null) return false;

            existing.AlNome = entity.AlNome;
            existing.AlPubblica = entity.AlPubblica;
            existing.AlReleaseDate = entity.AlReleaseDate;

            _context.Albums.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Albums.FindAsync(id);
            if (entity == null) return false;

            _context.Albums.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}