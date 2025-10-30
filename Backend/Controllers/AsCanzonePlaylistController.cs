using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Entities;
using Backend.Models;
using System.Linq;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsCanzonePlaylistController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;

        public AsCanzonePlaylistController(SpotigneteDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("AddTrackToPlaylist")]
        public IActionResult AddTrackToPlaylist([FromBody] AsCanzonePlaylistModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var asCanzonePlaylistEntity = new AsCanzonePlaylistEntity
            {
                AscpPlaylistFk = model.AcPlaylistId,
                AscpCanzoneFk = model.AcCanzoneId
            };

            _context.AsCanzonePlaylist.Add(asCanzonePlaylistEntity);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("GetTracksInPlaylist")]
        public IActionResult GetTracksInPlaylist([FromQuery] string playlistId)
        {
            var tracks = _context.AsCanzonePlaylist
                .Where(ac => ac.AscpPlaylistFk == playlistId)
                .Select(ac => new { 
                    CanzoneId = ac.AscpCanzoneFk
                })
                .ToList();

            return Ok(tracks);
        }

        [HttpGet]
        [Route("GetTrackInPlaylist/{playlistId}/{canzoneId}")]
        public IActionResult GetTrackInPlaylist(string playlistId, string canzoneId)
        {
            var relation = _context.AsCanzonePlaylist
                .FirstOrDefault(ac => ac.AscpPlaylistFk == playlistId && ac.AscpCanzoneFk == canzoneId);

            if (relation == null)
            {
                return NotFound("Track not found in playlist");
            }

            return Ok(relation);
        }

        [HttpGet]
        [Route("GetTracksInPlaylist/{id}/{trackname}")]
        public IActionResult GetTracksInPlaylist([FromRoute] string id, [FromRoute] string trackname)
        {
            // First find the playlist by name
            var playlist = _context.Playlists.FirstOrDefault(p => p.PlId == id);
            if (playlist == null)
            {
                return NotFound($"Playlist '{id}' not found");  
            }

            // Then get all tracks in that playlist
            var tracks = _context.AsCanzonePlaylist
                .Where(ac => ac.AscpPlaylistFk == playlist.PlId)
                .Join(_context.Canzoni,
                      ac => ac.AscpCanzoneFk,
                      c => c.CaId,
                      (ac, c) => c)
                .Where(c => c.CaNome.Contains(trackname))       
                .ToList();

            return Ok(tracks);
        }

        [HttpPut]
        [Route("UpdateTrackInPlaylist")]
        public IActionResult UpdateTrackFromPlaylist([FromBody] AsCanzonePlaylistModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEntity = _context.AsCanzonePlaylist
                .FirstOrDefault(ac => ac.AscpPlaylistFk == model.AcPlaylistId && ac.AscpCanzoneFk == model.OldCanzoneId);

            if (existingEntity == null)
            {
                return NotFound("Track not found in playlist");
            }

            existingEntity.AscpCanzoneFk = model.NewCanzoneId;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("RemoveTrackFromPlaylist")]
        public IActionResult RemoveTrackFromPlaylist([FromBody] AsCanzonePlaylistModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var asCanzonePlaylistEntity = _context.AsCanzonePlaylist
                .FirstOrDefault(ac => ac.AscpPlaylistFk == model.AcPlaylistId && ac.AscpCanzoneFk == model.AcCanzoneId);

            if (asCanzonePlaylistEntity == null)
            {
                return NotFound("Track not found in playlist");
            }

            _context.AsCanzonePlaylist.Remove(asCanzonePlaylistEntity);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Route("ReorderTrackInPlaylist")]
        public IActionResult ReorderTrackInPlaylist([FromBody] ReorderTrackModel model)
        {
            // La colonna TrackOrder non esiste nel DB corrente
            return StatusCode(501, "Track order non supportato: colonna mancante nel database.");
        }

        [HttpGet]
        [Route("GetTracksInPlaylistOrdered")]
        public IActionResult GetTracksInPlaylistOrdered([FromQuery] string playlistId)
        {
            // La colonna TrackOrder non esiste nel DB corrente
            return StatusCode(501, "Track order non supportato: colonna mancante nel database.");
        }

        [HttpPost]
        [Route("ShufflePlaylistTracks")]
        public IActionResult ShufflePlaylistTracks([FromQuery] string playlistId)
        {
            // La colonna TrackOrder non esiste nel DB corrente
            return StatusCode(501, "Track order non supportato: colonna mancante nel database.");
        }
    }

    public class ReorderTrackModel
    {
        public required string PlaylistId { get; set; }
        public required string CanzoneId { get; set; }
        public int NewOrder { get; set; }
    }
}