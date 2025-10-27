using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Entities;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;

        public RecordsController(SpotigneteDbContext context)
        {
            _context = context;
        }

        // GET: api/Records/canzoni
        [HttpGet("canzoni")]
        public ActionResult<IEnumerable<CanzoneEntity>> GetCanzoni()
        {
            return _context.Canzoni.ToList();
        }

        // GET: api/Records/playlists
        [HttpGet("playlists")]
        public ActionResult<IEnumerable<PlaylistEntity>> GetPlaylists()
        {
            return _context.Playlists.ToList();
        }

        // GET: api/Records/albums
        [HttpGet("albums")]
        public ActionResult<IEnumerable<AlbumEntity>> GetAlbums()
        {
            return _context.Albums.ToList();
        }
    }
}