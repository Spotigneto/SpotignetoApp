using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ArtistaRepository : IArtistaRepository
    {
        private readonly SpotigneteDbContext _context;

        public ArtistaRepository(SpotigneteDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArtistaEntity>> GetAllAsync()
        {
            return await _context.Artisti.AsNoTracking().ToListAsync();
        }

        public async Task<ArtistaEntity?> GetByIdAsync(string id)
        {
            return await _context.Artisti.AsNoTracking()
                .FirstOrDefaultAsync(a => a.ArId == id);
        }

        public async Task<ArtistaEntity?> GetByNameAsync(string name)
        {
            return await _context.Artisti.AsNoTracking()
                .FirstOrDefaultAsync(a => a.ArNome == name);
        }

        public async Task<ArtistaEntity> AddAsync(ArtistaEntity entity)
        {
            _context.Artisti.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(ArtistaEntity entity)
        {
            var existing = await _context.Artisti.FindAsync(entity.ArId);
            if (existing == null) return false;

            existing.ArNome = entity.ArNome;

            _context.Artisti.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Artisti.FindAsync(id);
            if (entity == null) return false;

            _context.Artisti.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}