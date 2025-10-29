using Backend.Models;

namespace Backend.Services
{
    public interface IQueueService
    {
        // Gestione coda
        Task<QueueModel> GetCurrentQueueAsync();
        Task<bool> SetQueueFromPlaylistAsync(long playlistId);
        Task<bool> SetQueueFromAlbumAsync(long albumId);
        Task<bool> AddToQueueAsync(long canzoneId);
        Task<bool> RemoveFromQueueAsync(int index);
        Task<bool> ClearQueueAsync();
        Task<bool> ReorderQueueAsync(int fromIndex, int toIndex);

        // Controlli di riproduzione
        Task<PlayerStateModel> GetPlayerStateAsync();
        Task<bool> PlayAsync();
        Task<bool> PauseAsync();
        Task<bool> StopAsync();
        Task<bool> NextTrackAsync();
        Task<bool> PreviousTrackAsync();
        Task<bool> SeekToAsync(int positionSeconds);
        Task<bool> SetVolumeAsync(float volume);

        // Modalità di riproduzione
        Task<bool> SetPlaybackModeAsync(PlaybackMode mode);
        Task<bool> ToggleShuffleAsync();
        Task<bool> ToggleRepeatAsync();

        // Navigazione coda
        Task<bool> PlayTrackAtIndexAsync(int index);
        Task<CanzoneModel?> GetCurrentTrackAsync();
        Task<CanzoneModel?> GetNextTrackAsync();
        Task<CanzoneModel?> GetPreviousTrackAsync();
    }
}