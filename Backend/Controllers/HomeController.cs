using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Entities;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly SpotigneteDbContext _db;
        private readonly Random _rng = new Random();

        public HomeController(SpotigneteDbContext db)
        {
            _db = db;
        }

        [HttpGet("random_view")]
        public async Task<IActionResult> RandomView([FromQuery] int songs = 10, [FromQuery] int artists = 5)
        {
            var songIds = await _db.Canzoni.Select(c => c.CaId).ToListAsync();
            var artistIds = await _db.Artisti.Select(a => a.ArId).ToListAsync();
            var chosenSongIds = PickRandom(songIds, songs);
            var chosenArtistIds = PickRandom(artistIds, artists);
            var chosenSongs = await _db.Canzoni.Where(c => chosenSongIds.Contains(c.CaId)).ToListAsync();
            var chosenArtists = await _db.Artisti.Where(a => chosenArtistIds.Contains(a.ArId)).ToListAsync();
            return Ok(new { songs = chosenSongs, artists = chosenArtists });
        }

        private List<long> PickRandom(List<long> source, int count)
        {
            if (source == null || source.Count == 0) return new List<long>();
            if (count >= source.Count) return new List<long>(source);
            var result = new List<long>(count);
            var set = new HashSet<int>();
            while (result.Count < count)
            {
                var idx = _rng.Next(source.Count);
                if (set.Add(idx)) result.Add(source[idx]);
            }
            return result;
        }
    }
}
