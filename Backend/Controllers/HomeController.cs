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
        public async Task<IActionResult> Random_View(
            [FromQuery] int albums = 8,
            [FromQuery] int songs = 6,
            [FromQuery] int artists = 6,
            [FromQuery] int playlists = 6)
        {
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            var albumsList = await QueryAsync(conn,
                $"SELECT TOP (@t) al.al_id AS Id, al.al_nome AS Nome, STRING_AGG(ar.ar_nome, ', ') AS Artista " +
                "FROM Album al " +
                "LEFT JOIN as_artista_album aa ON aa.asaa_album_fk = al.al_id " +
                "LEFT JOIN Artista ar ON ar.ar_id = aa.asaa_artista_fk " +
                "GROUP BY al.al_id, al.al_nome " +
                "ORDER BY NEWID()",
                ("@t", albums));

            var songsList = await QueryAsync(conn,
                $"SELECT TOP (@t) ca.ca_id AS Id, ca.ca_nome AS Nome, STRING_AGG(ar.ar_nome, ', ') AS Artista " +
                "FROM Canzone ca " +
                "LEFT JOIN as_artista_canzone ac ON ac.asarc_canzone_fk = ca.ca_id " +
                "LEFT JOIN Artista ar ON ar.ar_id = ac.asarc_artista_fk " +
                "GROUP BY ca.ca_id, ca.ca_nome " +
                "ORDER BY NEWID()",
                ("@t", songs));
            var artistsList = await QueryAsync(conn, $"SELECT TOP (@t) ar_id AS Id, ar_nome AS Nome FROM Artista ORDER BY NEWID()", ("@t", artists));
            var playlistsList = await QueryAsync(conn, $"SELECT TOP (@t) pl_id AS Id, pl_nome AS Nome FROM Playlist ORDER BY NEWID()", ("@t", playlists));

            return Ok(new { albums = albumsList, songs = songsList, artists = artistsList, playlists = playlistsList });
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
                var item = new ItemModel { Id = rdr.GetString(0), Nome = rdr.GetString(1) };
                if (rdr.FieldCount >= 3)
                {
                    item.Artista = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2);
                }
                list.Add(item);
            }
            return list;
        }
    }
}
