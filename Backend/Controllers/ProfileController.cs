using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Services;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _service;
        public ProfileController(IProfileService service) { _service = service; }


        [HttpGet("Search_Filtri")]
        public async Task<IActionResult> Search_Filtri([FromQuery] string? q, [FromQuery] bool? albumPubblica, [FromQuery] DateTime? albumFrom, [FromQuery] DateTime? albumTo, [FromQuery] bool? playlistPrivata)
        {
            var (albums, playlists) = await _service.SearchFiltriAsync(q, albumPubblica, albumFrom, albumTo, playlistPrivata);
            return Ok(new { albums, playlists });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Backend.Models.ProfileModel model)
        {
            var created = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Backend.Models.ProfileModel model)
        {
            var ok = await _service.UpdateAsync(id, model);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }


    }
}
