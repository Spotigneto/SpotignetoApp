using Backend.Data;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistaCanzoneController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;

        public ArtistaCanzoneController(SpotigneteDbContext context)
        {
            _context = context;
        }

        [HttpPost("Associate")]
        public async Task<IActionResult> AssociateArtistaCanzone([FromBody] AsArtistaCanzoneModel model)
        {
            var existing = await _context.AsArtistaCanzone
                .FirstOrDefaultAsync(a => a.ArtistaFk == model.ArtistaId && a.CanzoneFk == model.CanzoneId);

            if (existing != null)
            {
                return Ok("Associazione già esistente");
            }

            var entity = new AsArtistaCanzoneEntity
            {
                ArtistaFk = model.ArtistaId,
                CanzoneFk = model.CanzoneId
            };

            _context.AsArtistaCanzone.Add(entity);
            await _context.SaveChangesAsync();

            return Ok("Associazione creata");
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveArtistaCanzone([FromQuery] string artistaId, [FromQuery] string canzoneId)
        {
            var entity = await _context.AsArtistaCanzone
                .FirstOrDefaultAsync(a => a.ArtistaFk == artistaId && a.CanzoneFk == canzoneId);

            if (entity == null)
            {
                return NotFound("Associazione non trovata");
            }

            _context.AsArtistaCanzone.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok("Associazione rimossa");
        }
    }
}