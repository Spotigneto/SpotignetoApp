using Backend.Models;
using Backend.Services;

namespace Backend.Services
{
    public class QueueService : IQueueService
    {
        private readonly ICanzoneService _canzoneService;
        private readonly IPlaylistService _playlistService;
        private readonly IAlbumService _albumService;
        private readonly IAsAlbumCanzoneService _asAlbumCanzoneService;
        private static QueueModel _currentQueue = new QueueModel();
        private static PlayerStateModel _playerState = new PlayerStateModel();

        public QueueService(
            ICanzoneService canzoneService,
            IPlaylistService playlistService,
            IAlbumService albumService,
            IAsAlbumCanzoneService asAlbumCanzoneService)
        {
            _canzoneService = canzoneService;
            _playlistService = playlistService;
            _albumService = albumService;
            _asAlbumCanzoneService = asAlbumCanzoneService;
        }

        // Gestione coda
        public async Task<QueueModel> GetCurrentQueueAsync()
        {
            return _currentQueue;
        }

        public async Task<bool> SetQueueFromPlaylistAsync(long playlistId)
        {
            try
            {
                var playlist = await _playlistService.GetByIdAsync(playlistId);
                if (playlist == null) return false;

                // Ottieni le canzoni della playlist (implementazione semplificata)
                var canzoni = await _canzoneService.GetAllAsync();
                
                _currentQueue = new QueueModel
                {
                    Id = playlistId,
                    Items = canzoni.Select((c, index) => new QueueItemModel
                    {
                        CanzoneId = c.Id,
                        Nome = c.Nome,
                        File = c.File,
                        Durata = c.Durata,
                        Secondi = c.Secondi,
                        OriginalIndex = index
                    }).ToList(),
                    CurrentIndex = 0,
                    Mode = _playerState.Mode
                };

                _playerState.Queue = _currentQueue;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetQueueFromAlbumAsync(long albumId)
        {
            try
            {
                var album = await _albumService.GetByIdAsync(albumId);
                if (album == null) return false;

                // Ottieni le canzoni dell'album
                var albumCanzoni = await _asAlbumCanzoneService.GetAllAsync();
                var canzoniIds = albumCanzoni.Where(ac => ac.AlbumId == albumId).Select(ac => ac.CanzoneId);
                
                var canzoni = await _canzoneService.GetAllAsync();
                var albumTracks = canzoni.Where(c => canzoniIds.Contains(c.Id)).ToList();

                _currentQueue = new QueueModel
                {
                    Id = albumId,
                    Items = albumTracks.Select((c, index) => new QueueItemModel
                    {
                        CanzoneId = c.Id,
                        Nome = c.Nome,
                        File = c.File,
                        Durata = c.Durata,
                        Secondi = c.Secondi,
                        OriginalIndex = index
                    }).ToList(),
                    CurrentIndex = 0,
                    Mode = _playerState.Mode
                };

                _playerState.Queue = _currentQueue;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddToQueueAsync(long canzoneId)
        {
            try
            {
                var canzone = await _canzoneService.GetByIdAsync(canzoneId);
                if (canzone == null) return false;

                var queueItem = new QueueItemModel
                {
                    CanzoneId = canzone.Id,
                    Nome = canzone.Nome,
                    File = canzone.File,
                    Durata = canzone.Durata,
                    Secondi = canzone.Secondi,
                    OriginalIndex = _currentQueue.Items.Count
                };

                _currentQueue.Items.Add(queueItem);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveFromQueueAsync(int index)
        {
            if (index < 0 || index >= _currentQueue.Items.Count) return false;

            _currentQueue.Items.RemoveAt(index);
            
            // Aggiusta l'indice corrente se necessario
            if (_currentQueue.CurrentIndex >= index && _currentQueue.CurrentIndex > 0)
            {
                _currentQueue.CurrentIndex--;
            }

            return true;
        }

        public async Task<bool> ClearQueueAsync()
        {
            _currentQueue.Items.Clear();
            _currentQueue.CurrentIndex = 0;
            _playerState.IsPlaying = false;
            _playerState.IsPaused = false;
            _playerState.CurrentCanzoneId = null;
            return true;
        }

        public async Task<bool> ReorderQueueAsync(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= _currentQueue.Items.Count ||
                toIndex < 0 || toIndex >= _currentQueue.Items.Count)
                return false;

            var item = _currentQueue.Items[fromIndex];
            _currentQueue.Items.RemoveAt(fromIndex);
            _currentQueue.Items.Insert(toIndex, item);

            // Aggiusta l'indice corrente
            if (_currentQueue.CurrentIndex == fromIndex)
            {
                _currentQueue.CurrentIndex = toIndex;
            }
            else if (fromIndex < _currentQueue.CurrentIndex && toIndex >= _currentQueue.CurrentIndex)
            {
                _currentQueue.CurrentIndex--;
            }
            else if (fromIndex > _currentQueue.CurrentIndex && toIndex <= _currentQueue.CurrentIndex)
            {
                _currentQueue.CurrentIndex++;
            }

            return true;
        }

        // Controlli di riproduzione
        public async Task<PlayerStateModel> GetPlayerStateAsync()
        {
            return _playerState;
        }

        public async Task<bool> PlayAsync()
        {
            if (_currentQueue.Items.Count == 0) return false;

            _playerState.IsPlaying = true;
            _playerState.IsPaused = false;
            
            if (_playerState.CurrentCanzoneId == null && _currentQueue.Items.Count > 0)
            {
                _playerState.CurrentCanzoneId = _currentQueue.Items[_currentQueue.CurrentIndex].CanzoneId;
            }

            return true;
        }

        public async Task<bool> PauseAsync()
        {
            _playerState.IsPlaying = false;
            _playerState.IsPaused = true;
            return true;
        }

        public async Task<bool> StopAsync()
        {
            _playerState.IsPlaying = false;
            _playerState.IsPaused = false;
            _playerState.CurrentPosition = 0;
            return true;
        }

        public async Task<bool> NextTrackAsync()
        {
            if (_currentQueue.Items.Count == 0) return false;

            switch (_currentQueue.Mode)
            {
                case PlaybackMode.Sequential:
                    if (_currentQueue.CurrentIndex < _currentQueue.Items.Count - 1)
                    {
                        _currentQueue.CurrentIndex++;
                    }
                    else
                    {
                        return false; // Fine della coda
                    }
                    break;

                case PlaybackMode.Loop:
                    _currentQueue.CurrentIndex = (_currentQueue.CurrentIndex + 1) % _currentQueue.Items.Count;
                    break;

                case PlaybackMode.LoopSingle:
                    // Rimane sulla stessa traccia
                    break;

                case PlaybackMode.Random:
                    if (_currentQueue.IsShuffled && _currentQueue.ShuffleOrder.Count > 0)
                    {
                        var currentShuffleIndex = _currentQueue.ShuffleOrder.IndexOf(_currentQueue.CurrentIndex);
                        if (currentShuffleIndex < _currentQueue.ShuffleOrder.Count - 1)
                        {
                            _currentQueue.CurrentIndex = _currentQueue.ShuffleOrder[currentShuffleIndex + 1];
                        }
                        else
                        {
                            _currentQueue.CurrentIndex = _currentQueue.ShuffleOrder[0]; // Ricomincia
                        }
                    }
                    else
                    {
                        var random = new Random();
                        _currentQueue.CurrentIndex = random.Next(_currentQueue.Items.Count);
                    }
                    break;
            }

            _playerState.CurrentCanzoneId = _currentQueue.Items[_currentQueue.CurrentIndex].CanzoneId;
            _playerState.CurrentPosition = 0;
            return true;
        }

        public async Task<bool> PreviousTrackAsync()
        {
            if (_currentQueue.Items.Count == 0) return false;

            switch (_currentQueue.Mode)
            {
                case PlaybackMode.Sequential:
                    if (_currentQueue.CurrentIndex > 0)
                    {
                        _currentQueue.CurrentIndex--;
                    }
                    else
                    {
                        return false; // Inizio della coda
                    }
                    break;

                case PlaybackMode.Loop:
                    _currentQueue.CurrentIndex = _currentQueue.CurrentIndex == 0 
                        ? _currentQueue.Items.Count - 1 
                        : _currentQueue.CurrentIndex - 1;
                    break;

                case PlaybackMode.LoopSingle:
                    // Rimane sulla stessa traccia
                    break;

                case PlaybackMode.Random:
                    if (_currentQueue.IsShuffled && _currentQueue.ShuffleOrder.Count > 0)
                    {
                        var currentShuffleIndex = _currentQueue.ShuffleOrder.IndexOf(_currentQueue.CurrentIndex);
                        if (currentShuffleIndex > 0)
                        {
                            _currentQueue.CurrentIndex = _currentQueue.ShuffleOrder[currentShuffleIndex - 1];
                        }
                        else
                        {
                            _currentQueue.CurrentIndex = _currentQueue.ShuffleOrder[_currentQueue.ShuffleOrder.Count - 1];
                        }
                    }
                    else
                    {
                        var random = new Random();
                        _currentQueue.CurrentIndex = random.Next(_currentQueue.Items.Count);
                    }
                    break;
            }

            _playerState.CurrentCanzoneId = _currentQueue.Items[_currentQueue.CurrentIndex].CanzoneId;
            _playerState.CurrentPosition = 0;
            return true;
        }

        public async Task<bool> SeekToAsync(int positionSeconds)
        {
            if (positionSeconds < 0) return false;
            
            _playerState.CurrentPosition = positionSeconds;
            return true;
        }

        public async Task<bool> SetVolumeAsync(float volume)
        {
            if (volume < 0 || volume > 1) return false;
            
            _playerState.Volume = volume;
            return true;
        }

        // Modalità di riproduzione
        public async Task<bool> SetPlaybackModeAsync(PlaybackMode mode)
        {
            _currentQueue.Mode = mode;
            _playerState.Mode = mode;
            
            // Se si passa a random, genera l'ordine shuffle
            if (mode == PlaybackMode.Random && !_currentQueue.IsShuffled)
            {
                await ToggleShuffleAsync();
            }
            
            return true;
        }

        public async Task<bool> ToggleShuffleAsync()
        {
            if (_currentQueue.IsShuffled)
            {
                // Disabilita shuffle
                _currentQueue.IsShuffled = false;
                _currentQueue.ShuffleOrder.Clear();
            }
            else
            {
                // Abilita shuffle
                _currentQueue.IsShuffled = true;
                _currentQueue.ShuffleOrder = Enumerable.Range(0, _currentQueue.Items.Count)
                    .OrderBy(x => Guid.NewGuid())
                    .ToList();
            }
            
            return true;
        }

        public async Task<bool> ToggleRepeatAsync()
        {
            switch (_currentQueue.Mode)
            {
                case PlaybackMode.Sequential:
                    _currentQueue.Mode = PlaybackMode.Loop;
                    break;
                case PlaybackMode.Loop:
                    _currentQueue.Mode = PlaybackMode.LoopSingle;
                    break;
                case PlaybackMode.LoopSingle:
                    _currentQueue.Mode = PlaybackMode.Sequential;
                    break;
                default:
                    _currentQueue.Mode = PlaybackMode.Sequential;
                    break;
            }
            
            _playerState.Mode = _currentQueue.Mode;
            return true;
        }

        // Navigazione coda
        public async Task<bool> PlayTrackAtIndexAsync(int index)
        {
            if (index < 0 || index >= _currentQueue.Items.Count) return false;

            _currentQueue.CurrentIndex = index;
            _playerState.CurrentCanzoneId = _currentQueue.Items[index].CanzoneId;
            _playerState.CurrentPosition = 0;
            _playerState.IsPlaying = true;
            _playerState.IsPaused = false;
            
            return true;
        }

        public async Task<CanzoneModel?> GetCurrentTrackAsync()
        {
            if (_currentQueue.Items.Count == 0 || _currentQueue.CurrentIndex >= _currentQueue.Items.Count)
                return null;

            var currentItem = _currentQueue.Items[_currentQueue.CurrentIndex];
            return await _canzoneService.GetByIdAsync(currentItem.CanzoneId);
        }

        public async Task<CanzoneModel?> GetNextTrackAsync()
        {
            if (_currentQueue.Items.Count == 0) return null;

            int nextIndex;
            switch (_currentQueue.Mode)
            {
                case PlaybackMode.Sequential:
                    nextIndex = _currentQueue.CurrentIndex + 1;
                    if (nextIndex >= _currentQueue.Items.Count) return null;
                    break;
                case PlaybackMode.Loop:
                    nextIndex = (_currentQueue.CurrentIndex + 1) % _currentQueue.Items.Count;
                    break;
                case PlaybackMode.LoopSingle:
                    nextIndex = _currentQueue.CurrentIndex;
                    break;
                case PlaybackMode.Random:
                    if (_currentQueue.IsShuffled && _currentQueue.ShuffleOrder.Count > 0)
                    {
                        var currentShuffleIndex = _currentQueue.ShuffleOrder.IndexOf(_currentQueue.CurrentIndex);
                        nextIndex = currentShuffleIndex < _currentQueue.ShuffleOrder.Count - 1
                            ? _currentQueue.ShuffleOrder[currentShuffleIndex + 1]
                            : _currentQueue.ShuffleOrder[0];
                    }
                    else
                    {
                        var random = new Random();
                        nextIndex = random.Next(_currentQueue.Items.Count);
                    }
                    break;
                default:
                    return null;
            }

            var nextItem = _currentQueue.Items[nextIndex];
            return await _canzoneService.GetByIdAsync(nextItem.CanzoneId);
        }

        public async Task<CanzoneModel?> GetPreviousTrackAsync()
        {
            if (_currentQueue.Items.Count == 0) return null;

            int prevIndex;
            switch (_currentQueue.Mode)
            {
                case PlaybackMode.Sequential:
                    prevIndex = _currentQueue.CurrentIndex - 1;
                    if (prevIndex < 0) return null;
                    break;
                case PlaybackMode.Loop:
                    prevIndex = _currentQueue.CurrentIndex == 0 
                        ? _currentQueue.Items.Count - 1 
                        : _currentQueue.CurrentIndex - 1;
                    break;
                case PlaybackMode.LoopSingle:
                    prevIndex = _currentQueue.CurrentIndex;
                    break;
                case PlaybackMode.Random:
                    if (_currentQueue.IsShuffled && _currentQueue.ShuffleOrder.Count > 0)
                    {
                        var currentShuffleIndex = _currentQueue.ShuffleOrder.IndexOf(_currentQueue.CurrentIndex);
                        prevIndex = currentShuffleIndex > 0
                            ? _currentQueue.ShuffleOrder[currentShuffleIndex - 1]
                            : _currentQueue.ShuffleOrder[_currentQueue.ShuffleOrder.Count - 1];
                    }
                    else
                    {
                        var random = new Random();
                        prevIndex = random.Next(_currentQueue.Items.Count);
                    }
                    break;
                default:
                    return null;
            }

            var prevItem = _currentQueue.Items[prevIndex];
            return await _canzoneService.GetByIdAsync(prevItem.CanzoneId);
        }
    }
}