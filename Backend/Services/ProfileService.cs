using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;

namespace Backend.Services
{
    public class ProfileService : IProfileService
    {
        private readonly SpotigneteDbContext _context;
        public ProfileService(SpotigneteDbContext context) { _context = context; }

        public async Task<(IReadOnlyList<AlbumItemModel> albums, IReadOnlyList<ItemModel> playlists)> SearchFiltriAsync(string? q, bool? albumPubblica, DateTime? albumFrom, DateTime? albumTo, bool? playlistPrivata)
        {
            var qp = (q ?? "").Trim();
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            var albumsSql = "SELECT al_id, al_nome, al_pubblica, al_release_date FROM Album WHERE 1=1";
            var aPars = new List<(string, object)>();
            if (!string.IsNullOrEmpty(qp)) { albumsSql += " AND al_nome LIKE @aq + '%'"; aPars.Add(("@aq", qp)); }
            if (albumPubblica.HasValue) { albumsSql += " AND al_pubblica = @ap"; aPars.Add(("@ap", albumPubblica.Value)); }
            if (albumFrom.HasValue) { albumsSql += " AND al_release_date >= @af"; aPars.Add(("@af", albumFrom.Value)); }
            if (albumTo.HasValue) { albumsSql += " AND al_release_date <= @at"; aPars.Add(("@at", albumTo.Value)); }
            albumsSql += " ORDER BY al_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            var playlistsSql = "SELECT pl_id, pl_nome FROM Playlist WHERE 1=1";
            var pPars = new List<(string, object)>();
            if (!string.IsNullOrEmpty(qp)) { playlistsSql += " AND pl_nome LIKE @pq + '%'"; pPars.Add(("@pq", qp)); }
            if (playlistPrivata.HasValue) { playlistsSql += " AND pl_privata = @pp"; pPars.Add(("@pp", playlistPrivata.Value)); }
            playlistsSql += " ORDER BY pl_nome OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY";

            var albums = await QueryAlbumsAsync(conn, albumsSql, aPars);
            var playlists = await QueryItemsAsync(conn, playlistsSql, pPars);

            return (albums, playlists);
        }

        private static async Task<List<ItemModel>> QueryItemsAsync(System.Data.Common.DbConnection conn, string sql, List<(string, object)> ps)
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
                list.Add(new ItemModel { Id = rdr.GetInt64(0), Nome = rdr.GetString(1) });
            }
            return list;
        }

        private static async Task<List<AlbumItemModel>> QueryAlbumsAsync(System.Data.Common.DbConnection conn, string sql, List<(string, object)> ps)
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
            var list = new List<AlbumItemModel>();
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                var id = rdr.GetInt64(0);
                var nome = rdr.GetString(1);
                var pub = rdr.GetBoolean(2);
                DateTime? rd = rdr.IsDBNull(3) ? (DateTime?)null : rdr.GetDateTime(3);
                list.Add(new AlbumItemModel { Id = id, Nome = nome, Pubblica = pub, ReleaseDate = rd });
            }
            return list;
        }
    }
}
