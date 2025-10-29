using Backend.Models;
using Backend.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsUtenteArtistaController : ControllerBase
    {
        private readonly IAsUtenteArtistaService _service;

        public AsUtenteArtistaController(IAsUtenteArtistaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Ottiene tutte le relazioni utente-artista
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AsUtenteArtistaModel>>> GetAll()
        {
            try
            {
                var relations = await _service.GetAllAsync();
                return Ok(relations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }

        /// <summary>
        /// Ottiene una relazione utente-artista per ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AsUtenteArtistaModel>> GetById(long id)
        {
            try
            {
                var relation = await _service.GetByIdAsync(id);
                if (relation == null)
                    return NotFound($"Relazione con ID {id} non trovata");

                return Ok(relation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }

        [HttpGet("utente/{utenteId}")]
        public async Task<ActionResult<IEnumerable<AsUtenteArtistaModel>>> GetByUtenteId(string utenteId)
        {
            var result = await _service.GetByUtenteIdAsync(utenteId);
            return Ok(result);
        }

        // GET: api/AsUtenteArtista/artista/{artistaId}
        [HttpGet("artista/{artistaId}")]
        public async Task<ActionResult<IEnumerable<AsUtenteArtistaModel>>> GetByArtistaId(string artistaId)
        {
            var result = await _service.GetByArtistaIdAsync(artistaId);
            return Ok(result);
        }

        // GET: api/AsUtenteArtista/{utenteId}/{artistaId}
        [HttpGet("{utenteId}/{artistaId}")]
        public async Task<ActionResult<AsUtenteArtistaModel>> GetByUtenteAndArtista(string utenteId, string artistaId)
        {
            var result = await _service.GetByUtenteAndArtistaAsync(utenteId, artistaId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/AsUtenteArtista/isFollowing/{utenteId}/{artistaId}
        [HttpGet("isFollowing/{utenteId}/{artistaId}")]
        public async Task<ActionResult<bool>> IsFollowing(string utenteId, string artistaId)
        {
            var result = await _service.IsFollowingAsync(utenteId, artistaId);
            return Ok(result);
        }

        // POST: api/AsUtenteArtista/follow/{utenteId}/{artistaId}
        [HttpPost("follow/{utenteId}/{artistaId}")]
        public async Task<ActionResult> FollowArtista(string utenteId, string artistaId)
        {
            var result = await _service.FollowArtistaAsync(utenteId, artistaId);
            if (!result)
            {
                return BadRequest("Unable to follow artist");
            }
            return Ok();
        }

        // DELETE: api/AsUtenteArtista/unfollow/{utenteId}/{artistaId}
        [HttpDelete("unfollow/{utenteId}/{artistaId}")]
        public async Task<ActionResult> UnfollowArtista(string utenteId, string artistaId)
        {
            var result = await _service.UnfollowArtistaAsync(utenteId, artistaId);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        /// Crea una nuova relazione utente-artista
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AsUtenteArtistaModel>> Create([FromBody] AsUtenteArtistaModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Dati non validi");

                // Verifica se la relazione esiste già
                if (await _service.ExistsAsync(model.UtenteId, model.ArtistaId))
                    return BadRequest("La relazione esiste già");

                var created = await _service.CreateAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }

        /// <summary>
        /// Aggiorna una relazione utente-artista
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AsUtenteArtistaModel>> Update(long id, [FromBody] AsUtenteArtistaModel model)
        {
            try
            {
                if (model == null || model.Id != id)
                    return BadRequest("Dati non validi");

                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound($"Relazione con ID {id} non trovata");

                var updated = await _service.UpdateAsync(model);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina una relazione utente-artista per ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                if (!success)
                    return NotFound($"Relazione con ID {id} non trovata");

                return Ok("Relazione eliminata con successo");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }
    }
}