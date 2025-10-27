using Microsoft.AspNetCore.Mvc;
using SpotignetoApp.Data;
using SpotignetoApp.Models;

namespace SpotignetoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanzoneController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;

        public CanzoneController(SpotigneteDbContext context)
        {
            _context = context;
        }

        // POST: api/Records/canzone
        [HttpPost("canzone")]
        public ActionResult<CanzoneEntity> PostCanzone(CanzoneEntity canzone)
        {
            _context.Canzoni.Add(canzone);
            _context.SaveChanges();

            return CreatedAtAction("GetCanzone", new { id = canzone.CId }, canzone);
        }

        // GET: api/Records/canzoni
        [HttpGet("canzoni")]
        public ActionResult<IEnumerable<CanzoneEntity>> GetCanzoni()
        {
            return _context.Canzoni.ToList();
        }

        // GET: api/Records/canzone/{id}
        [HttpGet("canzone/{id}")]
        public ActionResult<CanzoneEntity> GetCanzone(int id)
        {
            var canzone = _context.Canzoni.Find(id);

            if (canzone == null)
            {
                return NotFound();
            }

            return canzone;
        }

        // PUT: api/Records/canzone/{id}
        [HttpPut("canzone/{id}")]
        public ActionResult<CanzoneEntity> PutCanzone(int id, CanzoneEntity canzone)
        {
            if (id != canzone.CId)
            {
                return BadRequest();
            }

            _context.Entry(canzone).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Records/canzone/{id}
        [HttpDelete("canzone/{id}")]
        public ActionResult<CanzoneEntity> DeleteCanzone(int id)
        {
            var canzone = _context.Canzoni.Find(id);
            if (canzone == null)
            {
                return NotFound();
            }

            _context.Canzoni.Remove(canzone);
            _context.SaveChanges();

            return canzone;
        }
    }
}