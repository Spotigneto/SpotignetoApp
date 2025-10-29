using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly SpotigneteDbContext _context;

        public ProfileRepository(SpotigneteDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProfileEntity>> GetAllAsync()
        {
            return await _context.Utenti.AsNoTracking().ToListAsync();
        }

        public async Task<ProfileEntity?> GetByIdAsync(long id)
        {
            return await _context.Utenti.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ProfileEntity> AddAsync(ProfileEntity entity)
        {
            _context.Utenti.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(ProfileEntity entity)
        {
            var existing = await _context.Utenti.FindAsync(entity.Id);
            if (existing == null) return false;

            existing.Nome = entity.Nome;

            _context.Utenti.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.Utenti.FindAsync(id);
            if (entity == null) return false;

            _context.Utenti.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}