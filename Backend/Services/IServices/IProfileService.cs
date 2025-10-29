using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IProfileService
    {
        Task<(IReadOnlyList<AlbumItemModel> albums, IReadOnlyList<ItemModel> playlists)> SearchFiltriAsync(
            string? q, bool? albumPubblica, DateTime? albumFrom, DateTime? albumTo, bool? playlistPrivata);

        Task<List<ProfileModel>> GetAllAsync();
        Task<ProfileModel?> GetByIdAsync(long id);
        Task<ProfileModel> CreateAsync(ProfileModel model);
        Task<bool> UpdateAsync(long id, ProfileModel model);
        Task<bool> DeleteAsync(long id);
    }
}