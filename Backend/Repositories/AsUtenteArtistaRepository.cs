using Backend.Data;
using Backend.Entities;
using Backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class AsUtenteArtistaRepository : IAsUtenteArtistaRepository
    {
        private readonly SpotigneteDbContext _context;

        public AsUtenteArtistaRepository(SpotigneteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AsUtenteArtistaEntity>> GetAllAsync()
        {
            return await _context.AsUtenteArtista.ToListAsync();
        }

        public async Task<AsUtenteArtistaEntity?> GetByIdAsync(long id)
        {
            return await _context.AsUtenteArtista.FindAsync(id);
        }

        public async Task<IEnumerable<AsUtenteArtistaEntity>> GetByUtenteIdAsync(string utenteId)
        {
            return await _context.AsUtenteArtista
                .Where(ua => ua.AsuaUtenteFk == utenteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AsUtenteArtistaEntity>> GetByArtistaIdAsync(string artistaId)
        {
            return await _context.AsUtenteArtista
                .Where(ua => ua.AsuaArtistaFk == artistaId)
                .ToListAsync();
        }

        public async Task<AsUtenteArtistaEntity?> GetByUtenteAndArtistaAsync(string utenteId, string artistaId)
        {
            return await _context.AsUtenteArtista
                .FirstOrDefaultAsync(ua => ua.AsuaUtenteFk == utenteId && ua.AsuaArtistaFk == artistaId);
        }

        public async Task<AsUtenteArtistaEntity> CreateAsync(AsUtenteArtistaEntity entity)
        {
            _context.AsUtenteArtista.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<AsUtenteArtistaEntity> UpdateAsync(AsUtenteArtistaEntity entity)
        {
            _context.AsUtenteArtista.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.AsUtenteArtista.FindAsync(id);
            if (entity == null)
                return false;

            _context.AsUtenteArtista.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByUtenteAndArtistaAsync(string utenteId, string artistaId)
        {
            var entity = await GetByUtenteAndArtistaAsync(utenteId, artistaId);
            if (entity == null)
                return false;

            _context.AsUtenteArtista.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string utenteId, string artistaId)
        {
            return await _context.AsUtenteArtista
                .AnyAsync(ua => ua.AsuaUtenteFk == utenteId && ua.AsuaArtistaFk == artistaId);
        }
    }
}