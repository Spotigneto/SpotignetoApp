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

        /// <summary>
        /// Ottiene tutti gli artisti seguiti da un utente
        /// </summary>
        [HttpGet("utente/{utenteId}")]
        public async Task<ActionResult<IEnumerable<AsUtenteArtistaModel>>> GetByUtenteId(long utenteId)
        {
            try
            {
                var relations = await _service.GetByUtenteIdAsync(utenteId);
                return Ok(relations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }

        /// <summary>
        /// Ottiene tutti i follower di un artista
        /// </summary>
        [HttpGet("artista/{artistaId}")]
        public async Task<ActionResult<IEnumerable<AsUtenteArtistaModel>>> GetByArtistaId(long artistaId)
        {
            try
            {
                var relations = await _service.GetByArtistaIdAsync(artistaId);
                return Ok(relations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se un utente segue un artista
        /// </summary>
        [HttpGet("utente/{utenteId}/artista/{artistaId}")]
        public async Task<ActionResult<AsUtenteArtistaModel>> GetByUtenteAndArtista(long utenteId, long artistaId)
        {
            try
            {
                var relation = await _service.GetByUtenteAndArtistaAsync(utenteId, artistaId);
                if (relation == null)
                    return NotFound($"L'utente {utenteId} non segue l'artista {artistaId}");

                return Ok(relation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se un utente segue un artista (restituisce solo true/false)
        /// </summary>
        [HttpGet("utente/{utenteId}/artista/{artistaId}/following")]
        public async Task<ActionResult<bool>> IsFollowing(long utenteId, long artistaId)
        {
            try
            {
                var isFollowing = await _service.IsFollowingAsync(utenteId, artistaId);
                return Ok(isFollowing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }

        /// <summary>
        /// Fa seguire un artista a un utente
        /// </summary>
        [HttpPost("utente/{utenteId}/artista/{artistaId}/follow")]
        public async Task<ActionResult> FollowArtista(long utenteId, long artistaId)
        {
            try
            {
                var success = await _service.FollowArtistaAsync(utenteId, artistaId);
                if (!success)
                    return BadRequest("L'utente segue già questo artista");

                return Ok("Artista seguito con successo");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
        }

        /// <summary>
        /// Fa smettere di seguire un artista a un utente
        /// </summary>
        [HttpDelete("utente/{utenteId}/artista/{artistaId}/unfollow")]
        public async Task<ActionResult> UnfollowArtista(long utenteId, long artistaId)
        {
            try
            {
                var success = await _service.UnfollowArtistaAsync(utenteId, artistaId);
                if (!success)
                    return NotFound("L'utente non segue questo artista");

                return Ok("Artista non più seguito");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore interno del server: {ex.Message}");
            }
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