using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using System.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;
        public HomeController(SpotigneteDbContext context) { _context = context; }


        [HttpGet("Random_View")]
        public async Task<IActionResult> Random_View([FromQuery] int songs = 6, [FromQuery] int artists = 6, [FromQuery] int playlists = 6)
        {
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            var songsList = await QueryAsync(conn, $"SELECT TOP (@t) ca_id AS Id, ca_nome AS Nome FROM Canzone ORDER BY NEWID()", ("@t", songs));
            var artistsList = await QueryAsync(conn, $"SELECT TOP (@t) ar_id AS Id, ar_nome AS Nome FROM Artista ORDER BY NEWID()", ("@t", artists));
            var playlistsList = await QueryAsync(conn, $"SELECT TOP (@t) pl_id AS Id, pl_nome AS Nome FROM Playlist ORDER BY NEWID()", ("@t", playlists));

            return Ok(new { songs = songsList, artists = artistsList, playlists = playlistsList });
        }

        private static async Task<List<ItemModel>> QueryAsync(System.Data.Common.DbConnection conn, string sql, params (string, object)[] ps)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            foreach (var p in ps)
            {
                var par = cmd.CreateParameter();
                par.ParameterName = p.Item1;
                par.Value = p.Item2 ?? DBNull.Value;
                cmd.Parameters.Add(par);
            }
            var list = new List<ItemModel>();
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new ItemModel { Id = rdr.GetInt64(0).ToString(), Nome = rdr.GetString(1) });
            }
            return list;
        }
    }
}
