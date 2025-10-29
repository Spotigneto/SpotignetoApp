using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanzoneController : ControllerBase
    {
        private readonly ICanzoneService _service;
        public CanzoneController(ICanzoneService service)
        {
            _service = service;
        }

        // POST: api/Canzone/canzone
        [HttpPost("canzone")]
        public async Task<ActionResult<CanzoneModel>> PostCanzone(CanzoneModel canzone)
        {
            var created = await _service.CreateAsync(canzone);
            return CreatedAtAction(nameof(GetCanzone), new { id = created.Id }, created);
        }

        // GET: api/Canzone/canzoni
        [HttpGet("canzoni")]
        public async Task<ActionResult<IEnumerable<CanzoneModel>>> GetCanzoni()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        // GET: api/Canzone/canzone/{id}
        [HttpGet("canzone/{id:long}")]
        public async Task<ActionResult<CanzoneModel>> GetCanzone(long id)
        {
            var canzone = await _service.GetByIdAsync(id);
            if (canzone == null)
            {
                return NotFound();
            }
            return Ok(canzone);
        }

        // GET: api/Canzone/canzone/{name}
        [HttpGet("canzone/by-name/{name}")]
        public async Task<ActionResult<CanzoneModel>> GetCanzoneByName(string name)
        {
            var canzone = await _service.GetByNameAsync(name);
            if (canzone == null)
            {
                return NotFound();
            }
            return Ok(canzone);
        }

        // PUT: api/Canzone/canzone/{id}
        [HttpPut("canzone/{id}")]
        public async Task<IActionResult> PutCanzone(long id, CanzoneModel canzone)
        {
            var ok = await _service.UpdateAsync(id, canzone);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Canzone/canzone/{id}
        [HttpDelete("canzone/{id}")]
        public async Task<IActionResult> DeleteCanzone(long id)
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