using Backend.Data;
using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtentePlaylistController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;

        public UtentePlaylistController(SpotigneteDbContext context)
        {
            _context = context;
        }

        [HttpPost("Associate")]
        public async Task<IActionResult> AssociateUtentePlaylist([FromBody] AsUtentePlaylistModel model)
        {
            var existing = await _context.AsUtentePlaylist
                .FirstOrDefaultAsync(a => a.UtenteFk == model.UtenteId && a.PlaylistFk == model.PlaylistId);

            if (existing != null)
            {
                return Ok("Associazione già esistente");
            }

            var entity = new AsUtentePlaylistEntity
            {
                UtenteFk = model.UtenteId,
                PlaylistFk = model.PlaylistId
            };

            _context.AsUtentePlaylist.Add(entity);
            await _context.SaveChangesAsync();

            return Ok("Associazione creata");
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveUtentePlaylist([FromQuery] string utenteId, [FromQuery] string playlistId)
        {
            var entity = await _context.AsUtentePlaylist
                .FirstOrDefaultAsync(a => a.UtenteFk == utenteId && a.PlaylistFk == playlistId);

            if (entity == null)
            {
                return NotFound("Associazione non trovata");
            }

            _context.AsUtentePlaylist.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok("Associazione rimossa");
        }
    }
}