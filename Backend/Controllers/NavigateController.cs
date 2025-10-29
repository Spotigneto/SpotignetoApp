using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigateController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;
        public NavigateController(SpotigneteDbContext context) { _context = context; }


        [HttpGet("Search_Filtri")]
        public async Task<IActionResult> Search_Filtri([FromQuery] string? q, [FromQuery] string? genereId, [FromQuery] string? sottoGenereId, [FromQuery] bool? playlistPrivata)
        {
            var qp = (q ?? "").Trim();
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            var songsSql = "SELECT ca_id AS Id, ca_nome AS Nome FROM Canzone WHERE 1=1";
            var songsPars = new List<(string, object)>();
            if (!string.IsNullOrEmpty(qp)) { songsSql += " AND ca_nome LIKE @q + '%'"; songsPars.Add(("@q", qp)); }
            if (!string.IsNullOrEmpty(genereId)) { songsSql += " AND ca_genere_fk = @g"; songsPars.Add(("@g", genereId)); }
            if (!string.IsNullOrEmpty(sottoGenereId)) { songsSql += " AND ca_sottogenere_fk = @sg"; songsPars.Add(("@sg", sottoGenereId)); }
            songsSql += " ORDER BY ca_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            var playlistsSql = "SELECT pl_id AS Id, pl_nome AS Nome FROM Playlist WHERE 1=1";
            var playlistsPars = new List<(string, object)>();
            if (!string.IsNullOrEmpty(qp)) { playlistsSql += " AND pl_nome LIKE @pq + '%'"; playlistsPars.Add(("@pq", qp)); }
            if (playlistPrivata.HasValue) { playlistsSql += " AND pl_privata = @pr"; playlistsPars.Add(("@pr", playlistPrivata.Value)); }
            playlistsSql += " ORDER BY pl_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            var artistsSql = "SELECT ar_id AS Id, ar_nome AS Nome FROM Artista";
            var artistsPars = new List<(string, object)>();
            if (!string.IsNullOrEmpty(qp)) { artistsSql += " WHERE ar_nome LIKE @aq + '%'"; artistsPars.Add(("@aq", qp)); }
            artistsSql += " ORDER BY ar_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            var songs = await QueryAsync(conn, songsSql, songsPars);
            var playlists = await QueryAsync(conn, playlistsSql, playlistsPars);
            var artists = await QueryAsync(conn, artistsSql, artistsPars);

            return Ok(new { songs, playlists, artists });
        }

        [HttpGet("Lettere")]
        public async Task<IActionResult> Lettere([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return BadRequest();
            var qp = q.Trim();
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            var songs = await QueryAsync(conn, "SELECT ca_id AS Id, ca_nome AS Nome FROM Canzone WHERE ca_nome LIKE @q + '%' ORDER BY ca_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY", new List<(string, object)> { ("@q", qp) });
            var playlists = await QueryAsync(conn, "SELECT pl_id AS Id, pl_nome AS Nome FROM Playlist WHERE pl_nome LIKE @q + '%' ORDER BY pl_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY", new List<(string, object)> { ("@q", qp) });
            var artists = await QueryAsync(conn, "SELECT ar_id AS Id, ar_nome AS Nome FROM Artista WHERE ar_nome LIKE @q + '%' ORDER BY ar_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY", new List<(string, object)> { ("@q", qp) });

            return Ok(new { songs, playlists, artists });
        }

        private static async Task<List<ItemModel>> QueryAsync(System.Data.Common.DbConnection conn, string sql, List<(string, object)> ps)
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
