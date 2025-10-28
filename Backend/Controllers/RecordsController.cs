using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using Backend.Services;
using Backend.Entities;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;
        private readonly IPlaylistService _playlistService;
        private readonly IAlbumService _albumService;
        private readonly IAsAlbumCanzoneService _asAlbumCanzoneService;

        public RecordsController(SpotigneteDbContext context, IPlaylistService playlistService, IAlbumService albumService, IAsAlbumCanzoneService asAlbumCanzoneService)
        {
            _context = context;
            _playlistService = playlistService;
            _albumService = albumService;
            _asAlbumCanzoneService = asAlbumCanzoneService;
        }

        // POST: api/Records/playlist
        [HttpPost("playlist")]
        public async Task<ActionResult<PlaylistModel>> PostPlaylist(PlaylistModel playlist)
        {
            var created = await _playlistService.CreateAsync(playlist);
            return CreatedAtAction(nameof(GetPlaylist), new { id = created.Id }, created);
        }

        // POST: api/Records/album
        [HttpPost("album")]
        public async Task<ActionResult<AlbumModel>> PostAlbum(AlbumModel album)
        {
            var created = await _albumService.CreateAsync(album);
            return CreatedAtAction(nameof(GetAlbum), new { id = created.Id }, created);
        }

        // GET: api/Records/playlist/canzoni
        [HttpGet("playlist/canzoni")]
        public ActionResult<IEnumerable<AsCanzonePlaylistEntity>> GetPlaylistCanzoni()
        {
            return _context.AsCanzonePlaylist.ToList();
        }

        // GET: api/Records/album/canzoni
        [HttpGet("album/canzoni")]
        public async Task<ActionResult<IEnumerable<AsAlbumCanzoneModel>>> GetAlbumCanzoni()
        {
            var items = await _asAlbumCanzoneService.GetAllAsync();
            return Ok(items);
        }

        // GET: api/Records/playlists
        [HttpGet("playlists")]
        public async Task<ActionResult<IEnumerable<PlaylistModel>>> GetPlaylists()
        {
            var items = await _playlistService.GetAllAsync();
            return Ok(items);
        }

        // GET: api/Records/albums
        [HttpGet("albums")]
        public async Task<ActionResult<IEnumerable<AlbumModel>>> GetAlbums()
        {
            var items = await _albumService.GetAllAsync();
            return Ok(items);
        }

        // GET: api/Records/playlists/{id}
        [HttpGet("playlists/{id:long}")]
        public async Task<ActionResult<PlaylistModel>> GetPlaylist(long id)
        {
            var playlist = await _playlistService.GetByIdAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return Ok(playlist);
        }

        // GET: api/Records/albums/{id}
        [HttpGet("albums/{id:long}")]
        public async Task<ActionResult<AlbumModel>> GetAlbum(long id)
        {
            var album = await _albumService.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }

        // GET: api/Records/playlists/{name}
        [HttpGet("playlists/by-name/{name}")]
        public async Task<ActionResult<PlaylistModel>> GetPlaylist(string name)
        {
            var playlist = await _playlistService.GetByNameAsync(name);
            if (playlist == null)
            {
                return NotFound();
            }
            return Ok(playlist);
        }

        // GET: api/Records/albums/{name}
        [HttpGet("albums/by-name/{name}")]
        public async Task<ActionResult<AlbumModel>> GetAlbum(string name)
        {
            var album = await _albumService.GetByNameAsync(name);
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }

        // PUT: api/Records/playlists/{id}
        [HttpPut("playlists/{id}")]
        public async Task<IActionResult> PutPlaylist(long id, PlaylistModel playlist)
        {
            var ok = await _playlistService.UpdateAsync(id, playlist);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        // PUT: api/Records/albums/{id}
        [HttpPut("albums/{id}")]
        public async Task<IActionResult> PutAlbum(long id, AlbumModel album)
        {
            var ok = await _albumService.UpdateAsync(id, album);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Records/playlists/{id}
        [HttpDelete("playlists/{id}")]
        public async Task<IActionResult> DeletePlaylist(long id)
        {
            var ok = await _playlistService.DeleteAsync(id);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Records/albums/{id}
        [HttpDelete("albums/{id}")]
        public async Task<IActionResult> DeleteAlbum(long id)
        {
            var ok = await _albumService.DeleteAsync(id);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}