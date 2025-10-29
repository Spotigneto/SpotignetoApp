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
        public async Task<ActionResult<IEnumerable<AsAlbumCanzoneModel>>> GetTracksInAlbum([FromQuery] string albumId)
        {
            var items = await _service.GetByAlbumIdAsync(albumId);
            return Ok(items);
        }

        // GET: api/AsAlbumCanzone/GetTrackInAlbum/{albumId}/{canzoneId}
        [HttpGet("GetTrackInAlbum/{albumId}/{canzoneId}")]
        public async Task<ActionResult<AsAlbumCanzoneModel>> GetTrackInAlbum(string albumId, string canzoneId)
        {
            var items = await _service.GetByAlbumIdAsync(albumId);
            var track = items.FirstOrDefault(t => t.CanzoneId == canzoneId);
            
            if (track == null)
            {
                return NotFound("Track not found in album");
            }

            return Ok(track);
        }

        [HttpPut("{albumId}/{canzoneId}")]
        public async Task<IActionResult> UpdateTrackInAlbum(string albumId, string canzoneId, [FromBody] AsAlbumCanzoneModel model)
        {
            // Find the existing relationship
            var allRelations = await _service.GetAllAsync();
            var existingRelation = allRelations.FirstOrDefault(r => r.AlbumId == albumId && r.CanzoneId == canzoneId);
            
            if (existingRelation == null)
            {
                return NotFound("Track not found in album");
            }

            var result = await _service.UpdateAsync(existingRelation.Id, model);
            return result ? Ok() : BadRequest();
        }

        [HttpDelete("{albumId}/{canzoneId}")]
        public async Task<IActionResult> RemoveTrackFromAlbum(string albumId, string canzoneId)
        {
            // Find the existing relationship
            var allRelations = await _service.GetAllAsync();
            var existingRelation = allRelations.FirstOrDefault(r => r.AlbumId == albumId && r.CanzoneId == canzoneId);
            
            if (existingRelation == null)
            {
                return NotFound("Track not found in album");
            }

            var result = await _service.DeleteAsync(existingRelation.Id);
            return result ? Ok() : NotFound();
        }
    }
}