using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;

        public DatabaseController(SpotigneteDbContext context)
        {
            _context = context;
        }

        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            try
        {
                // Test database connection
                await _context.Database.CanConnectAsync();
                
                var connectionString = _context.Database.GetConnectionString();
                
                return Ok(new 
                {
                    Status = "Success",
                    Message = "Database connection successful",
                    ConnectionString = connectionString?.Replace("Password=", "Password=***") // Hide password for security
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new 
                {
                    Status = "Error",
                    Message = "Database connection failed",
                    Error = ex.Message
                });
            }
        }
    }
}