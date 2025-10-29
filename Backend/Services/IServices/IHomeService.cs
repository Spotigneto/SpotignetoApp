using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IHomeService
    {
        Task<(IReadOnlyList<ItemModel> songs, IReadOnlyList<ItemModel> artists, IReadOnlyList<ItemModel> playlists)> RandomViewAsync(int songs = 6, int artists = 6, int playlists = 6);
    }
}
