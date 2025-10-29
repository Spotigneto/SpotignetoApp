using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class CanzoneRepository : ICanzoneRepository
    {
        private readonly SpotigneteDbContext _context;

        public CanzoneRepository(SpotigneteDbContext context)
        {
            _context = context;
        }

        public async Task<List<CanzoneEntity>> GetAllAsync()
        {
            return await _context.Canzoni.AsNoTracking().ToListAsync();
        }

        public async Task<CanzoneEntity?> GetByIdAsync(string id)
        {
            return await _context.Canzoni.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CaId == id);
        }

        public async Task<CanzoneEntity?> GetByNameAsync(string name)
        {
            return await _context.Canzoni.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CaNome == name);
        }

        public async Task<CanzoneEntity> AddAsync(CanzoneEntity entity)
        {
            _context.Canzoni.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(CanzoneEntity entity)
        {
            var existing = await _context.Canzoni.FindAsync(entity.CaId);
            if (existing == null) return false;

            existing.CaNome = entity.CaNome;
            existing.CaFile = entity.CaFile;
            existing.CaGenere = entity.CaGenere;
            existing.CaSottogenere = entity.CaSottogenere;
            existing.CaDurata = entity.CaDurata;
            existing.CaSecondi = entity.CaSecondi;

            _context.Canzoni.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.Canzoni.FindAsync(id);
            if (entity == null) return false;

            _context.Canzoni.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}