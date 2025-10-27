using Microsoft.AspNetCore.Mvc;
using SpotignetoApp.Data;
using SpotignetoApp.Models;

namespace SpotignetoApp.Contoller;
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsCanzonePlaylistController : ControllerBase
    {
        private readonly SpotignetoAppContext _context;

        public AsCanzonePlaylistController(SpotignetoAppContext context)
        {
            _context = context;
        }
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
            AcPlaylistId = model.AcPlaylistId,
            AcCanzoneId = model.AcCanzoneId
        };

        _context.AsCanzonePlaylist.Add(asCanzonePlaylistEntity);
        _context.SaveChanges();

        return Ok();
    }

    [HttpGet]
    [Route("GetTracksInPlaylist")]
    public IActionResult GetTracksInPlaylist([FromQuery] int playlistId)
    {
        var tracks = _context.AsCanzonePlaylist
            .Where(ac => ac.AcPlaylistId == playlistId)
            .Select(ac => ac.AcCanzoneId)
            .ToList();

        return Ok(tracks);
    }

    [HttpPut]
    [Route("UpdateTrackInPlaylist")]
    public IActionResult UpdateTrackFromPlaylist([FromBody])
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingEntity = _context.AsCanzonePlaylist
            .FirstOrDefault(ac => ac.AcPlaylistId == model.AcPlaylistId && ac.AcCanzoneId == model.OldCanzoneId);

        if (existingEntity == null)
        {
            return NotFound("Track not found in playlist");
        }

        existingEntity.AcCanzoneId = model.NewCanzoneId;
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
            .FirstOrDefault(ac => ac.AcPlaylistId == model.AcPlaylistId && ac.AcCanzoneId == model.AcCanzoneId);

        if (asCanzonePlaylistEntity == null)
        {
            return NotFound("Track not found in playlist");
        }

        _context.AsCanzonePlaylist.Remove(asCanzonePlaylistEntity);
        _context.SaveChanges();

        return Ok();
    }
}