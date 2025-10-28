using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly SpotigneteDbContext _context;
        public ProfileController(SpotigneteDbContext context) { _context = context; }

        public class ItemDto
        {
            public long Id { get; set; }
            public string Nome { get; set; } = "";
        }

        public class AlbumDto : ItemDto
        {
            public bool Pubblica { get; set; }
            public DateTime? ReleaseDate { get; set; }
        }

        [HttpGet("Search_Filtri")]
        public async Task<IActionResult> Search_Filtri([FromQuery] string? q, [FromQuery] bool? albumPubblica, [FromQuery] DateTime? albumFrom, [FromQuery] DateTime? albumTo, [FromQuery] bool? playlistPrivata)
        {
            var qp = (q ?? "").Trim();
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            var albumsSql = "SELECT al_id AS Id, al_nome AS Nome, al_pubblica AS Pubblica, al_release_date AS ReleaseDate FROM Album WHERE 1=1";
            var aPars = new List<(string, object)>();
            if (!string.IsNullOrEmpty(qp)) { albumsSql += " AND al_nome LIKE @aq + '%'"; aPars.Add(("@aq", qp)); }
            if (albumPubblica.HasValue) { albumsSql += " AND al_pubblica = @ap"; aPars.Add(("@ap", albumPubblica.Value)); }
            if (albumFrom.HasValue) { albumsSql += " AND al_release_date >= @af"; aPars.Add(("@af", albumFrom.Value)); }
            if (albumTo.HasValue) { albumsSql += " AND al_release_date <= @at"; aPars.Add(("@at", albumTo.Value)); }
            albumsSql += " ORDER BY al_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            var playlistsSql = "SELECT pl_id AS Id, pl_nome AS Nome FROM Playlist WHERE 1=1";
            var pPars = new List<(string, object)>();
            if (!string.IsNullOrEmpty(qp)) { playlistsSql += " AND pl_nome LIKE @pq + '%'"; pPars.Add(("@pq", qp)); }
            if (playlistPrivata.HasValue) { playlistsSql += " AND pl_privata = @pp"; pPars.Add(("@pp", playlistPrivata.Value)); }
            playlistsSql += " ORDER BY pl_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            var albums = await QueryAlbumsAsync(conn, albumsSql, aPars);
            var playlists = await QueryItemsAsync(conn, playlistsSql, pPars);

            return Ok(new { albums, playlists });
        }

        private static async Task<List<ItemDto>> QueryItemsAsync(System.Data.Common.DbConnection conn, string sql, List<(string, object)> ps)
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
            var list = new List<ItemDto>();
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                var id = rdr.GetInt64(0);
                var nome = rdr.GetString(1);
                list.Add(new ItemDto { Id = id, Nome = nome });
            }
            return list;
        }

        private static async Task<List<AlbumDto>> QueryAlbumsAsync(System.Data.Common.DbConnection conn, string sql, List<(string, object)> ps)
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
            var list = new List<AlbumDto>();
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                var id = rdr.GetInt64(0);
                var nome = rdr.GetString(1);
                var pub = rdr.GetBoolean(2);
                DateTime? rd = rdr.IsDBNull(3) ? (DateTime?)null : rdr.GetDateTime(3);
                list.Add(new AlbumDto { Id = id, Nome = nome, Pubblica = pub, ReleaseDate = rd });
            }
            return list;
        }
    }
}
