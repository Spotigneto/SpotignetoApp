using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NavigateController : ControllerBase
    {
        private readonly SpotigneteDbContext _db;

        public NavigateController(SpotigneteDbContext db)
        {
            _db = db;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? letter)
        {
            if (string.IsNullOrEmpty(letter))
            {
                var playlistsAll = await _db.Playlists.ToListAsync();
                var canzoniAll = await _db.Canzoni.ToListAsync();
                var artistiAll = await _db.Artisti.ToListAsync();
                var generiAll = await _db.Generi.ToListAsync();
                return Ok(new { playlists = playlistsAll, canzoni = canzoniAll, artisti = artistiAll, generi = generiAll });
            }

            var playlists = await _db.Playlists.Where(p => p.PlNome.StartsWith(letter)).ToListAsync();
            var canzoni = await _db.Canzoni.Where(c => c.CaNome.StartsWith(letter)).ToListAsync();
            var artisti = await _db.Artisti.Where(a => a.ArNome.StartsWith(letter)).ToListAsync();
            var generi = await _db.Generi.Where(g => g.GtpGenere.StartsWith(letter)).ToListAsync();
            return Ok(new { playlists, canzoni, artisti, generi });
        }
    }
}
