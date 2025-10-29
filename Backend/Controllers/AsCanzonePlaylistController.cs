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

            // Se non è specificato un ordine, usa il prossimo numero disponibile
            if (!model.TrackOrder.HasValue)
            {
                var maxOrder = _context.AsCanzonePlaylist
                    .Where(ac => ac.AscpPlaylistFk == model.AcPlaylistId)
                    .Max(ac => (int?)ac.AscpTrackOrder) ?? 0;
                model.TrackOrder = maxOrder + 1;
            }

            var asCanzonePlaylistEntity = new AsCanzonePlaylistEntity
            {
                AscpPlaylistFk = model.AcPlaylistId,
                AscpCanzoneFk = model.AcCanzoneId,
                AscpTrackOrder = model.TrackOrder
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
                .OrderBy(ac => ac.AscpTrackOrder)
                .Select(ac => new { 
                    CanzoneId = ac.AscpCanzoneFk, 
                    TrackOrder = ac.AscpTrackOrder 
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trackToMove = _context.AsCanzonePlaylist
                .FirstOrDefault(ac => ac.AscpPlaylistFk == model.PlaylistId && ac.AscpCanzoneFk == model.CanzoneId);

            if (trackToMove == null)
            {
                return NotFound("Track not found in playlist");
            }

            var oldOrder = trackToMove.AscpTrackOrder ?? 0;
            var newOrder = model.NewOrder;

            // Aggiorna l'ordine delle altre tracce
            if (newOrder > oldOrder)
            {
                // Sposta verso il basso: decrementa l'ordine delle tracce tra oldOrder e newOrder
                var tracksToUpdate = _context.AsCanzonePlaylist
                    .Where(ac => ac.AscpPlaylistFk == model.PlaylistId && 
                                ac.AscpTrackOrder > oldOrder && 
                                ac.AscpTrackOrder <= newOrder)
                    .ToList();

                foreach (var track in tracksToUpdate)
                {
                    track.AscpTrackOrder = (track.AscpTrackOrder ?? 0) - 1;
                }
            }
            else if (newOrder < oldOrder)
            {
                // Sposta verso l'alto: incrementa l'ordine delle tracce tra newOrder e oldOrder
                var tracksToUpdate = _context.AsCanzonePlaylist
                    .Where(ac => ac.AscpPlaylistFk == model.PlaylistId && 
                                ac.AscpTrackOrder >= newOrder && 
                                ac.AscpTrackOrder < oldOrder)
                    .ToList();

                foreach (var track in tracksToUpdate)
                {
                    track.AscpTrackOrder = (track.AscpTrackOrder ?? 0) + 1;
                }
            }

            // Aggiorna l'ordine della traccia spostata
            trackToMove.AscpTrackOrder = newOrder;
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("GetTracksInPlaylistOrdered")]
        public IActionResult GetTracksInPlaylistOrdered([FromQuery] string playlistId)
        {
            var tracks = _context.AsCanzonePlaylist
                .Where(ac => ac.AscpPlaylistFk == playlistId)
                .Join(_context.Canzoni,
                      ac => ac.AscpCanzoneFk,
                      c => c.CaId,
                      (ac, c) => new { 
                          TrackOrder = ac.AscpTrackOrder ?? 0,
                          Canzone = c 
                      })
                .OrderBy(x => x.TrackOrder)
                .Select(x => x.Canzone)
                .ToList();

            return Ok(tracks);
        }

        [HttpPost]
        [Route("ShufflePlaylistTracks")]
        public IActionResult ShufflePlaylistTracks([FromQuery] string playlistId)
        {
            var tracks = _context.AsCanzonePlaylist
                .Where(ac => ac.AscpPlaylistFk == playlistId)
                .ToList();

            if (!tracks.Any())
            {
                return NotFound("No tracks found in playlist");
            }

            // Mescola l'ordine delle tracce
            var random = new Random();
            var shuffledOrders = Enumerable.Range(1, tracks.Count).OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < tracks.Count; i++)
            {
                tracks[i].AscpTrackOrder = shuffledOrders[i];
            }

            _context.SaveChanges();
            return Ok();
        }
    }

    public class ReorderTrackModel
    {
        public required string PlaylistId { get; set; }
        public required string CanzoneId { get; set; }
        public int NewOrder { get; set; }
    }
}