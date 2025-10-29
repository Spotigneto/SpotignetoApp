using Backend.Data;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistaAlbumController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;

        public ArtistaAlbumController(SpotigneteDbContext context)
        {
            _context = context;
        }

        [HttpPost("Associate")]
        public async Task<IActionResult> AssociateArtistaAlbum([FromBody] AsArtistaAlbumModel model)
        {
            var existing = await _context.AsArtistaAlbum
                .FirstOrDefaultAsync(a => a.ArtistaFk == model.ArtistaId && a.AlbumFk == model.AlbumId);

            if (existing != null)
            {
                return Ok("Associazione già esistente");
            }

            var entity = new AsArtistaAlbumEntity
            {
                ArtistaFk = model.ArtistaId,
                AlbumFk = model.AlbumId
            };

            _context.AsArtistaAlbum.Add(entity);
            await _context.SaveChangesAsync();

            return Ok("Associazione creata");
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveArtistaAlbum([FromQuery] string artistaId, [FromQuery] string albumId)
        {
            var entity = await _context.AsArtistaAlbum
                .FirstOrDefaultAsync(a => a.ArtistaFk == artistaId && a.AlbumFk == albumId);

            if (entity == null)
            {
                return NotFound("Associazione non trovata");
            }

            _context.AsArtistaAlbum.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok("Associazione rimossa");
        }
    }
}