using Microsoft.JSInterop;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace Frontend.Services.Playback
{
    public enum RepeatMode { None, One, All }

    public class TrackItem
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Album { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Gradient { get; set; } = string.Empty;
        public string? File { get; set; }

        [JsonIgnore]
        public string DurationText => FormatDuration(DurationSeconds);

        public static string FormatDuration(int seconds)
        {
            if (seconds <= 0) return "";
            var m = seconds / 60;
            var s = seconds % 60;
            return $"{m}:{s:D2}";
        }
    }

    public class PlaybackService : IAsyncDisposable
    {
        private readonly IConfiguration _config;
        private IJSRuntime? _js;
        private IJSObjectReference? _module;
        private bool _attached;
        private string _apiBase;

        public PlaybackService(IConfiguration config)
        {
            _config = config;
            _apiBase = _config["ApiSettings:BaseUrl"] ?? "http://localhost:5232";
        }

        public event Action? StateChanged;

        public List<TrackItem> Queue { get; } = new();
        public List<TrackItem> RecentlyPlayed { get; } = new();
        public Dictionary<string, TrackItem> Cache { get; } = new();

        public TrackItem? NowPlaying { get; private set; }
        public int NowIndex { get; private set; } = -1;
        public bool IsPlaying { get; private set; }
        public bool Shuffle { get; private set; }
        public RepeatMode Repeat { get; private set; } = RepeatMode.None;

        public int ProgressPercent { get; private set; }
        public string CurrentTimeText { get; private set; } = "0:00";
        public string TotalTimeText { get; private set; } = "0:00";

        public void Initialize(IJSRuntime js)
        {
            _js = js;
        }

        private async Task EnsureModuleAsync()
        {
            if (_js == null) return;
            if (_module == null)
            {
                _module = await _js.InvokeAsync<IJSObjectReference>("import", "/js/audio.js");
                if (!_attached)
                {
                    try
                    {
                        await _module.InvokeVoidAsync("attach", DotNetObjectReference.Create(this));
                        _attached = true;
                    }
                    catch { }
                }
            }
        }

        public void SetShuffle(bool enabled)
        {
            Shuffle = enabled;
            Notify();
        }

        public void SetRepeat(RepeatMode mode)
        {
            Repeat = mode;
            Notify();
        }

        public void AddToQueue(TrackItem item)
        {
            Queue.Add(item);
            Cache[item.Id] = item;
            RecalculatePositions();
            Notify();
        }

        public void RemoveFromQueue(string id)
        {
            var idx = Queue.FindIndex(t => t.Id == id);
            if (idx >= 0)
            {
                Queue.RemoveAt(idx);
                RecalculatePositions();
                Notify();
            }
        }

        public void ClearQueue()
        {
            Queue.Clear();
            Notify();
        }

        public void ReorderQueue(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex || fromIndex < 0 || toIndex < 0 || fromIndex >= Queue.Count || toIndex >= Queue.Count) return;
            var item = Queue[fromIndex];
            Queue.RemoveAt(fromIndex);
            Queue.Insert(toIndex, item);
            RecalculatePositions();
            Notify();
        }

        public void ShuffleQueue()
        {
            var rnd = new Random();
            var shuffled = Queue.OrderBy(_ => rnd.Next()).ToList();
            Queue.Clear();
            Queue.AddRange(shuffled);
            RecalculatePositions();
            Notify();
        }

        private void RecalculatePositions()
        {
            for (int i = 0; i < Queue.Count; i++)
            {
                // position is implicit via index; UI reads index + 1
            }
        }

        public async Task PlayAsync(TrackItem item)
        {
            NowPlaying = item;
            NowIndex = Queue.FindIndex(t => t.Id == item.Id);
            await StartAudioAsync(item);
            IsPlaying = true;
            Notify();
        }

        public void SetNowPlayingInfo(TrackItem item)
        {
            NowPlaying = item;
            NowIndex = Queue.FindIndex(t => t.Id == item.Id);
            IsPlaying = false;
            Notify();
        }

        public async Task TogglePlayAsync()
        {
            if (!IsPlaying)
            {
                await ResumeAudioAsync();
                IsPlaying = true;
            }
            else
            {
                await PauseAudioAsync();
                IsPlaying = false;
            }
            Notify();
        }

        public async Task StopAsync()
        {
            await EnsureModuleAsync();
            if (_module != null)
            {
                await _module.InvokeVoidAsync("stop");
            }
            IsPlaying = false;
            CurrentTimeText = "0:00";
            ProgressPercent = 0;
            Notify();
        }

        public async Task SkipNextAsync()
        {
            if (Queue.Count == 0) return;
            if (Shuffle)
            {
                var rnd = new Random();
                var next = Queue[rnd.Next(Queue.Count)];
                await PlayAsync(next);
                return;
            }

            if (NowIndex < 0 || NowIndex + 1 >= Queue.Count)
            {
                if (Repeat == RepeatMode.All && Queue.Count > 0)
                {
                    NowIndex = 0;
                    await PlayAsync(Queue[NowIndex]);
                }
                else
                {
                    await StopAsync();
                }
            }
            else
            {
                NowIndex++;
                await PlayAsync(Queue[NowIndex]);
            }
        }

        private async Task StartAudioAsync(TrackItem item)
        {
            await EnsureModuleAsync();
            if (_module == null) return;
            string url;
            if (!string.IsNullOrWhiteSpace(item.File))
            {
                // Combine base URL with relative file path from DB (e.g. "/audio/songs/..")
                var path = item.File!.StartsWith("/") ? item.File : "/" + item.File;
                url = _apiBase.TrimEnd('/') + path;
            }
            else
            {
                // Fallback to streaming endpoint by id
                url = $"{_apiBase}/api/Canzone/stream/{item.Id}";
            }
            await _module.InvokeVoidAsync("setSource", url);
            await _module.InvokeVoidAsync("play");
        }

        private async Task ResumeAudioAsync()
        {
            await EnsureModuleAsync();
            if (_module == null) return;
            await _module.InvokeVoidAsync("play");
        }

        private async Task PauseAudioAsync()
        {
            await EnsureModuleAsync();
            if (_module == null) return;
            await _module.InvokeVoidAsync("pause");
        }

        public void UpdateProgress(double currentSeconds, double totalSeconds)
        {
            CurrentTimeText = TrackItem.FormatDuration((int)Math.Floor(currentSeconds));
            TotalTimeText = TrackItem.FormatDuration((int)Math.Floor(totalSeconds));
            ProgressPercent = totalSeconds > 0 ? (int)Math.Round(currentSeconds / totalSeconds * 100) : 0;
            Notify();
        }

        [JSInvokable]
        public void OnAudioTimeUpdate(double currentSeconds, double totalSeconds)
        {
            UpdateProgress(currentSeconds, totalSeconds);
        }

        [JSInvokable]
        public async Task OnAudioEnded()
        {
            await SkipNextAsync();
        }

        public async Task SkipPreviousAsync()
        {
            if (Queue.Count == 0) return;
            if (Shuffle)
            {
                var rnd = new Random();
                var prev = Queue[rnd.Next(Queue.Count)];
                await PlayAsync(prev);
                return;
            }

            if (NowIndex <= 0)
            {
                if (Repeat == RepeatMode.All && Queue.Count > 0)
                {
                    NowIndex = Queue.Count - 1;
                    await PlayAsync(Queue[NowIndex]);
                }
                else
                {
                    await StopAsync();
                }
            }
            else
            {
                NowIndex--;
                await PlayAsync(Queue[NowIndex]);
            }
        }

        public async Task SeekAsync(int seconds)
        {
            await EnsureModuleAsync();
            if (_module == null) return;
            await _module.InvokeVoidAsync("seek", seconds);
        }

        public async Task SetVolumeAsync(float volume)
        {
            await EnsureModuleAsync();
            if (_module == null) return;
            await _module.InvokeVoidAsync("setVolume", volume);
        }

        public ValueTask DisposeAsync()
        {
            if (_module != null)
            {
                try
                {
                    return _module.DisposeAsync();
                }
                catch { }
            }
            return ValueTask.CompletedTask;
        }

        private void Notify() => StateChanged?.Invoke();
    }
}