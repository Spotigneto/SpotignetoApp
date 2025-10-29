using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using System.Net.Mime;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanzoneController : ControllerBase
    {
        private readonly ICanzoneService _service;
        private readonly IFileService _fileService;
        
        public CanzoneController(ICanzoneService service, IFileService fileService)
        {
            _service = service;
            _fileService = fileService;
        }

        [HttpPost("canzone")]
        public async Task<ActionResult<CanzoneModel>> PostCanzone(CanzoneModel canzone)
        {
            var created = await _service.CreateAsync(canzone);
            return CreatedAtAction(nameof(GetCanzone), new { id = created.Id }, created);
        }

        [HttpGet("canzoni")]
        public async Task<ActionResult<IEnumerable<CanzoneModel>>> GetCanzoni()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("canzone/{id}")]
        public async Task<ActionResult<CanzoneModel>> GetCanzone(string id)
        {
            var canzone = await _service.GetByIdAsync(id);
            if (canzone == null)
            {
                return NotFound();
            }
            return Ok(canzone);
        }

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

        [HttpPut("canzone/{id}")]
        public async Task<IActionResult> PutCanzone(string id, CanzoneModel canzone)
        {
            var ok = await _service.UpdateAsync(id, canzone);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("canzone/{id}")]
        public async Task<IActionResult> DeleteCanzone(string id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Canzone/upload/{id}
        [HttpPost("upload/{id}")]
        public async Task<IActionResult> UploadMp3File(string id, IFormFile file)
        {
            try
            {
                // Validate file using FileService
                if (!_fileService.ValidateMp3File(file))
                {
                    return BadRequest("Invalid MP3 file. Please ensure the file is a valid MP3 format and under 50MB.");
                }

                // Check if canzone exists
                var canzone = await _service.GetByIdAsync(id);
                if (canzone == null)
                {
                    return NotFound("Canzone not found");
                }

                // Delete old file if exists
                if (!string.IsNullOrEmpty(canzone.File))
                {
                    await _fileService.DeleteMp3FileAsync(canzone.File);
                }

                // Save new file using FileService
                var relativePath = await _fileService.SaveMp3FileAsync(file, id);

                // Update canzone with new file path
                canzone.File = relativePath;
                await _service.UpdateAsync(id, canzone);

                return Ok(new { 
                    message = "File uploaded successfully", 
                    filePath = relativePath,
                    fileName = Path.GetFileName(relativePath)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Canzone/stream/{id}
        [HttpGet("stream/{id}")]
        public async Task<IActionResult> StreamMp3(string id)
        {
            try
            {
                var canzone = await _service.GetByIdAsync(id);
                if (canzone == null)
                {
                    return NotFound("Canzone not found");
                }

                if (string.IsNullOrEmpty(canzone.File))
                {
                    return NotFound("No audio file associated with this canzone");
                }

                var fileStream = await _fileService.GetMp3FileStreamAsync(canzone.File);
                if (fileStream == null)
                {
                    return NotFound("Audio file not found on server");
                }

                var fileName = Path.GetFileName(canzone.File);
                return File(fileStream, "audio/mpeg", fileName, enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Canzone/download/{id}
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadMp3(string id)
        {
            try
            {
                var canzone = await _service.GetByIdAsync(id);
                if (canzone == null)
                {
                    return NotFound("Canzone not found");
                }

                if (string.IsNullOrEmpty(canzone.File))
                {
                    return NotFound("No audio file associated with this canzone");
                }

                var fileStream = await _fileService.GetMp3FileStreamAsync(canzone.File);
                if (fileStream == null)
                {
                    return NotFound("Audio file not found on server");
                }

                var fileName = $"{canzone.Nome}.mp3";
                return File(fileStream, MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}