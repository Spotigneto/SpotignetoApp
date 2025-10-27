using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Entities;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly SpotigneteDbContext _db;

        public ProfileController(SpotigneteDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _db.Profili.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var p = await _db.Profili.FindAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProfileEntity profile)
        {
            _db.Profili.Add(profile);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = profile.UtId }, profile);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ProfileEntity profile)
        {
            var existing = await _db.Profili.FindAsync(id);
            if (existing == null) return NotFound();
            existing.UtNome = profile.UtNome;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existing = await _db.Profili.FindAsync(id);
            if (existing == null) return NotFound();
            _db.Profili.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
