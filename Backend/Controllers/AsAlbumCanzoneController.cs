using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsAlbumCanzoneController : ControllerBase
    {
        private readonly IAsAlbumCanzoneService _service;

        public AsAlbumCanzoneController(IAsAlbumCanzoneService service)
        {
            _service = service;
        }

        // POST: api/AsAlbumCanzone/AddTrackToAlbum
        [HttpPost("AddTrackToAlbum")]
        public async Task<ActionResult<AsAlbumCanzoneModel>> AddTrackToAlbum(AsAlbumCanzoneModel model)
        {
            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetTrackInAlbum), new { albumId = created.AlbumId, canzoneId = created.CanzoneId }, created);
        }

        // GET: api/AsAlbumCanzone/GetTracksInAlbum
        [HttpGet("GetTracksInAlbum")]
        public async Task<ActionResult<IEnumerable<AsAlbumCanzoneModel>>> GetTracksInAlbum([FromQuery] long albumId)
        {
            var items = await _service.GetByAlbumIdAsync(albumId);
            return Ok(items);
        }

        // GET: api/AsAlbumCanzone/GetTrackInAlbum/{albumId}/{canzoneId}
        [HttpGet("GetTrackInAlbum/{albumId}/{canzoneId}")]
        public async Task<ActionResult<AsAlbumCanzoneModel>> GetTrackInAlbum(long albumId, long canzoneId)
        {
            var items = await _service.GetByAlbumIdAsync(albumId);
            var track = items.FirstOrDefault(t => t.CanzoneId == canzoneId);
            
            if (track == null)
            {
                return NotFound("Track not found in album");
            }

            return Ok(track);
        }

        // PUT: api/AsAlbumCanzone/UpdateTrackInAlbum/{id}
        [HttpPut("UpdateTrackInAlbum/{id}")]
        public async Task<IActionResult> UpdateTrackInAlbum(long id, AsAlbumCanzoneModel model)
        {
            var ok = await _service.UpdateAsync(id, model);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/AsAlbumCanzone/RemoveTrackFromAlbum/{id}
        [HttpDelete("RemoveTrackFromAlbum/{id}")]
        public async Task<IActionResult> RemoveTrackFromAlbum(long id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}