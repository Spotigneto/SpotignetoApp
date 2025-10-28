using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class AsAlbumCanzoneRepository : IAsAlbumCanzoneRepository
    {
        private readonly SpotigneteDbContext _context;

        public AsAlbumCanzoneRepository(SpotigneteDbContext context)
        {
            _context = context;
        }

        public async Task<List<AsAlbumCanzoneEntity>> GetAllAsync()
        {
            return await _context.AsAlbumCanzone.AsNoTracking().ToListAsync();
        }

        public async Task<AsAlbumCanzoneEntity?> GetByIdAsync(long id)
        {
            return await _context.AsAlbumCanzone.AsNoTracking().FirstOrDefaultAsync(a => a.AsalcId == id);
        }

        public async Task<List<AsAlbumCanzoneEntity>> GetByAlbumIdAsync(long albumId)
        {
            return await _context.AsAlbumCanzone.AsNoTracking()
                .Where(a => a.AsalcAlbumFk == albumId)
                .ToListAsync();
        }

        public async Task<List<AsAlbumCanzoneEntity>> GetByCanzoneIdAsync(long canzoneId)
        {
            return await _context.AsAlbumCanzone.AsNoTracking()
                .Where(a => a.AsalcCanzoneFk == canzoneId)
                .ToListAsync();
        }

        public async Task<AsAlbumCanzoneEntity> AddAsync(AsAlbumCanzoneEntity entity)
        {
            _context.AsAlbumCanzone.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(AsAlbumCanzoneEntity entity)
        {
            var existing = await _context.AsAlbumCanzone.FirstOrDefaultAsync(a => a.AsalcId == entity.AsalcId);
            if (existing == null)
            {
                return false;
            }

            existing.AsalcCanzoneFk = entity.AsalcCanzoneFk;
            existing.AsalcAlbumFk = entity.AsalcAlbumFk;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.AsAlbumCanzone.FirstOrDefaultAsync(a => a.AsalcId == id);
            if (entity == null)
            {
                return false;
            }

            _context.AsAlbumCanzone.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}