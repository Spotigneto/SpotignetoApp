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

        [HttpPost("playlist")]
        public async Task<ActionResult<PlaylistModel>> PostPlaylist(PlaylistModel playlist)
        {
            try
            {
                var created = await _playlistService.CreateAsync(playlist);
                // Use explicit location to avoid routing mismatches
                return Created($"/api/Records/playlists/{created.Id}", created);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.ToString(), statusCode: 500);
            }
        }

        [HttpPost("album")]
        public async Task<ActionResult<AlbumModel>> PostAlbum(AlbumModel album)
        {
            var created = await _albumService.CreateAsync(album);
            return CreatedAtAction(nameof(GetAlbum), new { id = created.Id }, created);
        }

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



        [HttpGet("albums")]
        public async Task<ActionResult<IEnumerable<AlbumModel>>> GetAlbums()
        {
            var items = await _albumService.GetAllAsync();
            return Ok(items);
        }



        [HttpGet("playlists/{id}")]
        public async Task<ActionResult<PlaylistModel>> GetPlaylist(string id)
        {
            var playlist = await _playlistService.GetByIdAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return Ok(playlist);
        }



        [HttpGet("albums/{id}")]
        public async Task<ActionResult<AlbumModel>> GetAlbum(string id)
        {
            var album = await _albumService.GetByIdAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }



        [HttpGet("playlists/by-name/{name}")]
        public async Task<ActionResult<PlaylistModel>> GetPlaylistByName(string name)
        {
            var playlist = await _playlistService.GetByNameAsync(name);
            if (playlist == null)
            {
                return NotFound();
            }
            return Ok(playlist);
        }



        [HttpGet("albums/by-name/{name}")]
        public async Task<ActionResult<AlbumModel>> GetAlbumByName(string name)
        {
            var album = await _albumService.GetByNameAsync(name);
            if (album == null)
            {
                return NotFound();
            }
            return Ok(album);
        }

        [HttpPut("playlists/{id}")]
        public async Task<IActionResult> PutPlaylist(string id, PlaylistModel playlist)
        {
            var ok = await _playlistService.UpdateAsync(id, playlist);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("albums/{id}")]
        public async Task<IActionResult> PutAlbum(string id, AlbumModel album)
        {
            var ok = await _albumService.UpdateAsync(id, album);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("playlists/{id}")]
        public async Task<IActionResult> DeletePlaylist(string id)
        {
            var ok = await _playlistService.DeleteAsync(id);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("albums/{id}")]
        public async Task<IActionResult> DeleteAlbum(string id)
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