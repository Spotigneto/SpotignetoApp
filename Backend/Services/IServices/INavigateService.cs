using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface INavigateService
    {
        Task<(IReadOnlyList<ItemModel> songs, IReadOnlyList<ItemModel> playlists, IReadOnlyList<ItemModel> artists)> SearchFiltriAsync(string? q, long? genereId, long? sottoGenereId, bool? playlistPrivata);
        Task<(IReadOnlyList<ItemModel> songs, IReadOnlyList<ItemModel> playlists, IReadOnlyList<ItemModel> artists)> LettereAsync(string q);
    }
}
