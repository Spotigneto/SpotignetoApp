using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistaController : ControllerBase
    {
        private readonly IArtistaService _service;

        public ArtistaController(IArtistaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistaModel>>> GetArtisti()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistaModel>> GetArtista(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ArtistaModel>> PostArtista(ArtistaModel model)
        {
            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetArtista), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtista(long id, ArtistaModel model)
        {
            var ok = await _service.UpdateAsync(id, model);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtista(long id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}