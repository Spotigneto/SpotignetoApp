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

        // POST: api/Records/playlist
        [HttpPost("playlist")]
        public ActionResult<Record> PostPlaylist(PlaylistEntity playlist)
        {
            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return CreatedAtAction("GetPlaylist", new { id = playlist.PlId }, playlist);
        }

        // POST: api/Records/album
        [HttpPost("album")]
        public ActionResult<Record> PostAlbum(AlbumEntity album)
        {
            _context.Albums.Add(album);
            _context.SaveChanges();

            return CreatedAtAction("GetAlbum", new { id = album.AlId }, album);
        }

        // GET: api/Records/playlist/canzoni
        [HttpGet("playlist/canzoni")]
        public ActionResult<IEnumerable<AsCanzonePlaylistEntity>> GetPlaylistCanzoni()
        {
            return _context.PlaylistCanzoni.ToList();
        }

        // GET: api/Records/album/canzoni
        [HttpGet("album/canzoni")]
        public ActionResult<IEnumerable<AsAlbumCanzoneEntity>> GetAlbumCanzoni()
        {
            return _context.AlbumCanzoni.ToList();
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