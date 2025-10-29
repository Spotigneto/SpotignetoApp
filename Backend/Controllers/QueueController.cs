using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _queueService;

        public QueueController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        // GET: api/Queue
        [HttpGet]
        public async Task<ActionResult<QueueModel>> GetCurrentQueue()
        {
            var queue = await _queueService.GetCurrentQueueAsync();
            return Ok(queue);
        }

        // POST: api/Queue/playlist/{id}
        [HttpPost("playlist/{id}")]
        public async Task<IActionResult> SetQueueFromPlaylist(string id)
        {
            var success = await _queueService.SetQueueFromPlaylistAsync(id);
            if (!success)
            {
                return NotFound("Playlist not found");
            }
            return Ok();
        }

        // POST: api/Queue/album/{id}
        [HttpPost("album/{id}")]
        public async Task<IActionResult> SetQueueFromAlbum(string id)
        {
            var success = await _queueService.SetQueueFromAlbumAsync(id);
            if (!success)
            {
                return NotFound("Album not found");
            }
            return Ok();
        }

        // POST: api/Queue/add/{canzoneId}
        [HttpPost("add/{canzoneId}")]
        public async Task<IActionResult> AddToQueue(string canzoneId)
        {
            var success = await _queueService.AddToQueueAsync(canzoneId);
            if (!success)
            {
                return NotFound("Canzone not found");
            }
            return Ok();
        }

        // DELETE: api/Queue/{index}
        [HttpDelete("{index:int}")]
        public async Task<IActionResult> RemoveFromQueue(int index)
        {
            var success = await _queueService.RemoveFromQueueAsync(index);
            if (!success)
            {
                return BadRequest("Invalid index");
            }
            return Ok();
        }

        // DELETE: api/Queue
        [HttpDelete]
        public async Task<IActionResult> ClearQueue()
        {
            await _queueService.ClearQueueAsync();
            return Ok();
        }

        // PUT: api/Queue/reorder
        [HttpPut("reorder")]
        public async Task<IActionResult> ReorderQueue([FromBody] ReorderRequest request)
        {
            var success = await _queueService.ReorderQueueAsync(request.FromIndex, request.ToIndex);
            if (!success)
            {
                return BadRequest("Invalid indices");
            }
            return Ok();
        }

        // GET: api/Queue/player/state
        [HttpGet("player/state")]
        public async Task<ActionResult<PlayerStateModel>> GetPlayerState()
        {
            var state = await _queueService.GetPlayerStateAsync();
            return Ok(state);
        }

        // POST: api/Queue/player/play
        [HttpPost("player/play")]
        public async Task<IActionResult> Play()
        {
            var success = await _queueService.PlayAsync();
            if (!success)
            {
                return BadRequest("Cannot play - queue is empty");
            }
            return Ok();
        }

        // POST: api/Queue/player/pause
        [HttpPost("player/pause")]
        public async Task<IActionResult> Pause()
        {
            await _queueService.PauseAsync();
            return Ok();
        }

        // POST: api/Queue/player/stop
        [HttpPost("player/stop")]
        public async Task<IActionResult> Stop()
        {
            await _queueService.StopAsync();
            return Ok();
        }

        // POST: api/Queue/player/next
        [HttpPost("player/next")]
        public async Task<IActionResult> NextTrack()
        {
            var success = await _queueService.NextTrackAsync();
            if (!success)
            {
                return BadRequest("Cannot go to next track");
            }
            return Ok();
        }

        // POST: api/Queue/player/previous
        [HttpPost("player/previous")]
        public async Task<IActionResult> PreviousTrack()
        {
            var success = await _queueService.PreviousTrackAsync();
            if (!success)
            {
                return BadRequest("Cannot go to previous track");
            }
            return Ok();
        }

        // POST: api/Queue/player/seek
        [HttpPost("player/seek")]
        public async Task<IActionResult> SeekTo([FromBody] SeekRequest request)
        {
            var success = await _queueService.SeekToAsync(request.PositionSeconds);
            if (!success)
            {
                return BadRequest("Invalid position");
            }
            return Ok();
        }

        // POST: api/Queue/player/volume
        [HttpPost("player/volume")]
        public async Task<IActionResult> SetVolume([FromBody] VolumeRequest request)
        {
            var success = await _queueService.SetVolumeAsync(request.Volume);
            if (!success)
            {
                return BadRequest("Invalid volume level");
            }
            return Ok();
        }

        // POST: api/Queue/mode/{mode}
        [HttpPost("mode/{mode}")]
        public async Task<IActionResult> SetPlaybackMode(PlaybackMode mode)
        {
            var success = await _queueService.SetPlaybackModeAsync(mode);
            if (!success)
            {
                return BadRequest("Invalid playback mode");
            }
            return Ok();
        }

        // POST: api/Queue/shuffle/toggle
        [HttpPost("shuffle/toggle")]
        public async Task<IActionResult> ToggleShuffle()
        {
            await _queueService.ToggleShuffleAsync();
            return Ok();
        }

        // POST: api/Queue/repeat/toggle
        [HttpPost("repeat/toggle")]
        public async Task<IActionResult> ToggleRepeat()
        {
            await _queueService.ToggleRepeatAsync();
            return Ok();
        }

        // POST: api/Queue/play/{index}
        [HttpPost("play/{index:int}")]
        public async Task<IActionResult> PlayTrackAtIndex(int index)
        {
            var success = await _queueService.PlayTrackAtIndexAsync(index);
            if (!success)
            {
                return BadRequest("Invalid track index");
            }
            return Ok();
        }

        // GET: api/Queue/current-track
        [HttpGet("current-track")]
        public async Task<ActionResult<CanzoneModel>> GetCurrentTrack()
        {
            var track = await _queueService.GetCurrentTrackAsync();
            if (track == null)
            {
                return NotFound("No current track");
            }
            return Ok(track);
        }

        // GET: api/Queue/next-track
        [HttpGet("next-track")]
        public async Task<ActionResult<CanzoneModel>> GetNextTrack()
        {
            var track = await _queueService.GetNextTrackAsync();
            if (track == null)
            {
                return NotFound("No next track");
            }
            return Ok(track);
        }

        // GET: api/Queue/previous-track
        [HttpGet("previous-track")]
        public async Task<ActionResult<CanzoneModel>> GetPreviousTrack()
        {
            var track = await _queueService.GetPreviousTrackAsync();
            if (track == null)
            {
                return NotFound("No previous track");
            }
            return Ok(track);
        }
    }

    // Modelli per le richieste
    public class ReorderRequest
    {
        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
    }

    public class SeekRequest
    {
        public int PositionSeconds { get; set; }
    }

    public class VolumeRequest
    {
        public float Volume { get; set; }
    }
}